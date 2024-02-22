using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Presentation.WebApi;
using Presentation.WebApi.Commands;
using Presentation.WebApi.Dtos;

public class KafkaConsumerService : BackgroundService
{
    private readonly ILogger<KafkaConsumerService> logger;

    private readonly IConsumer<string,string> consumer;

    private readonly IServiceScopeFactory _scopeFactory;


    public KafkaConsumerService(
        ILogger<KafkaConsumerService> logger,
        IConsumer<string, string> consumer,
        IServiceScopeFactory scopeFactory)
    {
        this.logger = logger;
        this.consumer = consumer;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        consumer.Subscribe("browser-info-collected-topic");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                this.logger.LogInformation("Consuming message from Kafka");
                var consumeResult = consumer.Consume(stoppingToken);

                var dateAccessed = consumeResult.Timestamp.UtcDateTime
                    .ToString(GlobalConstants.TimeStampFormat, System.Globalization.CultureInfo.InvariantCulture);

                if (consumeResult.IsPartitionEOF)
                    continue;

                // Process the consumed message
                this.logger.LogInformation($"Received message: {consumeResult.Message.Value}");

                var visitData = JsonConvert.DeserializeObject<CollectBrowserInfoInputDto>(consumeResult.Message.Value);

                var command = new StoreVisitCommand
                {
                    CollectBrowserInfoInputDto = visitData,
                    TimeStamp = dateAccessed

                };

                using var scope = this._scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var result = await mediator.Send(command);

                this.logger.LogInformation("Message consumed with success");
            }
            catch (OperationCanceledException)
            {
                // Graceful shutdown
                break;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error consuming message from Kafka");
            }
        }
    }
}