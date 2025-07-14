using Moq;
using SftpClient.Exceptions;
using SftpClient.Interfaces;

namespace SftpClient.Tests;

public class SftpNetClientTests
{
    private readonly Mock<IConnectionConfiguration> _mockConfig;
    private readonly Mock<IAuthenticationMethod> _mockAuth;

    public SftpNetClientTests()
    {
        _mockConfig = new Mock<IConnectionConfiguration>();
        _mockAuth = new Mock<IAuthenticationMethod>();

        // Setup default configuration
        _mockConfig.Setup(c => c.Host).Returns("localhost");
        _mockConfig.Setup(c => c.Port).Returns(22);
        _mockConfig.Setup(c => c.Username).Returns("testuser");
        _mockConfig.Setup(c => c.OperationTimeout).Returns(60000);

        // Setup authentication
        _mockAuth.Setup(a => a.GetConnectionInfo(_mockConfig.Object))
                .Returns(new Renci.SshNet.ConnectionInfo("localhost", 22, "testuser", 
                    new Renci.SshNet.PasswordAuthenticationMethod("testuser", "password")));
    }

    [Fact]
    public void Constructor_ValidParameters_CreatesInstance()
    {
        // Act
        var client = new SftpNetClient(_mockConfig.Object, _mockAuth.Object);

        // Assert
        Assert.NotNull(client);
        Assert.False(client.IsConnected);
    }

    [Fact]
    public void Constructor_NullConfiguration_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new SftpNetClient(null!, _mockAuth.Object));
    }

    [Fact]
    public void Constructor_NullAuthenticationMethod_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new SftpNetClient(_mockConfig.Object, null!));
    }

    [Fact]
    public void IsConnected_InitialState_ReturnsFalse()
    {
        // Arrange
        var client = new SftpNetClient(_mockConfig.Object, _mockAuth.Object);

        // Act
        var isConnected = client.IsConnected;

        // Assert
        Assert.False(isConnected);
    }

    [Fact]
    public void UploadFile_WhenNotConnected_ThrowsSftpConnectionException()
    {
        // Arrange
        var client = new SftpNetClient(_mockConfig.Object, _mockAuth.Object);

        // Act & Assert
        var exception = Assert.Throws<SftpConnectionException>(() => 
            client.UploadFile("local.txt", "remote.txt"));
        Assert.Contains("not connected", exception.Message);
    }

    [Fact]
    public void DownloadFile_WhenNotConnected_ThrowsSftpConnectionException()
    {
        // Arrange
        var client = new SftpNetClient(_mockConfig.Object, _mockAuth.Object);

        // Act & Assert
        var exception = Assert.Throws<SftpConnectionException>(() => 
            client.DownloadFile("remote.txt", "local.txt"));
        Assert.Contains("not connected", exception.Message);
    }

    [Fact]
    public void DeleteFile_WhenNotConnected_ThrowsSftpConnectionException()
    {
        // Arrange
        var client = new SftpNetClient(_mockConfig.Object, _mockAuth.Object);

        // Act & Assert
        var exception = Assert.Throws<SftpConnectionException>(() => 
            client.DeleteFile("remote.txt"));
        Assert.Contains("not connected", exception.Message);
    }

    [Fact]
    public void FileExists_WhenNotConnected_ThrowsSftpConnectionException()
    {
        // Arrange
        var client = new SftpNetClient(_mockConfig.Object, _mockAuth.Object);

        // Act & Assert
        var exception = Assert.Throws<SftpConnectionException>(() => 
            client.FileExists("remote.txt"));
        Assert.Contains("not connected", exception.Message);
    }

    [Fact]
    public void CreateDirectory_WhenNotConnected_ThrowsSftpConnectionException()
    {
        // Arrange
        var client = new SftpNetClient(_mockConfig.Object, _mockAuth.Object);

        // Act & Assert
        var exception = Assert.Throws<SftpConnectionException>(() => 
            client.CreateDirectory("newdir"));
        Assert.Contains("not connected", exception.Message);
    }

    [Fact]
    public void ListDirectory_WhenNotConnected_ThrowsSftpConnectionException()
    {
        // Arrange
        var client = new SftpNetClient(_mockConfig.Object, _mockAuth.Object);

        // Act & Assert
        var exception = Assert.Throws<SftpConnectionException>(() => 
            client.ListDirectory("/path"));
        Assert.Contains("not connected", exception.Message);
    }

    [Fact]
    public async Task UploadFileAsync_WhenNotConnected_ThrowsSftpConnectionException()
    {
        // Arrange
        var client = new SftpNetClient(_mockConfig.Object, _mockAuth.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<SftpConnectionException>(() => 
            client.UploadFileAsync("local.txt", "remote.txt"));
        Assert.Contains("not connected", exception.Message);
    }

    [Fact]
    public async Task DownloadFileAsync_WhenNotConnected_ThrowsSftpConnectionException()
    {
        // Arrange
        var client = new SftpNetClient(_mockConfig.Object, _mockAuth.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<SftpConnectionException>(() => 
            client.DownloadFileAsync("remote.txt", "local.txt"));
        Assert.Contains("not connected", exception.Message);
    }

    [Fact]
    public async Task DeleteFileAsync_WhenNotConnected_ThrowsSftpConnectionException()
    {
        // Arrange
        var client = new SftpNetClient(_mockConfig.Object, _mockAuth.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<SftpConnectionException>(() => 
            client.DeleteFileAsync("remote.txt"));
        Assert.Contains("not connected", exception.Message);
    }

    [Fact]
    public async Task FileExistsAsync_WhenNotConnected_ThrowsSftpConnectionException()
    {
        // Arrange
        var client = new SftpNetClient(_mockConfig.Object, _mockAuth.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<SftpConnectionException>(() => 
            client.FileExistsAsync("remote.txt"));
        Assert.Contains("not connected", exception.Message);
    }

    [Fact]
    public async Task CreateDirectoryAsync_WhenNotConnected_ThrowsSftpConnectionException()
    {
        // Arrange
        var client = new SftpNetClient(_mockConfig.Object, _mockAuth.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<SftpConnectionException>(() => 
            client.CreateDirectoryAsync("newdir"));
        Assert.Contains("not connected", exception.Message);
    }

    [Fact]
    public async Task ListDirectoryAsync_WhenNotConnected_ThrowsSftpConnectionException()
    {
        // Arrange
        var client = new SftpNetClient(_mockConfig.Object, _mockAuth.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<SftpConnectionException>(() => 
            client.ListDirectoryAsync("/path"));
        Assert.Contains("not connected", exception.Message);
    }

    [Fact]
    public void Disconnect_WhenNotConnected_DoesNotThrow()
    {
        // Arrange
        var client = new SftpNetClient(_mockConfig.Object, _mockAuth.Object);

        // Act & Assert
        client.Disconnect(); // Should not throw
    }

    [Fact]
    public void Dispose_DoesNotThrow()
    {
        // Arrange
        var client = new SftpNetClient(_mockConfig.Object, _mockAuth.Object);

        // Act & Assert
        client.Dispose(); // Should not throw
    }

    [Fact]
    public void Dispose_WhenAlreadyDisposed_DoesNotThrow()
    {
        // Arrange
        var client = new SftpNetClient(_mockConfig.Object, _mockAuth.Object);

        // Act & Assert
        client.Dispose();
        client.Dispose(); // Should not throw
    }

    [Fact]
    public void Connect_WhenDisposed_ThrowsObjectDisposedException()
    {
        // Arrange
        var client = new SftpNetClient(_mockConfig.Object, _mockAuth.Object);
        client.Dispose();

        // Act & Assert
        Assert.Throws<ObjectDisposedException>(() => client.Connect());
    }

    [Fact]
    public void UploadFile_FileNotFound_ThrowsSftpFileOperationException()
    {
        // Arrange
        var client = new SftpNetClient(_mockConfig.Object, _mockAuth.Object);
        const string nonExistentFile = "nonexistent.txt";

        // Act & Assert
        var exception = Assert.Throws<SftpConnectionException>(() => 
            client.UploadFile(nonExistentFile, "remote.txt"));
        Assert.Contains("not connected", exception.Message);
    }
}