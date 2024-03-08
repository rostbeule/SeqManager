namespace Abstractions.Data;

/// <summary>
/// Represents application settings.
/// </summary>
public interface IAppSettings
{
    /// <summary>
    /// Gets or sets whether to open Seq after import.
    /// </summary>
    bool OpenSeqAfterImport { get; set; }

    /// <summary>
    /// Gets the URL of the web service.
    /// </summary>
    string WebServiceUrl { get; set; }

    /// <summary>
    /// Gets the directory path for log files to ingest.
    /// </summary>
    string LogFileDirectory { get; set; }

    /// <summary>
    /// Gets the path to the Seq CLI executable.
    /// </summary>
    string SeqCliPath { get; set; }

    /// <summary>
    /// Gets the command for ingesting log files.
    /// </summary>
    string IngestLogFiles { get; }
}