using Abstractions.Data;

namespace Abstractions;

/// <summary>
/// Represents a service for importing log files.
/// </summary>
public interface ILogFileImportService
{
    /// <summary>
    /// Tries to add a file path to the log file import.
    /// </summary>
    /// <param name="filePath">The path of the file to be added.</param>
    /// <param name="fileName">The name of the added file.</param>
    /// <returns>True if the file path was added successfully, otherwise false.</returns>
    bool TryAddFilePath(string filePath, out string fileName);

    /// <summary>
    /// Clears all the log file paths.
    /// </summary>
    void ClearLogFilePaths();

    /// <summary>
    /// Removes the log file path at the specified index.
    /// </summary>
    /// <param name="selectedIndex">The index of the log file path to be removed.</param>
    void RemoveLogFilePathAt(int selectedIndex);

    /// <summary>
    /// Attempts to start the log file import asynchronously.
    /// </summary>
    /// <returns>Returns an <see cref="IImportResult"/> indicating the result of the operation.</returns>
    Task<IImportResult> TryStartAsync();


}