namespace SftpClient.Tests;

public class SftpClientFactoryTests
{
    [Fact]
    public void CreateBuilder_ReturnsNewSftpClientBuilder()
    {
        // Act
        var builder = SftpClientFactory.CreateBuilder();

        // Assert
        Assert.NotNull(builder);
        Assert.IsType<SftpClientBuilder>(builder);
    }

    [Fact]
    public void CreateBuilder_MultipleCalls_ReturnsDifferentInstances()
    {
        // Act
        var builder1 = SftpClientFactory.CreateBuilder();
        var builder2 = SftpClientFactory.CreateBuilder();

        // Assert
        Assert.NotSame(builder1, builder2);
    }
}