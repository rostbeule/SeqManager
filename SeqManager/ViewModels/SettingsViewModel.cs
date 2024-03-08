using System.ComponentModel;
using Abstractions;

namespace SeqManager.ViewModels;

public sealed class SettingsViewModel : INotifyPropertyChanged
{
    private bool openSeqAfterImport;
    private string seqCliPath = string.Empty;
    private string webServiceUrl = string.Empty;
    private string logFileDirectory = string.Empty;

    public string WebServiceUrl
    {
        get => webServiceUrl;
        set
        {
            if (webServiceUrl == value)
            {
                return;
            }

            webServiceUrl = value;
            OnPropertyChanged(nameof(WebServiceUrl));
        }
    }
    
    public string LogFileDirectory
    {
        get => logFileDirectory;
        set
        {
            if (logFileDirectory == value)
            {
                return;
            }

            logFileDirectory = value;
            OnPropertyChanged(nameof(LogFileDirectory));
        }
    }

    public string SeqCliPath
    {
        get => seqCliPath;
        set
        {
            if (seqCliPath == value)
            {
                return;
            }

            seqCliPath = value;
            OnPropertyChanged(nameof(SeqCliPath));
        }
    }

    public bool OpenSeqAfterImport
    {
        get => openSeqAfterImport;
        set
        {
            if (openSeqAfterImport == value)
            {
                return;
            }

            openSeqAfterImport = value;
            OnPropertyChanged(nameof(OpenSeqAfterImport));
        }
    }

    public void Reset(IAppSettingsService service)
    {
        var settings = service.Settings;
        SeqCliPath = settings.SeqCliPath;
        WebServiceUrl = settings.WebServiceUrl;
        LogFileDirectory = settings.LogFileDirectory;
        OpenSeqAfterImport = settings.OpenSeqAfterImport;
    }

    public void SaveChanges(IAppSettingsService service)
    {
        var settings = service.Settings;
        settings.SeqCliPath = SeqCliPath;
        settings.WebServiceUrl = WebServiceUrl;
        settings.LogFileDirectory = LogFileDirectory;
        settings.OpenSeqAfterImport = OpenSeqAfterImport;
        service.Save(settings);
    }

    // Not required right now =)
    public event PropertyChangedEventHandler? PropertyChanged;
    
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}