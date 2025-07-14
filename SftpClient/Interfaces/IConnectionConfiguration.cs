namespace SftpClient.Interfaces;

/// <summary>
/// Interface defining the configuration needed for SFTP connections
/// </summary>
public interface IConnectionConfiguration
{
    /// <summary>
    /// Gets the hostname or IP address of the SFTP server
    /// </summary>
    string Host { get; }
    
    /// <summary>
    /// Gets the port number of the SFTP server
    /// </summary>
    int Port { get; }
    
    /// <summary>
    /// Gets the username for authentication
    /// </summary>
    string Username { get; }
    
    /// <summary>
    /// Gets the connection timeout in milliseconds
    /// </summary>
    int ConnectionTimeout { get; }
    
    /// <summary>
    /// Gets the operation timeout in milliseconds
    /// </summary>
    int OperationTimeout { get; }
    
    /// <summary>
    /// Gets a value indicating whether to keep the connection alive
    /// </summary>
    bool KeepAlive { get; }
}