using SftpClient.Configuration;

namespace SftpClient.Tests.Configuration;

public class ConnectionConfigurationTests
{
    [Fact]
    public void Constructor_ValidParameters_CreatesInstance()
    {
        // Arrange
        const string host = "example.com";
        const string username = "testuser";
        const int port = 22;

        // Act
        var config = new ConnectionConfiguration(host, username, port);

        // Assert
        Assert.Equal(host, config.Host);
        Assert.Equal(username, config.Username);
        Assert.Equal(port, config.Port);
        Assert.Equal(30000, config.ConnectionTimeout);
        Assert.Equal(60000, config.OperationTimeout);
        Assert.True(config.KeepAlive);
    }

    [Fact]
    public void Constructor_DefaultPort_Uses22()
    {
        // Arrange
        const string host = "example.com";
        const string username = "testuser";

        // Act
        var config = new ConnectionConfiguration(host, username);

        // Assert
        Assert.Equal(22, config.Port);
    }

    [Fact]
    public void Constructor_NullHost_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ConnectionConfiguration(null!, "testuser"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidHost_ThrowsArgumentException(string host)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ConnectionConfiguration(host, "testuser"));
    }

    [Fact]
    public void Constructor_NullUsername_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ConnectionConfiguration("example.com", null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidUsername_ThrowsArgumentException(string username)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ConnectionConfiguration("example.com", username));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(65536)]
    public void Constructor_InvalidPort_ThrowsArgumentOutOfRangeException(int port)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new ConnectionConfiguration("example.com", "testuser", port));
    }

    [Fact]
    public void ConnectionTimeout_SetValue_UpdatesProperty()
    {
        // Arrange
        var config = new ConnectionConfiguration("example.com", "testuser");
        const int newTimeout = 5000;

        // Act
        config.ConnectionTimeout = newTimeout;

        // Assert
        Assert.Equal(newTimeout, config.ConnectionTimeout);
    }

    [Fact]
    public void OperationTimeout_SetValue_UpdatesProperty()
    {
        // Arrange
        var config = new ConnectionConfiguration("example.com", "testuser");
        const int newTimeout = 10000;

        // Act
        config.OperationTimeout = newTimeout;

        // Assert
        Assert.Equal(newTimeout, config.OperationTimeout);
    }

    [Fact]
    public void KeepAlive_SetValue_UpdatesProperty()
    {
        // Arrange
        var config = new ConnectionConfiguration("example.com", "testuser");

        // Act
        config.KeepAlive = false;

        // Assert
        Assert.False(config.KeepAlive);
    }
}