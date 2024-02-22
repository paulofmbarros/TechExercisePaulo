namespace Presentation.WebApi.Services.BrowserInfoCollectedEventPublisher;

public interface IBrowserInfoCollectedEventPublisher
{
    /// <summary>
    /// Publishes the message to the Kafka topic.
    /// </summary>
    /// <param name="requestReferrer"></param>
    /// <param name="referrer"></param>
    /// <param name="userAgent"></param>
    /// <returns></returns>
    Task PublishAsync( string referrer, string userAgent, string ipAddress);
}