namespace SftpClient.Exceptions;

/// <summary>
/// Exception thrown when a file operation fails on an SFTP server
/// </summary>
public class SftpFileOperationException : SftpException
{
    /// <summary>
    /// Gets the path of the file that caused the exception
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SftpFileOperationException"/> class
    /// </summary>
    /// <param name="filePath">The file path that caused the exception</param>
    public SftpFileOperationException(string filePath) 
        : base($"Failed to perform operation on file: {filePath}")
    {
        FilePath = filePath;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SftpFileOperationException"/> class with a message
    /// </summary>
    /// <param name="message">The exception message</param>
    /// <param name="filePath">The file path that caused the exception</param>
    public SftpFileOperationException(string message, string filePath) 
        : base(message)
    {
        FilePath = filePath;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SftpFileOperationException"/> class with a message and inner exception
    /// </summary>
    /// <param name="message">The exception message</param>
    /// <param name="innerException">The inner exception</param>
    /// <param name="filePath">The file path that caused the exception</param>
    public SftpFileOperationException(string message, Exception innerException, string filePath) 
        : base(message, innerException)
    {
        FilePath = filePath;
    }
}