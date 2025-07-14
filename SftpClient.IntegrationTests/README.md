# SftpClient Integration Tests

This project contains comprehensive integration tests for the SftpClient library. These tests require an actual SFTP server to be running and accessible.

## ?? Quick Start

### 1. Configure Your SFTP Server Settings

Open `TestConfiguration.cs` and update the following values for your SFTP server:
public static string Host => "192.168.133.2";        // Your SFTP server IP/hostname
public static string Username => "jonathan";          // Your SFTP username
public static string Password => "Wagener1";          // Your SFTP password
public static string BaseTestPath => "/home/jonathan/sftp-integration-tests";  // Test directory
### 2. Run the Tests
# Run all integration tests
dotnet test SftpClient.IntegrationTests

# Run with detailed output
dotnet test SftpClient.IntegrationTests --verbosity normal

# Run specific test
dotnet test SftpClient.IntegrationTests --filter "CanConnectToSftpServer"
## ?? Configuration Options

### Basic Configuration

Edit the following properties in `TestConfiguration.cs`:

| Property | Description | Example |
|----------|-------------|---------|
| `Host` | SFTP server hostname or IP | `"192.168.133.2"` |
| `Port` | SFTP server port | `22` |
| `Username` | Username for authentication | `"jonathan"` |
| `Password` | Password for authentication | `"Wagener1"` |
| `BaseTestPath` | Directory for test files | `"/home/jonathan/sftp-tests"` |

### Advanced Configuration

For additional options, uncomment and modify these properties:
// Use private key authentication instead of password
public static string? PrivateKeyPath => "/path/to/your/private/key.pem";
public static string? PrivateKeyPassphrase => "your-passphrase";

// Adjust timeouts if needed
public static int ConnectionTimeout => 60000;  // 60 seconds
public static int OperationTimeout => 120000;  // 2 minutes

// Skip tests entirely
public static bool SkipIntegrationTests => true;
## ?? Authentication Methods

### Password Authentication (Default)public static string Username => "your-username";
public static string Password => "your-password";
public static string? PrivateKeyPath => null;  // Keep this null
### Private Key Authenticationpublic static string Username => "your-username";
public static string? PrivateKeyPath => "/path/to/your/private/key.pem";
public static string? PrivateKeyPassphrase => "optional-passphrase";
// Password is ignored when using private key
## ????? Running Tests

### Run All Testsdotnet test SftpClient.IntegrationTests
### Run Specific Test Categories# Connection tests only
dotnet test SftpClient.IntegrationTests --filter "CanConnect"

# File upload/download tests
dotnet test SftpClient.IntegrationTests --filter "UploadAndDownload"

# Directory operations
dotnet test SftpClient.IntegrationTests --filter "Directory"

# Async tests only
dotnet test SftpClient.IntegrationTests --filter "Async"
### Debug with Verbose Outputdotnet test SftpClient.IntegrationTests --verbosity normal
## ?? Test Coverage

The integration tests cover the following scenarios:

### ? Connection Management
- [x] Connect to SFTP server (sync/async)
- [x] Disconnect from SFTP server
- [x] Connection state validation
- [x] Invalid credentials handling
- [x] Invalid host handling

### ? File Operations
- [x] Upload text files (sync/async)
- [x] Upload binary files
- [x] Download text files (sync/async)
- [x] Download binary files
- [x] File existence checking
- [x] File deletion
- [x] Non-existent file error handling

### ? Stream Operations
- [x] Upload from stream (sync/async)
- [x] Download to stream (sync/async)
- [x] Memory stream operations

### ? Directory Operations
- [x] List directory contents (sync/async)
- [x] Create directories (sync/async)
- [x] Directory existence in listings
- [x] Non-existent directory error handling

### ? Error Handling
- [x] Connection errors
- [x] Authentication errors
- [x] File operation errors
- [x] Directory operation errors
- [x] Timeout handling

## ?? Test Setup and Cleanup

The integration tests use a comprehensive setup and cleanup system:

### Automatic Setup
- Creates a unique test session directory for each test run
- Automatically creates the base test directory (and parent directories) if they don't exist
- Provides isolated test environment for each test session

### Automatic Cleanup
- Removes all test files after each test completes
- Cleans up test directories automatically
- Handles cleanup failures gracefully (won't fail tests due to cleanup issues)

### Test Isolation
- Each test run uses unique file names with session IDs
- Tests can run in parallel without conflicts
- Temporary local files are automatically cleaned up

## ?? Test Directory Structure
/home/jonathan/sftp-integration-tests/
??? test-session-20240714-123456-abcdef123456/
    ??? test-file-1.txt
    ??? test-file-2.bin
    ??? test-directory/
        ??? nested-file.txt
## ?? Troubleshooting

### Common Issues

1. **Connection Refused**
   - Verify the SFTP server is running
   - Check the `Host` and `Port` values in `TestConfiguration.cs`
   - Ensure firewall allows connections on the specified port

2. **Authentication Failed**
   - Verify `Username` and `Password` in `TestConfiguration.cs`
   - Try connecting manually with an SFTP client to verify credentials

3. **Permission Denied**
   - Check if the user has write permissions to the `BaseTestPath` directory
   - Try using a different directory like `/tmp/sftp-tests`
   - Ensure the directory path is accessible to your user

4. **Directory Creation Failed**
   - The tests automatically create directories, but ensure the parent path exists
   - Use a path within your user's home directory for best results

### Quick Diagnostic

Run this test first to verify your configuration:dotnet test SftpClient.IntegrationTests --filter "CanConnectToSftpServer"
If this passes, your configuration is correct and you can run the full test suite.

## ?? Using Docker for Testing

If you don't have access to an SFTP server, you can use Docker:
# Run a test SFTP server
docker run -d \
  --name sftp-test-server \
  -p 2222:22 \
  -e SFTP_USERS="testuser:testpass:::upload" \
  atmoz/sftp

# Update TestConfiguration.cs:
# Host => "localhost"
# Port => 2222
# Username => "testuser" 
# Password => "testpass"
# BaseTestPath => "/home/testuser/upload"

# Run tests
dotnet test SftpClient.IntegrationTests
## ?? Example Configuration

Here's a complete example configuration in `TestConfiguration.cs`:
public static class TestConfiguration
{
    // Basic SFTP server settings
    public static string Host => "192.168.1.100";
    public static int Port => 22;
    public static string Username => "myuser";
    public static string Password => "mypassword";
    public static string BaseTestPath => "/home/myuser/sftp-tests";

    // Optional settings (uncomment if needed)
    // public static string? PrivateKeyPath => "/home/myuser/.ssh/id_rsa";
    // public static string? PrivateKeyPassphrase => "my-key-passphrase";
    // public static int ConnectionTimeout => 60000;
    // public static bool SkipIntegrationTests => true;
}
## ?? Tips for Success

1. **Start with the connection test** to verify your configuration
2. **Use your user's home directory** as the base path when possible
3. **Ensure proper permissions** on the target directory
4. **Test credentials manually** with an SFTP client first
5. **Check the server logs** if tests fail unexpectedly

This integration test suite ensures your SftpClient library works correctly with real SFTP servers!