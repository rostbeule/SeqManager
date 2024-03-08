using Abstractions.Data;

namespace Services.Data;

internal sealed class ImportResult : IImportResult
{
    public bool Success => Error == string.Empty;
    
    public string Error { get; init; } = string.Empty;
}