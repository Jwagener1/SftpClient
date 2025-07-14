namespace SftpClient;

/// <summary>
/// This class provides examples of how to use the SftpClient library.
/// </summary>
public class SftpExamples
{
    /// <summary>
    /// Example showing how to create an SFTP client with password authentication
    /// </summary>
    public static void PasswordAuthenticationExample()
    {
        // Create a client with password authentication
        using var client = new SftpClientBuilder()
            .WithHost("sftp.example.com")
            .WithUsername("username")
            .WithPassword("password")
            .WithPort(22) // Optional, defaults to 22
            .WithConnectionTimeout(30000) // Optional, defaults to 30000 ms
            .WithOperationTimeout(60000)  // Optional, defaults to 60000 ms
            .WithKeepAlive(true)         // Optional, defaults to true
            .Build();
        
        // Connect to the server
        client.Connect();
        
        // Upload a file
        client.UploadFile("local-file.txt", "/remote-directory/file.txt");
        
        // Download a file
        client.DownloadFile("/remote-directory/file.txt", "downloaded-file.txt");
        
        // List files in a directory
        var files = client.ListDirectory("/remote-directory");
        foreach (var file in files)
        {
            Console.WriteLine($"Name: {file.Name}, Size: {file.Size}, Modified: {file.LastModified}");
        }
        
        // Check if a file exists
        bool exists = client.FileExists("/remote-directory/file.txt");
        
        // Delete a file
        client.DeleteFile("/remote-directory/file.txt");
        
        // Create a directory
        client.CreateDirectory("/remote-directory/new-directory");
        
        // Disconnect when finished
        client.Disconnect();
    }

    /// <summary>
    /// Example showing how to create an SFTP client with private key authentication
    /// </summary>
    public static void PrivateKeyAuthenticationExample()
    {
        // Create a client with private key authentication
        using var client = new SftpClientBuilder()
            .WithHost("sftp.example.com")
            .WithUsername("username")
            .WithPrivateKey("/path/to/private-key.pem", "optional-passphrase")
            .Build();
        
        // Connect to the server
        client.Connect();
        
        // Use the client...
        
        // Disconnect when finished
        client.Disconnect();
    }
    
    /// <summary>
    /// Example showing how to use the SFTP client asynchronously
    /// </summary>
    public static async Task AsyncExample()
    {
        // Create a client
        using var client = new SftpClientBuilder()
            .WithHost("sftp.example.com")
            .WithUsername("username")
            .WithPassword("password")
            .Build();
        
        // Connect to the server asynchronously
        await client.ConnectAsync();
        
        // Upload a file asynchronously
        await client.UploadFileAsync("local-file.txt", "/remote-directory/file.txt");
        
        // Download a file asynchronously
        await client.DownloadFileAsync("/remote-directory/file.txt", "downloaded-file.txt");
        
        // List files in a directory asynchronously
        var files = await client.ListDirectoryAsync("/remote-directory");
        foreach (var file in files)
        {
            Console.WriteLine($"Name: {file.Name}, Size: {file.Size}, Modified: {file.LastModified}");
        }
        
        // Check if a file exists asynchronously
        bool exists = await client.FileExistsAsync("/remote-directory/file.txt");
        
        // Delete a file asynchronously
        await client.DeleteFileAsync("/remote-directory/file.txt");
        
        // Create a directory asynchronously
        await client.CreateDirectoryAsync("/remote-directory/new-directory");
        
        // Disconnect when finished
        client.Disconnect();
    }
}