namespace SftpClient.IntegrationTests;

/// <summary>
/// Test fixture for SFTP integration tests that handles setup and cleanup
/// </summary>
public class SftpIntegrationTestFixture : IDisposable
{
    private readonly string _testSessionId;
    private readonly SftpClientBuilder _clientBuilder;
    private bool _disposed;

    public SftpIntegrationTestFixture()
    {
        _testSessionId = $"test-session-{DateTime.Now:yyyyMMdd-HHmmss}-{Guid.NewGuid():N}";
        _clientBuilder = TestConfiguration.CreateTestClientBuilder();
        
        // Create base test directory
        TestBaseDirectory = Path.Combine(TestConfiguration.BaseTestPath, _testSessionId).Replace('\\', '/');
        
        SetupTestEnvironment();
    }

    /// <summary>
    /// The base directory for this test session
    /// </summary>
    public string TestBaseDirectory { get; }

    /// <summary>
    /// Creates a new SFTP client for testing
    /// </summary>
    /// <returns>A new ISftpClient instance</returns>
    public Interfaces.ISftpClient CreateClient()
    {
        return _clientBuilder.Build();
    }

    /// <summary>
    /// Gets a unique test file name for the current test session
    /// </summary>
    /// <param name="fileName">Base file name</param>
    /// <returns>Unique test file name</returns>
    public string GetTestFileName(string fileName)
    {
        return $"{Path.GetFileNameWithoutExtension(fileName)}-{_testSessionId}{Path.GetExtension(fileName)}";
    }

    /// <summary>
    /// Gets a test directory path relative to the base test directory
    /// </summary>
    /// <param name="directoryName">Directory name</param>
    /// <returns>Full test directory path</returns>
    public string GetTestDirectoryPath(string directoryName)
    {
        return $"{TestBaseDirectory}/{directoryName}".Replace('\\', '/');
    }

    /// <summary>
    /// Creates a temporary local file with test content
    /// </summary>
    /// <param name="content">File content</param>
    /// <returns>Path to the temporary file</returns>
    public string CreateTempFile(string content)
    {
        var tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, content);
        return tempFile;
    }

    /// <summary>
    /// Creates a temporary local file with random binary content
    /// </summary>
    /// <param name="sizeInBytes">Size of the file in bytes</param>
    /// <returns>Path to the temporary file</returns>
    public string CreateTempBinaryFile(int sizeInBytes)
    {
        var tempFile = Path.GetTempFileName();
        var random = new Random();
        var buffer = new byte[sizeInBytes];
        random.NextBytes(buffer);
        File.WriteAllBytes(tempFile, buffer);
        return tempFile;
    }

    private void SetupTestEnvironment()
    {
        try
        {
            using var client = CreateClient();
            client.Connect();
            
            // Create base test directory recursively
            CreateDirectoryRecursively(client, TestBaseDirectory);
            
            client.Disconnect();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to setup test environment. Please ensure SFTP server is accessible at {TestConfiguration.Host}:{TestConfiguration.Port} " +
                $"with credentials {TestConfiguration.Username}. Error: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Creates a directory recursively, creating parent directories if they don't exist
    /// </summary>
    /// <param name="client">SFTP client</param>
    /// <param name="directoryPath">Directory path to create</param>
    private void CreateDirectoryRecursively(Interfaces.ISftpClient client, string directoryPath)
    {
        // Normalize the path
        var normalizedPath = directoryPath.Replace('\\', '/');
        
        // Split the path into components
        var pathComponents = normalizedPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        
        // Build the path incrementally
        var currentPath = normalizedPath.StartsWith('/') ? "" : "";
        
        foreach (var component in pathComponents)
        {
            currentPath += "/" + component;
            
            try
            {
                // Check if directory exists by trying to list it
                client.ListDirectory(currentPath);
            }
            catch
            {
                // Directory doesn't exist, create it
                try
                {
                    client.CreateDirectory(currentPath);
                }
                catch
                {
                    // If creation fails, it might already exist or there might be permission issues
                    // Try to list it again to verify
                    try
                    {
                        client.ListDirectory(currentPath);
                    }
                    catch
                    {
                        throw new InvalidOperationException(
                            $"Failed to create or access directory: {currentPath}. " +
                            "Please ensure the user has write permissions to the base path.");
                    }
                }
            }
        }
    }

    private void CleanupTestEnvironment()
    {
        try
        {
            using var client = CreateClient();
            client.Connect();
            
            // Clean up test files and directories
            CleanupDirectory(client, TestBaseDirectory);
            
            client.Disconnect();
        }
        catch
        {
            // Ignore cleanup errors
        }
    }

    private void CleanupDirectory(Interfaces.ISftpClient client, string directoryPath)
    {
        try
        {
            var files = client.ListDirectory(directoryPath);
            
            foreach (var file in files.Where(f => f.Name != "." && f.Name != ".."))
            {
                try
                {
                    if (file.IsDirectory)
                    {
                        CleanupDirectory(client, file.FullPath);
                    }
                    else
                    {
                        client.DeleteFile(file.FullPath);
                    }
                }
                catch
                {
                    // Ignore individual file cleanup errors
                }
            }
        }
        catch
        {
            // Ignore directory listing errors
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            CleanupTestEnvironment();
            _disposed = true;
        }
    }
}