namespace SftpClient;

/// <summary>
/// Main entry point for creating an SFTP client
/// </summary>
public class SftpClientFactory
{
    /// <summary>
    /// Creates a new SftpClientBuilder for configuring and building an SFTP client
    /// </summary>
    /// <returns>A new SftpClientBuilder instance</returns>
    public static SftpClientBuilder CreateBuilder()
    {
        return new SftpClientBuilder();
    }
}