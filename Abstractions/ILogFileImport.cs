namespace Abstractions;

public interface ILogFileImport
{
    bool TryAddFilePath(string filePath, out string fileName);
    void ClearLogFilePaths();
    void RemoveLogFilePathAt(int selectedIndex);
    Task StartAsync();
}