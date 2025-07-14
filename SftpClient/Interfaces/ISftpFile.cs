namespace SftpClient.Interfaces;

/// <summary>
/// Interface representing a file or directory on an SFTP server
/// </summary>
public interface ISftpFile
{
    /// <summary>
    /// Gets the name of the file or directory
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// Gets the full path of the file or directory
    /// </summary>
    string FullPath { get; }
    
    /// <summary>
    /// Gets the size of the file in bytes
    /// </summary>
    long Size { get; }
    
    /// <summary>
    /// Gets the last modification time of the file or directory
    /// </summary>
    DateTime LastModified { get; }
    
    /// <summary>
    /// Gets a value indicating whether this is a directory
    /// </summary>
    bool IsDirectory { get; }
    
    /// <summary>
    /// Gets the permissions of the file or directory
    /// </summary>
    string Permissions { get; }
}