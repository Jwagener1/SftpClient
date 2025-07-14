using Moq;
using SftpClient.Authentication;
using SftpClient.Interfaces;

namespace SftpClient.Tests.Authentication;

public class PrivateKeyAuthenticationMethodTests
{
    [Fact]
    public void Constructor_ValidPrivateKeyPath_CreatesInstance()
    {
        // Arrange
        const string privateKeyPath = "/path/to/key";

        // Act
        var auth = new PrivateKeyAuthenticationMethod(privateKeyPath);

        // Assert
        Assert.NotNull(auth);
    }

    [Fact]
    public void Constructor_ValidPrivateKeyPathWithPassphrase_CreatesInstance()
    {
        // Arrange
        const string privateKeyPath = "/path/to/key";
        const string passphrase = "passphrase";

        // Act
        var auth = new PrivateKeyAuthenticationMethod(privateKeyPath, passphrase);

        // Assert
        Assert.NotNull(auth);
    }

    [Fact]
    public void Constructor_NullPrivateKeyPath_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new PrivateKeyAuthenticationMethod(null!));
    }

    [Fact]
    public void GetConnectionInfo_NullConfiguration_ThrowsArgumentNullException()
    {
        // Arrange
        var auth = new PrivateKeyAuthenticationMethod("/path/to/key");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => auth.GetConnectionInfo(null!));
    }

    // Note: Testing GetConnectionInfo with a real private key file requires
    // complex setup and valid key files. This is better tested in integration tests
    // where real SSH key files can be used.
}