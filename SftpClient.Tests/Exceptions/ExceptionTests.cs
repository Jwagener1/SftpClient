using SftpClient.Exceptions;

namespace SftpClient.Tests.Exceptions;

public class SftpExceptionTests
{
    [Fact]
    public void Constructor_Default_CreatesExceptionWithDefaultMessage()
    {
        // Act
        var exception = new SftpException();

        // Assert
        Assert.Equal("An error occurred during the SFTP operation.", exception.Message);
    }

    [Fact]
    public void Constructor_WithMessage_CreatesExceptionWithMessage()
    {
        // Arrange
        const string message = "Custom error message";

        // Act
        var exception = new SftpException(message);

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_CreatesExceptionWithBoth()
    {
        // Arrange
        const string message = "Custom error message";
        var innerException = new InvalidOperationException("Inner exception");

        // Act
        var exception = new SftpException(message, innerException);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Same(innerException, exception.InnerException);
    }
}

public class SftpConnectionExceptionTests
{
    [Fact]
    public void Constructor_Default_CreatesExceptionWithDefaultMessage()
    {
        // Act
        var exception = new SftpConnectionException();

        // Assert
        Assert.Equal("Failed to connect to the SFTP server.", exception.Message);
    }

    [Fact]
    public void Constructor_WithMessage_CreatesExceptionWithMessage()
    {
        // Arrange
        const string message = "Custom connection error";

        // Act
        var exception = new SftpConnectionException(message);

        // Assert
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_CreatesExceptionWithBoth()
    {
        // Arrange
        const string message = "Custom connection error";
        var innerException = new System.Net.Sockets.SocketException();

        // Act
        var exception = new SftpConnectionException(message, innerException);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Same(innerException, exception.InnerException);
    }

    [Fact]
    public void InheritsFromSftpException()
    {
        // Act
        var exception = new SftpConnectionException();

        // Assert
        Assert.IsAssignableFrom<SftpException>(exception);
    }
}

public class SftpFileOperationExceptionTests
{
    [Fact]
    public void Constructor_WithFilePath_CreatesExceptionWithFilePathAndDefaultMessage()
    {
        // Arrange
        const string filePath = "/path/to/file.txt";

        // Act
        var exception = new SftpFileOperationException(filePath);

        // Assert
        Assert.Equal($"Failed to perform operation on file: {filePath}", exception.Message);
        Assert.Equal(filePath, exception.FilePath);
    }

    [Fact]
    public void Constructor_WithMessageAndFilePath_CreatesExceptionWithBoth()
    {
        // Arrange
        const string message = "Custom file operation error";
        const string filePath = "/path/to/file.txt";

        // Act
        var exception = new SftpFileOperationException(message, filePath);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Equal(filePath, exception.FilePath);
    }

    [Fact]
    public void Constructor_WithMessageInnerExceptionAndFilePath_CreatesExceptionWithAll()
    {
        // Arrange
        const string message = "Custom file operation error";
        const string filePath = "/path/to/file.txt";
        var innerException = new UnauthorizedAccessException("Access denied");

        // Act
        var exception = new SftpFileOperationException(message, innerException, filePath);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Same(innerException, exception.InnerException);
        Assert.Equal(filePath, exception.FilePath);
    }

    [Fact]
    public void InheritsFromSftpException()
    {
        // Act
        var exception = new SftpFileOperationException("/path/to/file.txt");

        // Assert
        Assert.IsAssignableFrom<SftpException>(exception);
    }
}