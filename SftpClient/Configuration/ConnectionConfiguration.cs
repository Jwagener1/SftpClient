using SftpClient.Interfaces;

namespace SftpClient.Configuration;

/// <summary>
/// Implementation of the connection configuration for SFTP
/// </summary>
public class ConnectionConfiguration : IConnectionConfiguration
{
    /// <summary>
    /// Default constructor with standard settings
    /// </summary>
    /// <param name="host">Host name or IP address</param>
    /// <param name="username">Username for authentication</param>
    /// <param name="port">Port number (default: 22)</param>
    public ConnectionConfiguration(string host, string username, int port = 22)
    {
        if (string.IsNullOrWhiteSpace(host))
            throw new ArgumentException("Host cannot be null or whitespace", nameof(host));
        
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be null or whitespace", nameof(username));
        
        if (port <= 0 || port > 65535)
            throw new ArgumentOutOfRangeException(nameof(port), "Port must be between 1 and 65535");
        
        Host = host;
        Username = username;
        Port = port;
        ConnectionTimeout = 30000; // Default 30 seconds
        OperationTimeout = 60000;  // Default 60 seconds
        KeepAlive = true;          // Default keep alive enabled
    }

    /// <inheritdoc />
    public string Host { get; }
    
    /// <inheritdoc />
    public int Port { get; }
    
    /// <inheritdoc />
    public string Username { get; }
    
    /// <inheritdoc />
    public int ConnectionTimeout { get; set; }
    
    /// <inheritdoc />
    public int OperationTimeout { get; set; }
    
    /// <inheritdoc />
    public bool KeepAlive { get; set; }
}