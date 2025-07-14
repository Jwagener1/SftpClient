using SftpClient.Models;

namespace SftpClient.Tests.Models;

public class SftpFileTests
{
    [Fact]
    public void Constructor_NullFullPath_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new SftpClient.Models.SftpFile(null!, null!));
    }

    [Fact]
    public void Constructor_EmptyFullPath_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new SftpClient.Models.SftpFile("", null!));
    }

    [Fact]
    public void Constructor_NullAttributes_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new SftpClient.Models.SftpFile("/path/to/file.txt", null!));
    }

    // Note: Additional tests would require properly constructed SftpFileAttributes objects
    // which have complex constructor requirements. These basic validation tests ensure
    // the core argument checking works correctly.
}