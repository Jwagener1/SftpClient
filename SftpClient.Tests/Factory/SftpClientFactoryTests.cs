using SftpClient.Factory;

namespace SftpClient.Tests.Factory;

public class SftpClientFactoryTests
{
    [Fact]
    public void Constructor_CreatesInstance()
    {
        // Act
        var factory = new SftpClientFactory();

        // Assert
        Assert.NotNull(factory);
    }

    // Note: Testing the CreateClient method requires properly mocked dependencies
    // which reference the internal SSH.NET types. These tests focus on basic
    // instantiation for now.
}