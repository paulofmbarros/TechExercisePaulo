using FluentAssertions;

namespace Domain.Tests;

public class VisitMetadataTests
{
    [Fact]
    public void Constructor_ValidInput_ShouldCreateInstance()
    {
        // Arrange
        var referrer = "https://example.com";
        var userAgent = "Mozilla/5.0";
        var ipAddress = "192.168.1.1";
        var timestamp = DateTime.UtcNow.ToString("O");

        // Act
        var visitMetaData = new VisitMetaData(referrer, userAgent, ipAddress, timestamp);

        // Assert
        visitMetaData.Referrer.Should().Be(referrer);
        visitMetaData.UserAgent.Should().Be(userAgent);
        visitMetaData.IpAddress.Should().Be(ipAddress);
        visitMetaData.Timestamp.Should().Be(timestamp);
    }

    [Fact]
    public void Constructor_NullIpAddress_ShouldThrowArgumentNullException()
    {
        // Arrange
        var referrer = "https://example.com";
        var userAgent = "Mozilla/5.0";
        var timestamp = DateTime.UtcNow.ToString("O");

        // Act & Assert
        Action act = () => new VisitMetaData(referrer, userAgent, null, timestamp);
        act.Should().Throw<NullReferenceException>().WithMessage("IP Address cannot be null or empty");
    }

    [Fact]
    public void Constructor_EmptyIpAddress_ShouldThrowArgumentNullException()
    {
        // Arrange
        var referrer = "https://example.com";
        var userAgent = "Mozilla/5.0";
        var timestamp = DateTime.UtcNow.ToString("O");

        // Act & Assert
        Action act = () => new VisitMetaData(referrer, userAgent, string.Empty, timestamp);
        act.Should().Throw<NullReferenceException>().WithMessage("IP Address cannot be null or empty");
    }
}