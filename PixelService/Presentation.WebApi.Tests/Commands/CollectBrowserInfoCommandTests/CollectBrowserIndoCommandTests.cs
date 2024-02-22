using AutoFixture;
using Domain.AggregateModels.Image;
using FluentAssertions;
using Moq;
using Presentation.WebApi.Commands.CollectBrowserInfoCommand;
using Presentation.WebApi.Services.BrowserInfoCollectedEventPublisher;

namespace Presentation.WebApi.Tests.Commands.CollectBrowserInfoCommandTests;

public class CollectBrowserIndoCommandTests
{
    [Fact]
    public async Task Handle_Should_PublishToKafkaAndReturnCorrectOutput()
    {
        // Arrange
        var referrer = "https://example.com";
        var userAgent = "Mozilla/5.0";

        var imageContent = "R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";

        var ipAddress = "192.168.1.1";

        var transparentPixel = Convert.FromBase64String(imageContent);

        var kafkaPublisherMock = new Mock<IBrowserInfoCollectedEventPublisher>();
        var imageRepositoryMock = new Mock<IImageRepository>();

        imageRepositoryMock.Setup(repo => repo.GetImageContent()).Returns(imageContent);

        var handler = new CollectBrowserInfoCommandHandler(kafkaPublisherMock.Object, imageRepositoryMock.Object);

        // Act
        var result = await handler.Handle(
            new CollectBrowserInfoCommand { Referrer = referrer, UserAgent = userAgent },
            CancellationToken.None);

        // Assert
        kafkaPublisherMock.Verify(
            p => p.PublishAsync(referrer, userAgent, ipAddress),
            Times.Once);

        result.Should().NotBeNull();
        result.ContentType.Should().Be("image/gif");
        result.Content.Should().BeEquivalentTo(transparentPixel);
    }

    [Fact]
    public async Task Handle_Should_LogErrorAndThrowException_OnKafkaError()
    {
        // Arrange
        var referrer = "https://example.com";
        var userAgent = "Mozilla/5.0";
        var imageContent = "R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";


        var kafkaPublisherMock = new Mock<IBrowserInfoCollectedEventPublisher>();
        kafkaPublisherMock.Setup(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("Kafka error"));

        var imageRepositoryMock = new Mock<IImageRepository>();

        imageRepositoryMock.Setup(repo => repo.GetImageContent()).Returns(imageContent);

        var handler = new CollectBrowserInfoCommandHandler(kafkaPublisherMock.Object, imageRepositoryMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(
            new CollectBrowserInfoCommand { Referrer = referrer, UserAgent = userAgent },
            CancellationToken.None));


    }
}