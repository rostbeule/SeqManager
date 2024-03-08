using System.Diagnostics;
using Abstractions;
using Abstractions.Data;
using Services.Data;

namespace Services;

public sealed class LogFileImportServiceService(
    IAppSettingsService appSettingsService) 
    : ILogFileImportService
{
    private readonly List<string> selectedFilePaths = [];

    public bool TryAddFilePath(string filePath, out string fileName)
    {
        if (selectedFilePaths.Contains(filePath))
        {
            fileName = string.Empty;
            return false;
        }
        
        selectedFilePaths.Add(filePath);
        fileName = Path.GetFileName(filePath);
        return true;
    }

    public void ClearLogFilePaths() 
        => selectedFilePaths.Clear();

    public void RemoveLogFilePathAt(int index) 
        => selectedFilePaths.RemoveAt(index);

    public async Task<IImportResult> TryStartAsync()
    {
        return await Task.Run(() =>
        {
            var settings = appSettingsService.Settings;
            var seqCliPath = settings.SeqCliPath;
            var webServiceUrl = settings.WebServiceUrl;
            var ingestLogFiles = settings.IngestLogFiles;
            var logFileDirectory = settings.LogFileDirectory;
            var openSeqAfterImport = settings.OpenSeqAfterImport;
            
            PrepareDirectory(logFileDirectory);
            
            if (TryCopySelectedFiles(logFileDirectory, out var copyError) is false)
            {
                return new ImportResult { Error = copyError };
            }

            if (TryImportLogFilesToSeq(seqCliPath, ingestLogFiles, out var importError) is false)
            {
                return new ImportResult { Error = importError };
            };

            if (openSeqAfterImport is false)
            {
                return new ImportResult();
            }
            
            if (TryOpenSeqInBrowser(webServiceUrl, out var openError) is false)
            {
                return new ImportResult { Error = openError };
            };

            return new ImportResult();
        });
    }
    
    private static void PrepareDirectory(string logFileDirectory)
    {
        if (Directory.Exists(logFileDirectory) is false)
        {
            Directory.CreateDirectory(logFileDirectory);
        }
        else
        {
            var directoryInfo = new DirectoryInfo(logFileDirectory);
            foreach (var file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
        }
    }
    
    private bool TryCopySelectedFiles(string logFileDirectory, out string copyError)
    {
        copyError = string.Empty;
        
        foreach (var filePath in selectedFilePaths)
        {
            try
            {
                var fileName = Path.GetFileName(filePath);
                var destinationPath = Path.Combine(logFileDirectory, fileName);

                using var sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var destinationStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);
                sourceStream.CopyTo(destinationStream);
            }
            catch (Exception ex)
            {
                copyError = $"Error copying file: {ex.Message}";
            }
        }

        return copyError == string.Empty;
    }
    
    private static bool TryImportLogFilesToSeq(string fileName, string arguments, out string importError)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();
        
        var error = process.StandardError.ReadToEnd();
        importError = string.IsNullOrEmpty(error)
            ? string.Empty
            : $"Error: {error}";
        
        return importError == string.Empty;
    }

    private static bool TryOpenSeqInBrowser(string url, out string openError)
    {
        try
        {
            using var client = new HttpClient();
            using var response = client.GetAsync(url).Result;
            
            if (response.IsSuccessStatusCode)
            {
                Process.Start(
                    new ProcessStartInfo(url)
                    {
                        UseShellExecute = true
                    });

                openError = string.Empty;
            }
            else
            {
                openError = $"Error opening Seq: HTTP {response.StatusCode}";
            }
        }
        catch (Exception exception)
        {
            openError = $"Error opening Seq: {exception.Message}";
        }

        return openError == string.Empty;
    }
}