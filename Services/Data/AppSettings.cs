using Abstractions.Data;

namespace Services.Data;

internal sealed class AppSettings : IAppSettings
{
    public bool OpenSeqAfterImport { get; set; } = true;
    
    public string WebServiceUrl { get; set; } = "http://localhost:5341";
    
    public string LogFileDirectory { get; set; } = @"C:\temp\SeqManager";
    
    public string SeqCliPath { get; set; } = @"C:\Program Files\Seq\Client\seqcli.exe";
    
    public string IngestLogFiles => $"ingest -i {LogFileDirectory}\\*.json --json";
}