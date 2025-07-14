using Renci.SshNet;

namespace SftpClient.Interfaces;

/// <summary>
/// Interface for defining authentication methods for SFTP connections
/// </summary>
public interface IAuthenticationMethod
{
    /// <summary>
    /// Gets the connection info for SSH.NET
    /// </summary>
    /// <param name="configuration">The connection configuration</param>
    /// <returns>Connection info for establishing an SSH connection</returns>
    ConnectionInfo GetConnectionInfo(IConnectionConfiguration configuration);
}