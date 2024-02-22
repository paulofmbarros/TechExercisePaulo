using Domain;

namespace Infrastructure.Repositories;

public class VisitMetadataRepository : IVisitMetadataRepository
{
    private readonly string _visitsLogFilePath;

    public VisitMetadataRepository(string visitsLogFilePath)
    {
        _visitsLogFilePath = visitsLogFilePath;
    }

    public async Task SaveAsync(VisitMetaData visitMetaData)
    {
        var formattedData = $"{visitMetaData.Timestamp:O}|{visitMetaData.Referrer ?? "null"}|{visitMetaData.UserAgent ?? "null"}|{visitMetaData.IpAddress}";
        await File.AppendAllLinesAsync(_visitsLogFilePath, new[] { formattedData });
    }
}