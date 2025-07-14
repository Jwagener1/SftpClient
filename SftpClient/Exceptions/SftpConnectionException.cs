namespace SftpClient.Exceptions;

/// <summary>
/// Exception thrown when there are issues connecting to an SFTP server
/// </summary>
public class SftpConnectionException : SftpException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SftpConnectionException"/> class
    /// </summary>
    public SftpConnectionException() : base("Failed to connect to the SFTP server.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SftpConnectionException"/> class with a message
    /// </summary>
    /// <param name="message">The exception message</param>
    public SftpConnectionException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SftpConnectionException"/> class with a message and inner exception
    /// </summary>
    /// <param name="message">The exception message</param>
    /// <param name="innerException">The inner exception</param>
    public SftpConnectionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}