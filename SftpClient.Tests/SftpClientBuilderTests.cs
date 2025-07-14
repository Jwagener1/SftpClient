using Moq;
using SftpClient.Authentication;
using SftpClient.Configuration;
using SftpClient.Interfaces;

namespace SftpClient.Tests;

public class SftpClientBuilderTests
{
    private readonly Mock<ISftpClientFactory> _mockFactory;
    private readonly Mock<ISftpClient> _mockSftpClient;

    public SftpClientBuilderTests()
    {
        _mockFactory = new Mock<ISftpClientFactory>();
        _mockSftpClient = new Mock<ISftpClient>();
        _mockFactory.Setup(f => f.CreateClient(It.IsAny<IConnectionConfiguration>(), It.IsAny<IAuthenticationMethod>()))
                   .Returns(_mockSftpClient.Object);
    }

    [Fact]
    public void Constructor_WithoutFactory_UsesDefaultFactory()
    {
        // Act
        var builder = new SftpClientBuilder();

        // Assert
        Assert.NotNull(builder);
    }

    [Fact]
    public void Constructor_WithFactory_UsesProvidedFactory()
    {
        // Act
        var builder = new SftpClientBuilder(_mockFactory.Object);

        // Assert
        Assert.NotNull(builder);
    }

    [Fact]
    public void Constructor_WithNullFactory_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new SftpClientBuilder(null!));
    }

    [Fact]
    public void WithHost_ValidHost_ReturnsBuilder()
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object);
        const string host = "example.com";

        // Act
        var result = builder.WithHost(host);

        // Assert
        Assert.Same(builder, result);
    }

    [Fact]
    public void WithHost_NullHost_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.WithHost(null!));
    }

    [Fact]
    public void WithPort_ValidPort_ReturnsBuilder()
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object);
        const int port = 2222;

        // Act
        var result = builder.WithPort(port);

        // Assert
        Assert.Same(builder, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(65536)]
    public void WithPort_InvalidPort_ThrowsArgumentOutOfRangeException(int port)
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithPort(port));
    }

    [Fact]
    public void WithUsername_ValidUsername_ReturnsBuilder()
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object);
        const string username = "testuser";

        // Act
        var result = builder.WithUsername(username);

        // Assert
        Assert.Same(builder, result);
    }

    [Fact]
    public void WithUsername_NullUsername_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.WithUsername(null!));
    }

    [Fact]
    public void WithPassword_ValidPassword_ReturnsBuilder()
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object);
        const string password = "testpassword";

        // Act
        var result = builder.WithPassword(password);

        // Assert
        Assert.Same(builder, result);
    }

    [Fact]
    public void WithPassword_NullPassword_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.WithPassword(null!));
    }

    [Fact]
    public void WithPrivateKey_ValidPath_ReturnsBuilder()
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object);
        const string privateKeyPath = "/path/to/key";

        // Act
        var result = builder.WithPrivateKey(privateKeyPath);

        // Assert
        Assert.Same(builder, result);
    }

    [Fact]
    public void WithPrivateKey_ValidPathWithPassphrase_ReturnsBuilder()
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object);
        const string privateKeyPath = "/path/to/key";
        const string passphrase = "passphrase";

        // Act
        var result = builder.WithPrivateKey(privateKeyPath, passphrase);

        // Assert
        Assert.Same(builder, result);
    }

    [Fact]
    public void WithPrivateKey_NullPath_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.WithPrivateKey(null!));
    }

    [Fact]
    public void WithConnectionTimeout_ValidTimeout_ReturnsBuilder()
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object);
        const int timeout = 5000;

        // Act
        var result = builder.WithConnectionTimeout(timeout);

        // Assert
        Assert.Same(builder, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void WithConnectionTimeout_InvalidTimeout_ThrowsArgumentOutOfRangeException(int timeout)
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithConnectionTimeout(timeout));
    }

    [Fact]
    public void WithOperationTimeout_ValidTimeout_ReturnsBuilder()
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object);
        const int timeout = 10000;

        // Act
        var result = builder.WithOperationTimeout(timeout);

        // Assert
        Assert.Same(builder, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void WithOperationTimeout_InvalidTimeout_ThrowsArgumentOutOfRangeException(int timeout)
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithOperationTimeout(timeout));
    }

    [Fact]
    public void WithKeepAlive_ValidValue_ReturnsBuilder()
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object);

        // Act
        var result = builder.WithKeepAlive(false);

        // Assert
        Assert.Same(builder, result);
    }

    [Fact]
    public void Build_ValidConfigurationWithPassword_ReturnsClient()
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object)
            .WithHost("example.com")
            .WithUsername("testuser")
            .WithPassword("testpassword");

        // Act
        var client = builder.Build();

        // Assert
        Assert.NotNull(client);
        _mockFactory.Verify(f => f.CreateClient(
            It.Is<IConnectionConfiguration>(c => 
                c.Host == "example.com" && 
                c.Username == "testuser" && 
                c.Port == 22),
            It.IsAny<PasswordAuthenticationMethod>()), Times.Once);
    }

    [Fact]
    public void Build_ValidConfigurationWithPrivateKey_ReturnsClient()
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object)
            .WithHost("example.com")
            .WithUsername("testuser")
            .WithPrivateKey("/path/to/key", "passphrase");

        // Act
        var client = builder.Build();

        // Assert
        Assert.NotNull(client);
        _mockFactory.Verify(f => f.CreateClient(
            It.Is<IConnectionConfiguration>(c => 
                c.Host == "example.com" && 
                c.Username == "testuser" && 
                c.Port == 22),
            It.IsAny<PrivateKeyAuthenticationMethod>()), Times.Once);
    }

    [Fact]
    public void Build_NoHost_ThrowsInvalidOperationException()
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object)
            .WithUsername("testuser")
            .WithPassword("testpassword");

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Equal("Host must be specified", exception.Message);
    }

    [Fact]
    public void Build_NoUsername_ThrowsInvalidOperationException()
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object)
            .WithHost("example.com")
            .WithPassword("testpassword");

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Equal("Username must be specified", exception.Message);
    }

    [Fact]
    public void Build_NoAuthentication_ThrowsInvalidOperationException()
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object)
            .WithHost("example.com")
            .WithUsername("testuser");

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Equal("Either password or private key must be specified for authentication", exception.Message);
    }

    [Fact]
    public void Build_CustomTimeoutsAndPort_ConfiguresCorrectly()
    {
        // Arrange
        var builder = new SftpClientBuilder(_mockFactory.Object)
            .WithHost("example.com")
            .WithUsername("testuser")
            .WithPassword("testpassword")
            .WithPort(2222)
            .WithConnectionTimeout(5000)
            .WithOperationTimeout(10000)
            .WithKeepAlive(false);

        // Act
        var client = builder.Build();

        // Assert
        Assert.NotNull(client);
        _mockFactory.Verify(f => f.CreateClient(
            It.Is<IConnectionConfiguration>(c => 
                c.Host == "example.com" && 
                c.Username == "testuser" && 
                c.Port == 2222 &&
                c.ConnectionTimeout == 5000 &&
                c.OperationTimeout == 10000 &&
                c.KeepAlive == false),
            It.IsAny<PasswordAuthenticationMethod>()), Times.Once);
    }
}