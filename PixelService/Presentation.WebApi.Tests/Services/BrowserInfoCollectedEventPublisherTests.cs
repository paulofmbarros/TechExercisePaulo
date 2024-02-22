

namespace Presentation.WebApi.Tests.Services;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Presentation.WebApi.Services.BrowserInfoCollectedEventPublisher;

public class BrowserInfoCollectedEventPublisherTests
{
    [Fact]
    public async Task PublishAsync_Should_ProduceMessageToKafka()
    {
        // Arrange
        var referrer = "https://example.com";
        var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3";
        var ipAddress = "192.168.1.1";

        var kafkaProducerMock = new Mock<IProducer<string, string>>();
        var loggerMock = new Mock<ILogger<BrowserInfoCollectedEventPublisher>>();

        var publisher = new BrowserInfoCollectedEventPublisher(kafkaProducerMock.Object, loggerMock.Object);

        // Act
        await publisher.PublishAsync(referrer, userAgent, ipAddress);

        // Assert
        kafkaProducerMock.Verify(
            p => p.ProduceAsync("browser-info-collected", It.IsAny<Message<string, string>>(),new CancellationToken()),
            Times.Once);
    }

    [Fact]
    public async Task PublishAsync_Should_LogErrorAndThrowException_OnKafkaError()
    {
        // Arrange
        var referrer = "https://example.com";
        var userAgent = "Mozilla/5.0";
        var ipAddress = "192.168.1.1";

        var kafkaProducerMock = new Mock<IProducer<string, string>>();
        kafkaProducerMock.Setup(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<string, string>>(), new CancellationToken()))
            .ThrowsAsync(new Exception("Kafka error"));

        var loggerMock = new Mock<ILogger<BrowserInfoCollectedEventPublisher>>();

        var publisher = new BrowserInfoCollectedEventPublisher(kafkaProducerMock.Object, loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => publisher.PublishAsync(referrer, userAgent, ipAddress));

    }
}