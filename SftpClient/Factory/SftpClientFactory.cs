using SftpClient.Interfaces;

namespace SftpClient.Factory;

/// <summary>
/// Factory for creating SFTP client instances
/// </summary>
public class SftpClientFactory : ISftpClientFactory
{
    /// <inheritdoc />
    public ISftpClient CreateClient(IConnectionConfiguration configuration, IAuthenticationMethod authenticationMethod)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));
        
        if (authenticationMethod == null)
            throw new ArgumentNullException(nameof(authenticationMethod));
        
        return new SftpNetClient(configuration, authenticationMethod);
    }
}