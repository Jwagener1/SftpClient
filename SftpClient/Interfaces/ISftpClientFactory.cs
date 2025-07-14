namespace SftpClient.Interfaces;

/// <summary>
/// Factory interface for creating SFTP clients
/// </summary>
public interface ISftpClientFactory
{
    /// <summary>
    /// Creates an SFTP client with the specified configuration and authentication method
    /// </summary>
    /// <param name="configuration">The connection configuration</param>
    /// <param name="authenticationMethod">The authentication method</param>
    /// <returns>An SFTP client</returns>
    ISftpClient CreateClient(IConnectionConfiguration configuration, IAuthenticationMethod authenticationMethod);
}