namespace SftpClient.Interfaces;

/// <summary>
/// Interface for SFTP client operations
/// </summary>
public interface ISftpClient : IDisposable
{
    /// <summary>
    /// Connects to the SFTP server
    /// </summary>
    void Connect();
    
    /// <summary>
    /// Connects to the SFTP server asynchronously
    /// </summary>
    Task ConnectAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Disconnects from the SFTP server
    /// </summary>
    void Disconnect();
    
    /// <summary>
    /// Checks if the client is connected to the server
    /// </summary>
    bool IsConnected { get; }
    
    /// <summary>
    /// Uploads a file to the SFTP server
    /// </summary>
    /// <param name="localFilePath">Path to the local file to upload</param>
    /// <param name="remoteFilePath">Path on the server where the file should be uploaded</param>
    void UploadFile(string localFilePath, string remoteFilePath);
    
    /// <summary>
    /// Uploads a file to the SFTP server asynchronously
    /// </summary>
    /// <param name="localFilePath">Path to the local file to upload</param>
    /// <param name="remoteFilePath">Path on the server where the file should be uploaded</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task UploadFileAsync(string localFilePath, string remoteFilePath, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Uploads a stream to the SFTP server
    /// </summary>
    /// <param name="stream">Stream to upload</param>
    /// <param name="remoteFilePath">Path on the server where the stream should be uploaded</param>
    void UploadStream(Stream stream, string remoteFilePath);
    
    /// <summary>
    /// Uploads a stream to the SFTP server asynchronously
    /// </summary>
    /// <param name="stream">Stream to upload</param>
    /// <param name="remoteFilePath">Path on the server where the stream should be uploaded</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task UploadStreamAsync(Stream stream, string remoteFilePath, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Downloads a file from the SFTP server
    /// </summary>
    /// <param name="remoteFilePath">Path to the file on the server</param>
    /// <param name="localFilePath">Path where the file should be saved locally</param>
    void DownloadFile(string remoteFilePath, string localFilePath);
    
    /// <summary>
    /// Downloads a file from the SFTP server asynchronously
    /// </summary>
    /// <param name="remoteFilePath">Path to the file on the server</param>
    /// <param name="localFilePath">Path where the file should be saved locally</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DownloadFileAsync(string remoteFilePath, string localFilePath, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Downloads a file from the SFTP server to a stream
    /// </summary>
    /// <param name="remoteFilePath">Path to the file on the server</param>
    /// <param name="stream">Stream where the file should be written</param>
    void DownloadStream(string remoteFilePath, Stream stream);
    
    /// <summary>
    /// Downloads a file from the SFTP server to a stream asynchronously
    /// </summary>
    /// <param name="remoteFilePath">Path to the file on the server</param>
    /// <param name="stream">Stream where the file should be written</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DownloadStreamAsync(string remoteFilePath, Stream stream, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Lists files in a directory on the SFTP server
    /// </summary>
    /// <param name="remotePath">Path to list</param>
    /// <returns>List of file information</returns>
    IEnumerable<ISftpFile> ListDirectory(string remotePath);
    
    /// <summary>
    /// Lists files in a directory on the SFTP server asynchronously
    /// </summary>
    /// <param name="remotePath">Path to list</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of file information</returns>
    Task<IEnumerable<ISftpFile>> ListDirectoryAsync(string remotePath, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Checks if a file exists on the SFTP server
    /// </summary>
    /// <param name="remotePath">Path to check</param>
    /// <returns>True if the file exists, false otherwise</returns>
    bool FileExists(string remotePath);
    
    /// <summary>
    /// Checks if a file exists on the SFTP server asynchronously
    /// </summary>
    /// <param name="remotePath">Path to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the file exists, false otherwise</returns>
    Task<bool> FileExistsAsync(string remotePath, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes a file on the SFTP server
    /// </summary>
    /// <param name="remotePath">Path to the file to delete</param>
    void DeleteFile(string remotePath);
    
    /// <summary>
    /// Deletes a file on the SFTP server asynchronously
    /// </summary>
    /// <param name="remotePath">Path to the file to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DeleteFileAsync(string remotePath, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates a directory on the SFTP server
    /// </summary>
    /// <param name="remotePath">Path of the directory to create</param>
    void CreateDirectory(string remotePath);
    
    /// <summary>
    /// Creates a directory on the SFTP server asynchronously
    /// </summary>
    /// <param name="remotePath">Path of the directory to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task CreateDirectoryAsync(string remotePath, CancellationToken cancellationToken = default);
}