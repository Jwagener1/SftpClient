using Renci.SshNet;
using SftpClient.Interfaces;

namespace SftpClient.Authentication;

/// <summary>
/// Authentication method using a private key
/// </summary>
public class PrivateKeyAuthenticationMethod : IAuthenticationMethod
{
    private readonly string _privateKeyPath;
    private readonly string? _passphrase;

    /// <summary>
    /// Initializes a new instance of the <see cref="PrivateKeyAuthenticationMethod"/> class
    /// </summary>
    /// <param name="privateKeyPath">Path to the private key file</param>
    /// <param name="passphrase">Optional passphrase for the private key</param>
    public PrivateKeyAuthenticationMethod(string privateKeyPath, string? passphrase = null)
    {
        _privateKeyPath = privateKeyPath ?? throw new ArgumentNullException(nameof(privateKeyPath));
        _passphrase = passphrase;
    }

    /// <inheritdoc />
    public ConnectionInfo GetConnectionInfo(IConnectionConfiguration configuration)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));

        var privateKey = string.IsNullOrEmpty(_passphrase)
            ? new PrivateKeyFile(_privateKeyPath)
            : new PrivateKeyFile(_privateKeyPath, _passphrase);

        return new ConnectionInfo(
            configuration.Host,
            configuration.Port,
            configuration.Username,
            new Renci.SshNet.PrivateKeyAuthenticationMethod(configuration.Username, privateKey))
        {
            Timeout = TimeSpan.FromMilliseconds(configuration.ConnectionTimeout)
        };
    }
}