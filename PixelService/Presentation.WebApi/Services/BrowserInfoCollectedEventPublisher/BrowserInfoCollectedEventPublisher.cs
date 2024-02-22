using Confluent.Kafka;
using Newtonsoft.Json;

namespace Presentation.WebApi.Services.BrowserInfoCollectedEventPublisher;

public class BrowserInfoCollectedEventPublisher : IBrowserInfoCollectedEventPublisher
{
    private readonly IProducer<string, string> _kafkaProducer;
    private readonly ILogger<BrowserInfoCollectedEventPublisher> logger;

    public BrowserInfoCollectedEventPublisher(IProducer<string, string> kafkaProducer,
        ILogger<BrowserInfoCollectedEventPublisher> logger)
    {
        _kafkaProducer = kafkaProducer;
        this.logger = logger;
    }

    public async Task PublishAsync( string referrer, string userAgent, string ipAddress)
    {
        try
        {
            this.logger.LogInformation("publishing message to Kafka topic browser-info-collected");

            var message = new Message<string, string>
            {
                Key = "BrowserInfoCollectedKey",
                Value = JsonConvert.SerializeObject(new { Referrer = referrer, UserAgent = userAgent, IpAddress = ipAddress })
            };
            await _kafkaProducer.ProduceAsync("browser-info-collected-topic", message);


            this.logger.LogInformation("Message kafka published with success");
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error publishing message to Kafka");
            throw;
        }
    }
}