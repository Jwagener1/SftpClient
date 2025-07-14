using Renci.SshNet.Sftp;
using SftpClient.Extensions;

namespace SftpClient.Tests.Extensions;

public class SftpFileAttributesExtensionsTests
{
    [Fact]
    public void GetOctalPermissions_NullAttributes_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => ((SftpFileAttributes)null!).GetOctalPermissions());
    }

    // Note: Testing the actual permission calculation requires creating SftpFileAttributes objects
    // which has complex constructor requirements. These tests focus on the null check for now.
    // In a real scenario, integration tests would verify the permission calculation.
}