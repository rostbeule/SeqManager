namespace Abstractions.Data;

/// <summary>
/// Represents the result of an import operation, indicating whether the operation was successful.
/// </summary>
public interface IImportResult
{
    /// <summary>
    /// Gets a value indicating whether the import operation was successful.
    /// </summary>
    bool Success { get; }

    /// <summary>
    /// Gets an error message associated with the import operation if it failed; otherwise, returns null.
    /// </summary>
    string Error { get; }
}