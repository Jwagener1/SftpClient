using Renci.SshNet.Sftp;

namespace SftpClient.Extensions;

/// <summary>
/// Extension methods for SftpFileAttributes
/// </summary>
public static class SftpFileAttributesExtensions
{
    /// <summary>
    /// Gets the file permissions in octal format (e.g., 755, 644)
    /// </summary>
    /// <param name="attributes">The file attributes</param>
    /// <returns>A string representation of the octal permissions</returns>
    public static string GetOctalPermissions(this SftpFileAttributes attributes)
    {
        if (attributes == null)
            throw new ArgumentNullException(nameof(attributes));

        int ownerPermissions = 0;
        if (attributes.OwnerCanRead) ownerPermissions += 4;
        if (attributes.OwnerCanWrite) ownerPermissions += 2;
        if (attributes.OwnerCanExecute) ownerPermissions += 1;

        int groupPermissions = 0;
        if (attributes.GroupCanRead) groupPermissions += 4;
        if (attributes.GroupCanWrite) groupPermissions += 2;
        if (attributes.GroupCanExecute) groupPermissions += 1;

        int otherPermissions = 0;
        if (attributes.OthersCanRead) otherPermissions += 4;
        if (attributes.OthersCanWrite) otherPermissions += 2;
        if (attributes.OthersCanExecute) otherPermissions += 1;

        return $"{ownerPermissions}{groupPermissions}{otherPermissions}";
    }
}