# Integration Tests Troubleshooting Guide

This guide will help you diagnose and fix common issues when running the SftpClient integration tests.

## ?? Quick Start

1. **First, configure your SFTP server settings in `TestConfiguration.cs`**
2. **Run the basic connectivity test:**dotnet test SftpClient.IntegrationTests --filter "CanConnectToSftpServer"3. **If that works, try the full test suite:**dotnet test SftpClient.IntegrationTests
## ?? Configuration Setup

### Update TestConfiguration.cs

Open `SftpClient.IntegrationTests/TestConfiguration.cs` and update these values:
public static string Host => "192.168.133.2";        // Your SFTP server
public static string Username => "jonathan";          // Your username
public static string Password => "Wagener1";          // Your password
public static string BaseTestPath => "/home/jonathan/sftp-integration-tests";
### Alternative Configurations

**For different server:**public static string Host => "your-server.com";
public static string Username => "your-username";
public static string Password => "your-password";
**For private key authentication:**public static string Username => "your-username";
public static string? PrivateKeyPath => "/path/to/your/key.pem";
public static string? PrivateKeyPassphrase => "optional-passphrase";
// Password is ignored when using private key
**To skip tests temporarily:**public static bool SkipIntegrationTests => true;
## ?? Common Issues and Solutions

### 1. Connection Refused / Host Unreachable

**Error:** `Failed to connect to SFTP server`

**Solutions:**
- Verify the SFTP server is running
- Check the `Host` and `Port` values in `TestConfiguration.cs`
- Ensure firewall allows connections on port 22 (or your custom port)
- Try connecting manually with an SFTP client like FileZilla or WinSCP

**Test:**dotnet test SftpClient.IntegrationTests --filter "CanConnectToSftpServer"
### 2. Authentication Failed

**Error:** `Authentication failed for user`

**Solutions:**
- Verify `Username` and `Password` in `TestConfiguration.cs`
- Check if the user account is enabled
- Ensure the user has SFTP access permissions
- Try connecting manually to verify credentials

**For private key authentication:**public static string? PrivateKeyPath => "/path/to/your/key.pem";
public static string? PrivateKeyPassphrase => "your-passphrase";
### 3. Permission Denied / Directory Creation Failed

**Error:** `Failed to create or access directory`

**Solutions:**
- Update `BaseTestPath` in `TestConfiguration.cs` to a writable directory:public static string BaseTestPath => "/home/jonathan/test-files";- Or try the temporary directory:public static string BaseTestPath => "/tmp/sftp-tests";- Ensure the user has write permissions to the specified directory

### 4. Directory Structure Issues

**Error:** `Remote path not found` or `No such file`

**Solutions:**
- The integration tests now automatically create directories recursively
- Ensure the base path is writable
- Try using your user's home directory:public static string BaseTestPath => "/home/jonathan/sftp-integration-tests";
### 5. Tests Taking Too Long / Timeouts

**Error:** `Operation timed out`

**Solutions:**
- Increase timeout values in `TestConfiguration.cs`:public static int ConnectionTimeout => 60000;   // 60 seconds
public static int OperationTimeout => 120000; // 2 minutes- Check network connectivity
- Ensure the SFTP server is responsive

### 6. Tests Failing Due to Server Issues

**Error:** Various SFTP operation errors

**Solutions:**
- Skip integration tests temporarily:public static bool SkipIntegrationTests => true;- Run only connectivity tests:dotnet test SftpClient.IntegrationTests --filter "CanConnect"
## ?? Diagnostic Commands

### Test Current Configuration# Basic connectivity test
dotnet test SftpClient.IntegrationTests --filter "CanConnectToSftpServer"

# Configuration validation test
dotnet test SftpClient.IntegrationTests --filter "TestConfiguration"

# Simple file operation test
dotnet test SftpClient.IntegrationTests --filter "CanUploadAndDownloadTextFile"
### Test Connection with Built-in SSH/SFTP Client# Test SSH connection (replace with your values)
ssh jonathan@192.168.133.2

# Test SFTP connection (replace with your values)
sftp jonathan@192.168.133.2
### Run Tests with Verbose Outputdotnet test SftpClient.IntegrationTests --verbosity normal
### Run Individual Test Groups# Connection tests only
dotnet test SftpClient.IntegrationTests --filter "CanConnect"

# File upload/download tests
dotnet test SftpClient.IntegrationTests --filter "UploadAndDownload"

# Directory operations
dotnet test SftpClient.IntegrationTests --filter "Directory"
## ?? Health Check Tests

Run these tests in order to identify where the issue occurs:

### 1. Basic Connection Testdotnet test SftpClient.IntegrationTests --filter "CanConnectToSftpServer"
### 2. Configuration Testdotnet test SftpClient.IntegrationTests --filter "TestConfiguration"
### 3. Simple Upload/Download Testdotnet test SftpClient.IntegrationTests --filter "CanUploadAndDownloadTextFile"
## ?? Configuration Reference

Edit these values in `TestConfiguration.cs`:

| Property | Description | Example |
|----------|-------------|---------|
| `Host` | SFTP server host | `"192.168.133.2"` |
| `Port` | SFTP server port | `22` |
| `Username` | Username | `"jonathan"` |
| `Password` | Password | `"Wagener1"` |
| `BaseTestPath` | Base test directory | `"/home/jonathan/test-files"` |
| `PrivateKeyPath` | Private key file path | `"/path/to/key.pem"` |
| `SkipIntegrationTests` | Skip integration tests | `true` |
| `ConnectionTimeout` | Connection timeout (ms) | `30000` |
| `OperationTimeout` | Operation timeout (ms) | `60000` |

## ?? Getting Help

If you're still having issues:

1. **Check the error message carefully** - it often contains the exact reason for failure
2. **Try manual SFTP connection** - use FileZilla, WinSCP, or command-line SFTP
3. **Check server logs** - look for authentication or permission errors
4. **Verify network connectivity** - ensure the server is reachable
5. **Test with different base paths** - try writable directories like `/tmp` or user home

## ?? Tips for Success

1. **Start with connectivity tests** before running the full suite
2. **Use your user's home directory** as the base path when possible
3. **Ensure proper permissions** on the target directory
4. **Test credentials manually** before running automated tests
5. **Consider using Docker** for a consistent test environment

## ?? Docker Alternative

If you don't have access to an SFTP server, you can use Docker:
# Run a test SFTP server
docker run -d \
  --name sftp-test \
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
## ?? Example Configurations

### Basic Configurationpublic static class TestConfiguration
{
    public static string Host => "192.168.1.100";
    public static int Port => 22;
    public static string Username => "myuser";
    public static string Password => "mypassword";
    public static string BaseTestPath => "/home/myuser/sftp-tests";
}
### Private Key Configurationpublic static class TestConfiguration
{
    public static string Host => "192.168.1.100";
    public static string Username => "myuser";
    public static string? PrivateKeyPath => "/home/myuser/.ssh/id_rsa";
    public static string? PrivateKeyPassphrase => "my-passphrase";
    public static string BaseTestPath => "/home/myuser/sftp-tests";
}
### Skip Tests Configurationpublic static class TestConfiguration
{
    // ... other settings ...
    public static bool SkipIntegrationTests => true;  // Skip all tests
}
This troubleshooting guide should help you identify and resolve most common issues with the integration tests.