namespace Domain;

public class VisitMetaData
{
    public string Referrer { get; private set; }

    public string UserAgent { get; private set; }

    public string IpAddress { get; private set; }

    public string Timestamp { get; private set; } = DateTime.UtcNow.ToString("O");

    public VisitMetaData(string referrer, string userAgent, string ipAddress, string timestamp)
    {
        Referrer = referrer;
        UserAgent = userAgent;
        Timestamp = timestamp;

        //Todo: The validation can be done in better way instead of raising exception
        if (string.IsNullOrEmpty(ipAddress))
        {
            throw new NullReferenceException("IP Address cannot be null or empty");
        }

        IpAddress = ipAddress;
    }
}