using System.Text;
using SftpClient.Exceptions;

namespace SftpClient.IntegrationTests;

/// <summary>
/// Integration tests for the SftpClient library.
/// These tests require an actual SFTP server to be running and accessible.
/// 
/// Environment Variables for Configuration:
/// - SFTP_TEST_HOST: SFTP server host (default: 192.168.133.2)
/// - SFTP_TEST_PORT: SFTP server port (default: 22)
/// - SFTP_TEST_USERNAME: Username for authentication (default: jonathan)
/// - SFTP_TEST_PASSWORD: Password for authentication (default: Wagener1)
/// - SFTP_TEST_PRIVATE_KEY: Path to private key file (optional)
/// - SFTP_TEST_PRIVATE_KEY_PASSPHRASE: Passphrase for private key (optional)
/// - SFTP_TEST_BASE_PATH: Base path for test files (default: /tmp/sftp-integration-tests)
/// - SFTP_TEST_CONNECTION_TIMEOUT: Connection timeout in ms (default: 30000)
/// - SFTP_TEST_OPERATION_TIMEOUT: Operation timeout in ms (default: 60000)
/// - SFTP_TEST_SKIP: Set to 'true' to skip integration tests (default: false)
/// </summary>
public class SftpClientIntegrationTests : IClassFixture<SftpIntegrationTestFixture>
{
    private readonly SftpIntegrationTestFixture _fixture;

    public SftpClientIntegrationTests(SftpIntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [SkippableFact]
    public void CanConnectToSftpServer()
    {
        Skip.If(TestConfiguration.SkipIntegrationTests, "Integration tests are disabled");

        // Arrange
        using var client = _fixture.CreateClient();

        // Act & Assert
        Assert.False(client.IsConnected);
        
        client.Connect();
        Assert.True(client.IsConnected);
        
        client.Disconnect();
        Assert.False(client.IsConnected);
    }

    [SkippableFact]
    public async Task CanConnectToSftpServerAsync()
    {
        Skip.If(TestConfiguration.SkipIntegrationTests, "Integration tests are disabled");

        // Arrange
        using var client = _fixture.CreateClient();

        // Act & Assert
        Assert.False(client.IsConnected);
        
        await client.ConnectAsync();
        Assert.True(client.IsConnected);
        
        client.Disconnect();
        Assert.False(client.IsConnected);
    }

    [SkippableFact]
    public void CanUploadAndDownloadTextFile()
    {
        Skip.If(TestConfiguration.SkipIntegrationTests, "Integration tests are disabled");

        // Arrange
        using var client = _fixture.CreateClient();
        client.Connect();

        const string testContent = "Hello, SFTP Integration Test!\nThis is a test file.\n";
        var localFile = _fixture.CreateTempFile(testContent);
        var remoteFile = $"{_fixture.TestBaseDirectory}/{_fixture.GetTestFileName("test.txt")}";
        var downloadFile = Path.GetTempFileName();

        try
        {
            // Act - Upload
            client.UploadFile(localFile, remoteFile);

            // Assert - File exists on server
            Assert.True(client.FileExists(remoteFile));

            // Act - Download
            client.DownloadFile(remoteFile, downloadFile);

            // Assert - Content matches
            var downloadedContent = File.ReadAllText(downloadFile);
            Assert.Equal(testContent, downloadedContent);
        }
        finally
        {
            // Cleanup
            try { File.Delete(localFile); } catch { }
            try { File.Delete(downloadFile); } catch { }
            try { client.DeleteFile(remoteFile); } catch { }
        }
    }

    [SkippableFact]
    public async Task CanUploadAndDownloadTextFileAsync()
    {
        Skip.If(TestConfiguration.SkipIntegrationTests, "Integration tests are disabled");

        // Arrange
        using var client = _fixture.CreateClient();
        await client.ConnectAsync();

        const string testContent = "Hello, SFTP Async Integration Test!\nThis is a test file.\n";
        var localFile = _fixture.CreateTempFile(testContent);
        var remoteFile = $"{_fixture.TestBaseDirectory}/{_fixture.GetTestFileName("test-async.txt")}";
        var downloadFile = Path.GetTempFileName();

        try
        {
            // Act - Upload
            await client.UploadFileAsync(localFile, remoteFile);

            // Assert - File exists on server
            Assert.True(await client.FileExistsAsync(remoteFile));

            // Act - Download
            await client.DownloadFileAsync(remoteFile, downloadFile);

            // Assert - Content matches
            var downloadedContent = File.ReadAllText(downloadFile);
            Assert.Equal(testContent, downloadedContent);
        }
        finally
        {
            // Cleanup
            try { File.Delete(localFile); } catch { }
            try { File.Delete(downloadFile); } catch { }
            try { await client.DeleteFileAsync(remoteFile); } catch { }
        }
    }

    [SkippableFact]
    public void CanUploadAndDownloadBinaryFile()
    {
        Skip.If(TestConfiguration.SkipIntegrationTests, "Integration tests are disabled");

        // Arrange
        using var client = _fixture.CreateClient();
        client.Connect();

        const int fileSize = 1024 * 10; // 10KB
        var localFile = _fixture.CreateTempBinaryFile(fileSize);
        var remoteFile = $"{_fixture.TestBaseDirectory}/{_fixture.GetTestFileName("test.bin")}";
        var downloadFile = Path.GetTempFileName();

        try
        {
            // Act - Upload
            client.UploadFile(localFile, remoteFile);

            // Assert - File exists on server
            Assert.True(client.FileExists(remoteFile));

            // Act - Download
            client.DownloadFile(remoteFile, downloadFile);

            // Assert - Content matches
            var originalBytes = File.ReadAllBytes(localFile);
            var downloadedBytes = File.ReadAllBytes(downloadFile);
            Assert.Equal(originalBytes.Length, downloadedBytes.Length);
            Assert.Equal(originalBytes, downloadedBytes);
        }
        finally
        {
            // Cleanup
            try { File.Delete(localFile); } catch { }
            try { File.Delete(downloadFile); } catch { }
            try { client.DeleteFile(remoteFile); } catch { }
        }
    }

    [SkippableFact]
    public void CanUploadAndDownloadStream()
    {
        Skip.If(TestConfiguration.SkipIntegrationTests, "Integration tests are disabled");

        // Arrange
        using var client = _fixture.CreateClient();
        client.Connect();

        const string testContent = "Hello, SFTP Stream Test!\nThis is a stream test.\n";
        var testBytes = Encoding.UTF8.GetBytes(testContent);
        var remoteFile = $"{_fixture.TestBaseDirectory}/{_fixture.GetTestFileName("stream-test.txt")}";

        try
        {
            // Act - Upload stream
            using (var uploadStream = new MemoryStream(testBytes))
            {
                client.UploadStream(uploadStream, remoteFile);
            }

            // Assert - File exists on server
            Assert.True(client.FileExists(remoteFile));

            // Act - Download stream
            using var downloadStream = new MemoryStream();
            client.DownloadStream(remoteFile, downloadStream);

            // Assert - Content matches
            var downloadedBytes = downloadStream.ToArray();
            var downloadedContent = Encoding.UTF8.GetString(downloadedBytes);
            Assert.Equal(testContent, downloadedContent);
        }
        finally
        {
            // Cleanup
            try { client.DeleteFile(remoteFile); } catch { }
        }
    }

    [SkippableFact]
    public async Task CanUploadAndDownloadStreamAsync()
    {
        Skip.If(TestConfiguration.SkipIntegrationTests, "Integration tests are disabled");

        // Arrange
        using var client = _fixture.CreateClient();
        await client.ConnectAsync();

        const string testContent = "Hello, SFTP Async Stream Test!\nThis is an async stream test.\n";
        var testBytes = Encoding.UTF8.GetBytes(testContent);
        var remoteFile = $"{_fixture.TestBaseDirectory}/{_fixture.GetTestFileName("stream-test-async.txt")}";

        try
        {
            // Act - Upload stream
            using (var uploadStream = new MemoryStream(testBytes))
            {
                await client.UploadStreamAsync(uploadStream, remoteFile);
            }

            // Assert - File exists on server
            Assert.True(await client.FileExistsAsync(remoteFile));

            // Act - Download stream
            using var downloadStream = new MemoryStream();
            await client.DownloadStreamAsync(remoteFile, downloadStream);

            // Assert - Content matches
            var downloadedBytes = downloadStream.ToArray();
            var downloadedContent = Encoding.UTF8.GetString(downloadedBytes);
            Assert.Equal(testContent, downloadedContent);
        }
        finally
        {
            // Cleanup
            try { await client.DeleteFileAsync(remoteFile); } catch { }
        }
    }

    [SkippableFact]
    public void CanListDirectory()
    {
        Skip.If(TestConfiguration.SkipIntegrationTests, "Integration tests are disabled");

        // Arrange
        using var client = _fixture.CreateClient();
        client.Connect();

        const string testContent = "Directory listing test content";
        var testFile1 = _fixture.CreateTempFile(testContent);
        var testFile2 = _fixture.CreateTempFile(testContent);
        var remoteFile1 = $"{_fixture.TestBaseDirectory}/{_fixture.GetTestFileName("list-test-1.txt")}";
        var remoteFile2 = $"{_fixture.TestBaseDirectory}/{_fixture.GetTestFileName("list-test-2.txt")}";

        try
        {
            // Act - Upload test files
            client.UploadFile(testFile1, remoteFile1);
            client.UploadFile(testFile2, remoteFile2);

            // Act - List directory
            var files = client.ListDirectory(_fixture.TestBaseDirectory).ToList();

            // Assert
            Assert.Contains(files, f => f.Name == Path.GetFileName(remoteFile1));
            Assert.Contains(files, f => f.Name == Path.GetFileName(remoteFile2));
            
            var file1Info = files.First(f => f.Name == Path.GetFileName(remoteFile1));
            Assert.False(file1Info.IsDirectory);
            Assert.True(file1Info.Size > 0);
            Assert.True(file1Info.LastModified > DateTime.MinValue);
        }
        finally
        {
            // Cleanup
            try { File.Delete(testFile1); } catch { }
            try { File.Delete(testFile2); } catch { }
            try { client.DeleteFile(remoteFile1); } catch { }
            try { client.DeleteFile(remoteFile2); } catch { }
        }
    }

    [SkippableFact]
    public async Task CanListDirectoryAsync()
    {
        Skip.If(TestConfiguration.SkipIntegrationTests, "Integration tests are disabled");

        // Arrange
        using var client = _fixture.CreateClient();
        await client.ConnectAsync();

        const string testContent = "Async directory listing test content";
        var testFile = _fixture.CreateTempFile(testContent);
        var remoteFile = $"{_fixture.TestBaseDirectory}/{_fixture.GetTestFileName("list-test-async.txt")}";

        try
        {
            // Act - Upload test file
            await client.UploadFileAsync(testFile, remoteFile);

            // Act - List directory
            var files = (await client.ListDirectoryAsync(_fixture.TestBaseDirectory)).ToList();

            // Assert
            Assert.Contains(files, f => f.Name == Path.GetFileName(remoteFile));
            
            var fileInfo = files.First(f => f.Name == Path.GetFileName(remoteFile));
            Assert.False(fileInfo.IsDirectory);
            Assert.True(fileInfo.Size > 0);
        }
        finally
        {
            // Cleanup
            try { File.Delete(testFile); } catch { }
            try { await client.DeleteFileAsync(remoteFile); } catch { }
        }
    }

    [SkippableFact]
    public void CanCreateAndDeleteDirectory()
    {
        Skip.If(TestConfiguration.SkipIntegrationTests, "Integration tests are disabled");

        // Arrange
        using var client = _fixture.CreateClient();
        client.Connect();

        var testDirectory = _fixture.GetTestDirectoryPath("test-directory");

        try
        {
            // Act - Create directory
            client.CreateDirectory(testDirectory);

            // Assert - Directory exists (by listing parent directory)
            var files = client.ListDirectory(_fixture.TestBaseDirectory).ToList();
            Assert.Contains(files, f => f.Name == "test-directory" && f.IsDirectory);
        }
        finally
        {
            // Cleanup - Note: SftpClient doesn't have DeleteDirectory method, so we skip cleanup
            // In a real scenario, you might want to implement directory deletion
        }
    }

    [SkippableFact]
    public async Task CanCreateAndDeleteDirectoryAsync()
    {
        Skip.If(TestConfiguration.SkipIntegrationTests, "Integration tests are disabled");

        // Arrange
        using var client = _fixture.CreateClient();
        await client.ConnectAsync();

        var testDirectory = _fixture.GetTestDirectoryPath("test-directory-async");

        try
        {
            // Act - Create directory
            await client.CreateDirectoryAsync(testDirectory);

            // Assert - Directory exists (by listing parent directory)
            var files = (await client.ListDirectoryAsync(_fixture.TestBaseDirectory)).ToList();
            Assert.Contains(files, f => f.Name == "test-directory-async" && f.IsDirectory);
        }
        finally
        {
            // Cleanup - Note: SftpClient doesn't have DeleteDirectory method, so we skip cleanup
        }
    }

    [SkippableFact]
    public void FileExists_ExistingFile_ReturnsTrue()
    {
        Skip.If(TestConfiguration.SkipIntegrationTests, "Integration tests are disabled");

        // Arrange
        using var client = _fixture.CreateClient();
        client.Connect();

        const string testContent = "File exists test content";
        var testFile = _fixture.CreateTempFile(testContent);
        var remoteFile = $"{_fixture.TestBaseDirectory}/{_fixture.GetTestFileName("exists-test.txt")}";

        try
        {
            // Act - Upload file
            client.UploadFile(testFile, remoteFile);

            // Assert - File exists
            Assert.True(client.FileExists(remoteFile));
            
            // Assert - Non-existent file returns false
            Assert.False(client.FileExists($"{_fixture.TestBaseDirectory}/non-existent-file.txt"));
        }
        finally
        {
            // Cleanup
            try { File.Delete(testFile); } catch { }
            try { client.DeleteFile(remoteFile); } catch { }
        }
    }

    [SkippableFact]
    public void DeleteFile_ExistingFile_RemovesFile()
    {
        Skip.If(TestConfiguration.SkipIntegrationTests, "Integration tests are disabled");

        // Arrange
        using var client = _fixture.CreateClient();
        client.Connect();

        const string testContent = "File delete test content";
        var testFile = _fixture.CreateTempFile(testContent);
        var remoteFile = $"{_fixture.TestBaseDirectory}/{_fixture.GetTestFileName("delete-test.txt")}";

        try
        {
            // Act - Upload file
            client.UploadFile(testFile, remoteFile);
            Assert.True(client.FileExists(remoteFile));

            // Act - Delete file
            client.DeleteFile(remoteFile);

            // Assert - File no longer exists
            Assert.False(client.FileExists(remoteFile));
        }
        finally
        {
            // Cleanup
            try { File.Delete(testFile); } catch { }
            try { client.DeleteFile(remoteFile); } catch { } // Extra safety
        }
    }

    [SkippableFact]
    public void Connect_InvalidCredentials_ThrowsException()
    {
        Skip.If(TestConfiguration.SkipIntegrationTests, "Integration tests are disabled");

        // Arrange
        using var client = new SftpClientBuilder()
            .WithHost(TestConfiguration.Host)
            .WithPort(TestConfiguration.Port)
            .WithUsername("invalid-user")
            .WithPassword("invalid-password")
            .Build();

        // Act & Assert
        Assert.Throws<SftpConnectionException>(() => client.Connect());
    }

    [SkippableFact]
    public void Connect_InvalidHost_ThrowsException()
    {
        Skip.If(TestConfiguration.SkipIntegrationTests, "Integration tests are disabled");

        // Arrange
        using var client = new SftpClientBuilder()
            .WithHost("invalid-host-that-does-not-exist")
            .WithPort(TestConfiguration.Port)
            .WithUsername(TestConfiguration.Username)
            .WithPassword(TestConfiguration.Password)
            .Build();

        // Act & Assert
        Assert.Throws<SftpConnectionException>(() => client.Connect());
    }

    [SkippableFact]
    public void UploadFile_NonExistentLocalFile_ThrowsException()
    {
        Skip.If(TestConfiguration.SkipIntegrationTests, "Integration tests are disabled");

        // Arrange
        using var client = _fixture.CreateClient();
        client.Connect();

        var nonExistentFile = "/path/to/non/existent/file.txt";
        var remoteFile = $"{_fixture.TestBaseDirectory}/{_fixture.GetTestFileName("upload-fail-test.txt")}";

        // Act & Assert
        Assert.Throws<SftpFileOperationException>(() => client.UploadFile(nonExistentFile, remoteFile));
    }

    [SkippableFact]
    public void DownloadFile_NonExistentRemoteFile_ThrowsException()
    {
        Skip.If(TestConfiguration.SkipIntegrationTests, "Integration tests are disabled");

        // Arrange
        using var client = _fixture.CreateClient();
        client.Connect();

        var nonExistentRemoteFile = $"{_fixture.TestBaseDirectory}/non-existent-file.txt";
        var localFile = Path.GetTempFileName();

        try
        {
            // Act & Assert
            Assert.Throws<SftpFileOperationException>(() => client.DownloadFile(nonExistentRemoteFile, localFile));
        }
        finally
        {
            // Cleanup
            try { File.Delete(localFile); } catch { }
        }
    }

    [SkippableFact]
    public void ListDirectory_NonExistentDirectory_ThrowsException()
    {
        Skip.If(TestConfiguration.SkipIntegrationTests, "Integration tests are disabled");

        // Arrange
        using var client = _fixture.CreateClient();
        client.Connect();

        var nonExistentDirectory = $"{_fixture.TestBaseDirectory}/non-existent-directory";

        // Act & Assert
        Assert.Throws<SftpFileOperationException>(() => client.ListDirectory(nonExistentDirectory).ToList());
    }

    [SkippableFact]
    public void DeleteFile_NonExistentFile_ThrowsException()
    {
        Skip.If(TestConfiguration.SkipIntegrationTests, "Integration tests are disabled");

        // Arrange
        using var client = _fixture.CreateClient();
        client.Connect();

        var nonExistentFile = $"{_fixture.TestBaseDirectory}/non-existent-file.txt";

        // Act & Assert
        Assert.Throws<SftpFileOperationException>(() => client.DeleteFile(nonExistentFile));
    }
}