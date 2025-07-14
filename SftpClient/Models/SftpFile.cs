using Renci.SshNet.Sftp;

namespace SftpClient.Models;

/// <summary>
/// Represents a file or directory on an SFTP server
/// </summary>
public class SftpFile : Interfaces.ISftpFile
{
    private readonly SftpFileAttributes _attributes;

    /// <summary>
    /// Initializes a new instance of the <see cref="SftpFile"/> class
    /// </summary>
    /// <param name="fullPath">Full path of the file or directory</param>
    /// <param name="attributes">SFTP file attributes</param>
    public SftpFile(string fullPath, SftpFileAttributes attributes)
    {
        if (string.IsNullOrEmpty(fullPath))
            throw new ArgumentException("Full path cannot be null or empty", nameof(fullPath));

        _attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
        FullPath = fullPath;
        Name = Path.GetFileName(fullPath);
    }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public string FullPath { get; }

    /// <inheritdoc />
    public long Size => _attributes.Size;

    /// <inheritdoc />
    public DateTime LastModified => _attributes.LastWriteTime;

    /// <inheritdoc />
    public bool IsDirectory => _attributes.IsDirectory;

    /// <inheritdoc />
    public string Permissions => _attributes.GetOctalPermissions();
}