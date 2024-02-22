using AutoFixture;
using Domain;
using Microsoft.Extensions.Logging;
using Moq;
using Presentation.WebApi.Commands;

namespace Presentation.WebApi.Tests.Commands.StoreVisitCommandTests;

public class StoreVisitCommandHandlerTests
{
     private readonly Fixture _fixture;
        private readonly Mock<IVisitMetadataRepository> _visitMetadataRepositoryMock;
        private readonly Mock<ILogger<StoreVisitCommandHandler>> _loggerMock;

        public StoreVisitCommandHandlerTests()
        {
            _fixture = new Fixture();
            _visitMetadataRepositoryMock = new Mock<IVisitMetadataRepository>();
            _loggerMock = new Mock<ILogger<StoreVisitCommandHandler>>();
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldSaveVisitMetadata()
        {
            // Arrange
            var sut = new StoreVisitCommandHandler(
                _visitMetadataRepositoryMock.Object,
                _loggerMock.Object);

            var request = _fixture.Create<StoreVisitCommand>();

            // Act
            await sut.Handle(request, CancellationToken.None);

            // Assert
            _visitMetadataRepositoryMock.Verify(
                repo => repo.SaveAsync(It.IsAny<VisitMetaData>()),
                Times.Once);

        }

        [Fact]
        public async Task Handle_ExceptionThrown_ShouldLogErrorAndRethrow()
        {
            // Arrange
            var sut = new StoreVisitCommandHandler(
                _visitMetadataRepositoryMock.Object,
                _loggerMock.Object);

            var request = _fixture.Create<StoreVisitCommand>();

            _visitMetadataRepositoryMock
                .Setup(repo => repo.SaveAsync(It.IsAny<VisitMetaData>()))
                .ThrowsAsync(new Exception("Simulated error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => sut.Handle(request, CancellationToken.None));
            
        }
}