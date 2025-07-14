using SftpClient.Authentication;
using SftpClient.Configuration;
using SftpClient.Factory;
using SftpClient.Interfaces;

namespace SftpClient;

/// <summary>
/// Builder for creating SFTP clients with fluent API
/// </summary>
public class SftpClientBuilder
{
    private string? _host;
    private int _port = 22;
    private string? _username;
    private string? _password;
    private string? _privateKeyPath;
    private string? _privateKeyPassphrase;
    private int _connectionTimeout = 30000;
    private int _operationTimeout = 60000;
    private bool _keepAlive = true;
    
    private readonly ISftpClientFactory _factory;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SftpClientBuilder"/> class
    /// </summary>
    public SftpClientBuilder() : this(new SftpClientFactory())
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SftpClientBuilder"/> class with a custom factory
    /// </summary>
    /// <param name="factory">The SFTP client factory to use</param>
    public SftpClientBuilder(ISftpClientFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    /// <summary>
    /// Sets the host for the SFTP connection
    /// </summary>
    /// <param name="host">The host name or IP address</param>
    /// <returns>The builder for method chaining</returns>
    public SftpClientBuilder WithHost(string host)
    {
        _host = host ?? throw new ArgumentNullException(nameof(host));
        return this;
    }

    /// <summary>
    /// Sets the port for the SFTP connection
    /// </summary>
    /// <param name="port">The port number</param>
    /// <returns>The builder for method chaining</returns>
    public SftpClientBuilder WithPort(int port)
    {
        if (port <= 0 || port > 65535)
            throw new ArgumentOutOfRangeException(nameof(port), "Port must be between 1 and 65535");
        
        _port = port;
        return this;
    }

    /// <summary>
    /// Sets the username for the SFTP connection
    /// </summary>
    /// <param name="username">The username</param>
    /// <returns>The builder for method chaining</returns>
    public SftpClientBuilder WithUsername(string username)
    {
        _username = username ?? throw new ArgumentNullException(nameof(username));
        return this;
    }

    /// <summary>
    /// Sets password authentication for the SFTP connection
    /// </summary>
    /// <param name="password">The password</param>
    /// <returns>The builder for method chaining</returns>
    public SftpClientBuilder WithPassword(string password)
    {
        _password = password ?? throw new ArgumentNullException(nameof(password));
        _privateKeyPath = null; // Clear private key settings when using password
        _privateKeyPassphrase = null;
        return this;
    }

    /// <summary>
    /// Sets private key authentication for the SFTP connection
    /// </summary>
    /// <param name="privateKeyPath">Path to the private key file</param>
    /// <param name="passphrase">Optional passphrase for the private key</param>
    /// <returns>The builder for method chaining</returns>
    public SftpClientBuilder WithPrivateKey(string privateKeyPath, string? passphrase = null)
    {
        _privateKeyPath = privateKeyPath ?? throw new ArgumentNullException(nameof(privateKeyPath));
        _privateKeyPassphrase = passphrase;
        _password = null; // Clear password when using private key
        return this;
    }

    /// <summary>
    /// Sets the connection timeout
    /// </summary>
    /// <param name="timeoutMs">Timeout in milliseconds</param>
    /// <returns>The builder for method chaining</returns>
    public SftpClientBuilder WithConnectionTimeout(int timeoutMs)
    {
        if (timeoutMs <= 0)
            throw new ArgumentOutOfRangeException(nameof(timeoutMs), "Timeout must be greater than 0");
        
        _connectionTimeout = timeoutMs;
        return this;
    }

    /// <summary>
    /// Sets the operation timeout
    /// </summary>
    /// <param name="timeoutMs">Timeout in milliseconds</param>
    /// <returns>The builder for method chaining</returns>
    public SftpClientBuilder WithOperationTimeout(int timeoutMs)
    {
        if (timeoutMs <= 0)
            throw new ArgumentOutOfRangeException(nameof(timeoutMs), "Timeout must be greater than 0");
        
        _operationTimeout = timeoutMs;
        return this;
    }

    /// <summary>
    /// Sets whether to keep the connection alive
    /// </summary>
    /// <param name="keepAlive">Whether to keep the connection alive</param>
    /// <returns>The builder for method chaining</returns>
    public SftpClientBuilder WithKeepAlive(bool keepAlive)
    {
        _keepAlive = keepAlive;
        return this;
    }

    /// <summary>
    /// Builds the SFTP client
    /// </summary>
    /// <returns>An SFTP client</returns>
    public ISftpClient Build()
    {
        ValidateConfiguration();

        var config = new ConnectionConfiguration(_host!, _username!, _port)
        {
            ConnectionTimeout = _connectionTimeout,
            OperationTimeout = _operationTimeout,
            KeepAlive = _keepAlive
        };

        IAuthenticationMethod authMethod;
        if (!string.IsNullOrEmpty(_privateKeyPath))
        {
            authMethod = new PrivateKeyAuthenticationMethod(_privateKeyPath, _privateKeyPassphrase);
        }
        else
        {
            authMethod = new PasswordAuthenticationMethod(_password!);
        }

        return _factory.CreateClient(config, authMethod);
    }
    
    private void ValidateConfiguration()
    {
        if (string.IsNullOrEmpty(_host))
            throw new InvalidOperationException("Host must be specified");
        
        if (string.IsNullOrEmpty(_username))
            throw new InvalidOperationException("Username must be specified");
        
        if (string.IsNullOrEmpty(_password) && string.IsNullOrEmpty(_privateKeyPath))
            throw new InvalidOperationException("Either password or private key must be specified for authentication");
    }
}