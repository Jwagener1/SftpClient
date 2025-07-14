namespace SftpClient.IntegrationTests;

/// <summary>
/// Configuration for integration tests
/// Update the values below to match your SFTP server configuration
/// </summary>
public static class TestConfiguration
{
    // =========================================================================
    // UPDATE THESE VALUES FOR YOUR SFTP SERVER
    // =========================================================================
    
    /// <summary>
    /// SFTP server hostname or IP address
    /// </summary>
    public static string Host => "192.168.133.2";

    /// <summary>
    /// SFTP server port (usually 22)
    /// </summary>
    public static int Port => 22;

    /// <summary>
    /// Username for SFTP authentication
    /// </summary>
    public static string Username => "jonathan";

    /// <summary>
    /// Password for SFTP authentication
    /// </summary>
    public static string Password => "Wagener1";

    /// <summary>
    /// Base directory for test files on the SFTP server
    /// This directory will be created if it doesn't exist
    /// </summary>
    public static string BaseTestPath => "/home/jonathan/sftp-integration-tests";

    // =========================================================================
    // OPTIONAL SETTINGS (uncomment and modify if needed)
    // =========================================================================

    /// <summary>
    /// Path to private key file (leave null to use password authentication)
    /// </summary>
    public static string? PrivateKeyPath => null;
    // public static string? PrivateKeyPath => "/path/to/your/private/key.pem";

    /// <summary>
    /// Passphrase for private key (if required)
    /// </summary>
    public static string? PrivateKeyPassphrase => null;
    // public static string? PrivateKeyPassphrase => "your-passphrase";

    /// <summary>
    /// Connection timeout in milliseconds
    /// </summary>
    public static int ConnectionTimeout => 30000;

    /// <summary>
    /// Operation timeout in milliseconds
    /// </summary>
    public static int OperationTimeout => 60000;

    /// <summary>
    /// Set to true to skip integration tests
    /// </summary>
    public static bool SkipIntegrationTests => false;

    // =========================================================================
    // INTERNAL CONFIGURATION (do not modify)
    // =========================================================================

    /// <summary>
    /// Gets whether to use private key authentication instead of password
    /// </summary>
    public static bool UsePrivateKey => !string.IsNullOrEmpty(PrivateKeyPath);

    /// <summary>
    /// Creates an SftpClientBuilder with the configured test settings
    /// </summary>
    /// <returns>Configured SftpClientBuilder</returns>
    public static SftpClientBuilder CreateTestClientBuilder()
    {
        var builder = new SftpClientBuilder()
            .WithHost(Host)
            .WithPort(Port)
            .WithUsername(Username)
            .WithConnectionTimeout(ConnectionTimeout)
            .WithOperationTimeout(OperationTimeout);

        if (UsePrivateKey)
        {
            builder.WithPrivateKey(PrivateKeyPath!, PrivateKeyPassphrase);
        }
        else
        {
            builder.WithPassword(Password);
        }

        return builder;
    }
}