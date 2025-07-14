using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
using SftpClient.Exceptions;
using SftpClient.Extensions;
using SftpClient.Models;
using SshNetSftpClient = Renci.SshNet.SftpClient;

namespace SftpClient;

/// <summary>
/// Implementation of the SFTP client using SSH.NET
/// </summary>
internal class SftpNetClient : Interfaces.ISftpClient
{
    private readonly Interfaces.IConnectionConfiguration _configuration;
    private readonly Interfaces.IAuthenticationMethod _authenticationMethod;
    private readonly SshNetSftpClient _client;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="SftpNetClient"/> class
    /// </summary>
    /// <param name="configuration">The connection configuration</param>
    /// <param name="authenticationMethod">The authentication method</param>
    public SftpNetClient(Interfaces.IConnectionConfiguration configuration, Interfaces.IAuthenticationMethod authenticationMethod)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _authenticationMethod = authenticationMethod ?? throw new ArgumentNullException(nameof(authenticationMethod));

        var connectionInfo = _authenticationMethod.GetConnectionInfo(_configuration);
        _client = new SshNetSftpClient(connectionInfo);
        _client.OperationTimeout = TimeSpan.FromMilliseconds(_configuration.OperationTimeout);
    }

    /// <inheritdoc />
    public bool IsConnected => _client.IsConnected;

    /// <inheritdoc />
    public void Connect()
    {
        EnsureNotDisposed();
        try
        {
            _client.Connect();
        }
        catch (SshConnectionException ex)
        {
            throw new SftpConnectionException($"Failed to connect to SFTP server at {_configuration.Host}:{_configuration.Port}", ex);
        }
        catch (SshAuthenticationException ex)
        {
            throw new SftpConnectionException($"Authentication failed for user '{_configuration.Username}' on server {_configuration.Host}", ex);
        }
        catch (Exception ex)
        {
            throw new SftpException("An unexpected error occurred while connecting to the SFTP server", ex);
        }
    }

    /// <inheritdoc />
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        EnsureNotDisposed();
        try
        {
            await Task.Run(() => _client.Connect(), cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (SshConnectionException ex)
        {
            throw new SftpConnectionException($"Failed to connect to SFTP server at {_configuration.Host}:{_configuration.Port}", ex);
        }
        catch (SshAuthenticationException ex)
        {
            throw new SftpConnectionException($"Authentication failed for user '{_configuration.Username}' on server {_configuration.Host}", ex);
        }
        catch (Exception ex)
        {
            throw new SftpException("An unexpected error occurred while connecting to the SFTP server", ex);
        }
    }

    /// <inheritdoc />
    public void Disconnect()
    {
        if (!_disposed && _client.IsConnected)
        {
            _client.Disconnect();
        }
    }

    /// <inheritdoc />
    public void UploadFile(string localFilePath, string remoteFilePath)
    {
        EnsureConnected();
        try
        {
            using var fileStream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read);
            _client.UploadFile(fileStream, remoteFilePath);
        }
        catch (FileNotFoundException ex)
        {
            throw new SftpFileOperationException($"Local file not found: {localFilePath}", ex, localFilePath);
        }
        catch (SftpPathNotFoundException ex)
        {
            throw new SftpFileOperationException($"Remote path not found: {remoteFilePath}", ex, remoteFilePath);
        }
        catch (SftpPermissionDeniedException ex)
        {
            throw new SftpFileOperationException($"Permission denied when uploading to {remoteFilePath}", ex, remoteFilePath);
        }
        catch (Exception ex)
        {
            throw new SftpFileOperationException($"Failed to upload file from {localFilePath} to {remoteFilePath}", ex, localFilePath);
        }
    }

    /// <inheritdoc />
    public async Task UploadFileAsync(string localFilePath, string remoteFilePath, CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        try
        {
            using var fileStream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read);
            await Task.Run(() => _client.UploadFile(fileStream, remoteFilePath), cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (FileNotFoundException ex)
        {
            throw new SftpFileOperationException($"Local file not found: {localFilePath}", ex, localFilePath);
        }
        catch (SftpPathNotFoundException ex)
        {
            throw new SftpFileOperationException($"Remote path not found: {remoteFilePath}", ex, remoteFilePath);
        }
        catch (SftpPermissionDeniedException ex)
        {
            throw new SftpFileOperationException($"Permission denied when uploading to {remoteFilePath}", ex, remoteFilePath);
        }
        catch (Exception ex)
        {
            throw new SftpFileOperationException($"Failed to upload file from {localFilePath} to {remoteFilePath}", ex, localFilePath);
        }
    }

    /// <inheritdoc />
    public void UploadStream(Stream stream, string remoteFilePath)
    {
        EnsureConnected();
        try
        {
            _client.UploadFile(stream, remoteFilePath);
        }
        catch (SftpPathNotFoundException ex)
        {
            throw new SftpFileOperationException($"Remote path not found: {remoteFilePath}", ex, remoteFilePath);
        }
        catch (SftpPermissionDeniedException ex)
        {
            throw new SftpFileOperationException($"Permission denied when uploading to {remoteFilePath}", ex, remoteFilePath);
        }
        catch (Exception ex)
        {
            throw new SftpFileOperationException($"Failed to upload stream to {remoteFilePath}", ex, remoteFilePath);
        }
    }

    /// <inheritdoc />
    public async Task UploadStreamAsync(Stream stream, string remoteFilePath, CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        try
        {
            await Task.Run(() => _client.UploadFile(stream, remoteFilePath), cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (SftpPathNotFoundException ex)
        {
            throw new SftpFileOperationException($"Remote path not found: {remoteFilePath}", ex, remoteFilePath);
        }
        catch (SftpPermissionDeniedException ex)
        {
            throw new SftpFileOperationException($"Permission denied when uploading to {remoteFilePath}", ex, remoteFilePath);
        }
        catch (Exception ex)
        {
            throw new SftpFileOperationException($"Failed to upload stream to {remoteFilePath}", ex, remoteFilePath);
        }
    }

    /// <inheritdoc />
    public void DownloadFile(string remoteFilePath, string localFilePath)
    {
        EnsureConnected();
        try
        {
            using var fileStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write);
            _client.DownloadFile(remoteFilePath, fileStream);
        }
        catch (SftpPathNotFoundException ex)
        {
            throw new SftpFileOperationException($"Remote file not found: {remoteFilePath}", ex, remoteFilePath);
        }
        catch (IOException ex)
        {
            throw new SftpFileOperationException($"Failed to write to local file: {localFilePath}", ex, localFilePath);
        }
        catch (Exception ex)
        {
            throw new SftpFileOperationException($"Failed to download file from {remoteFilePath} to {localFilePath}", ex, remoteFilePath);
        }
    }

    /// <inheritdoc />
    public async Task DownloadFileAsync(string remoteFilePath, string localFilePath, CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        try
        {
            using var fileStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write);
            await Task.Run(() => _client.DownloadFile(remoteFilePath, fileStream), cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (SftpPathNotFoundException ex)
        {
            throw new SftpFileOperationException($"Remote file not found: {remoteFilePath}", ex, remoteFilePath);
        }
        catch (IOException ex)
        {
            throw new SftpFileOperationException($"Failed to write to local file: {localFilePath}", ex, localFilePath);
        }
        catch (Exception ex)
        {
            throw new SftpFileOperationException($"Failed to download file from {remoteFilePath} to {localFilePath}", ex, remoteFilePath);
        }
    }

    /// <inheritdoc />
    public void DownloadStream(string remoteFilePath, Stream stream)
    {
        EnsureConnected();
        try
        {
            _client.DownloadFile(remoteFilePath, stream);
        }
        catch (SftpPathNotFoundException ex)
        {
            throw new SftpFileOperationException($"Remote file not found: {remoteFilePath}", ex, remoteFilePath);
        }
        catch (Exception ex)
        {
            throw new SftpFileOperationException($"Failed to download file from {remoteFilePath} to stream", ex, remoteFilePath);
        }
    }

    /// <inheritdoc />
    public async Task DownloadStreamAsync(string remoteFilePath, Stream stream, CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        try
        {
            await Task.Run(() => _client.DownloadFile(remoteFilePath, stream), cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (SftpPathNotFoundException ex)
        {
            throw new SftpFileOperationException($"Remote file not found: {remoteFilePath}", ex, remoteFilePath);
        }
        catch (Exception ex)
        {
            throw new SftpFileOperationException($"Failed to download file from {remoteFilePath} to stream", ex, remoteFilePath);
        }
    }

    /// <inheritdoc />
    public IEnumerable<Interfaces.ISftpFile> ListDirectory(string remotePath)
    {
        EnsureConnected();
        try
        {
            return _client.ListDirectory(remotePath)
                .Select(item => new Models.SftpFile(item.FullName, item.Attributes));
        }
        catch (SftpPathNotFoundException ex)
        {
            throw new SftpFileOperationException($"Directory not found: {remotePath}", ex, remotePath);
        }
        catch (Exception ex)
        {
            throw new SftpFileOperationException($"Failed to list directory: {remotePath}", ex, remotePath);
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Interfaces.ISftpFile>> ListDirectoryAsync(string remotePath, CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        try
        {
            return await Task.Run(() => _client.ListDirectory(remotePath)
                .Select(item => new Models.SftpFile(item.FullName, item.Attributes))
                .ToList() as IEnumerable<Interfaces.ISftpFile>, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (SftpPathNotFoundException ex)
        {
            throw new SftpFileOperationException($"Directory not found: {remotePath}", ex, remotePath);
        }
        catch (Exception ex)
        {
            throw new SftpFileOperationException($"Failed to list directory: {remotePath}", ex, remotePath);
        }
    }

    /// <inheritdoc />
    public bool FileExists(string remotePath)
    {
        EnsureConnected();
        try
        {
            var attributes = _client.GetAttributes(remotePath);
            return !attributes.IsDirectory;
        }
        catch (SftpPathNotFoundException)
        {
            return false;
        }
        catch (Exception ex)
        {
            throw new SftpFileOperationException($"Failed to check if file exists: {remotePath}", ex, remotePath);
        }
    }

    /// <inheritdoc />
    public async Task<bool> FileExistsAsync(string remotePath, CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        try
        {
            return await Task.Run(() =>
            {
                try
                {
                    var attributes = _client.GetAttributes(remotePath);
                    return !attributes.IsDirectory;
                }
                catch (SftpPathNotFoundException)
                {
                    return false;
                }
            }, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new SftpFileOperationException($"Failed to check if file exists: {remotePath}", ex, remotePath);
        }
    }

    /// <inheritdoc />
    public void DeleteFile(string remotePath)
    {
        EnsureConnected();
        try
        {
            _client.DeleteFile(remotePath);
        }
        catch (SftpPathNotFoundException ex)
        {
            throw new SftpFileOperationException($"File not found: {remotePath}", ex, remotePath);
        }
        catch (SftpPermissionDeniedException ex)
        {
            throw new SftpFileOperationException($"Permission denied when deleting file: {remotePath}", ex, remotePath);
        }
        catch (Exception ex)
        {
            throw new SftpFileOperationException($"Failed to delete file: {remotePath}", ex, remotePath);
        }
    }

    /// <inheritdoc />
    public async Task DeleteFileAsync(string remotePath, CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        try
        {
            await Task.Run(() => _client.DeleteFile(remotePath), cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (SftpPathNotFoundException ex)
        {
            throw new SftpFileOperationException($"File not found: {remotePath}", ex, remotePath);
        }
        catch (SftpPermissionDeniedException ex)
        {
            throw new SftpFileOperationException($"Permission denied when deleting file: {remotePath}", ex, remotePath);
        }
        catch (Exception ex)
        {
            throw new SftpFileOperationException($"Failed to delete file: {remotePath}", ex, remotePath);
        }
    }

    /// <inheritdoc />
    public void CreateDirectory(string remotePath)
    {
        EnsureConnected();
        try
        {
            _client.CreateDirectory(remotePath);
        }
        catch (SftpPermissionDeniedException ex)
        {
            throw new SftpFileOperationException($"Permission denied when creating directory: {remotePath}", ex, remotePath);
        }
        catch (Exception ex)
        {
            throw new SftpFileOperationException($"Failed to create directory: {remotePath}", ex, remotePath);
        }
    }

    /// <inheritdoc />
    public async Task CreateDirectoryAsync(string remotePath, CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        try
        {
            await Task.Run(() => _client.CreateDirectory(remotePath), cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (SftpPermissionDeniedException ex)
        {
            throw new SftpFileOperationException($"Permission denied when creating directory: {remotePath}", ex, remotePath);
        }
        catch (Exception ex)
        {
            throw new SftpFileOperationException($"Failed to create directory: {remotePath}", ex, remotePath);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (!_disposed)
        {
            if (_client.IsConnected)
            {
                _client.Disconnect();
            }
            _client.Dispose();
            _disposed = true;
        }
    }

    private void EnsureConnected()
    {
        EnsureNotDisposed();
        if (!_client.IsConnected)
        {
            throw new SftpConnectionException("SFTP client is not connected. Call Connect() method first.");
        }
    }

    private void EnsureNotDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(SftpNetClient));
        }
    }
}