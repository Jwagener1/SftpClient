using Moq;
using SftpClient.Authentication;
using SftpClient.Interfaces;

namespace SftpClient.Tests.Authentication;

public class PasswordAuthenticationMethodTests
{
    [Fact]
    public void Constructor_ValidPassword_CreatesInstance()
    {
        // Arrange
        const string password = "testpassword";

        // Act
        var auth = new PasswordAuthenticationMethod(password);

        // Assert
        Assert.NotNull(auth);
    }

    [Fact]
    public void Constructor_NullPassword_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new PasswordAuthenticationMethod(null!));
    }

    [Fact]
    public void GetConnectionInfo_ValidConfiguration_ReturnsConnectionInfo()
    {
        // Arrange
        var auth = new PasswordAuthenticationMethod("testpassword");
        var mockConfig = new Mock<IConnectionConfiguration>();
        mockConfig.Setup(c => c.Host).Returns("example.com");
        mockConfig.Setup(c => c.Port).Returns(22);
        mockConfig.Setup(c => c.Username).Returns("testuser");
        mockConfig.Setup(c => c.ConnectionTimeout).Returns(30000);

        // Act
        var connectionInfo = auth.GetConnectionInfo(mockConfig.Object);

        // Assert
        Assert.NotNull(connectionInfo);
        Assert.Equal("example.com", connectionInfo.Host);
        Assert.Equal(22, connectionInfo.Port);
        Assert.Equal("testuser", connectionInfo.Username);
        Assert.Equal(TimeSpan.FromMilliseconds(30000), connectionInfo.Timeout);
    }

    [Fact]
    public void GetConnectionInfo_NullConfiguration_ThrowsArgumentNullException()
    {
        // Arrange
        var auth = new PasswordAuthenticationMethod("testpassword");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => auth.GetConnectionInfo(null!));
    }
}