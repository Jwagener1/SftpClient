namespace SftpClient.Exceptions;

/// <summary>
/// Base exception for SFTP operations
/// </summary>
public class SftpException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SftpException"/> class
    /// </summary>
    public SftpException() : base("An error occurred during the SFTP operation.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SftpException"/> class with a message
    /// </summary>
    /// <param name="message">The exception message</param>
    public SftpException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SftpException"/> class with a message and inner exception
    /// </summary>
    /// <param name="message">The exception message</param>
    /// <param name="innerException">The inner exception</param>
    public SftpException(string message, Exception innerException) : base(message, innerException)
    {
    }
}