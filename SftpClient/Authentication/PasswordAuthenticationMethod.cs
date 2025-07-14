using Renci.SshNet;
using SftpClient.Interfaces;

namespace SftpClient.Authentication;

/// <summary>
/// Authentication method using username and password
/// </summary>
public class PasswordAuthenticationMethod : IAuthenticationMethod
{
    private readonly string _password;

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordAuthenticationMethod"/> class
    /// </summary>
    /// <param name="password">Password for authentication</param>
    public PasswordAuthenticationMethod(string password)
    {
        _password = password ?? throw new ArgumentNullException(nameof(password));
    }

    /// <inheritdoc />
    public ConnectionInfo GetConnectionInfo(IConnectionConfiguration configuration)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));

        return new ConnectionInfo(
            configuration.Host,
            configuration.Port,
            configuration.Username,
            new Renci.SshNet.PasswordAuthenticationMethod(configuration.Username, _password))
        {
            Timeout = TimeSpan.FromMilliseconds(configuration.ConnectionTimeout)
        };
    }
}