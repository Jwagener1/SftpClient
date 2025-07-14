namespace SftpClient.IntegrationTests;

/// <summary>
/// Tests for the TestConfiguration class to verify configuration values
/// </summary>
public class TestConfigurationTests
{
    [Fact]
    public void TestConfiguration_DefaultValues_AreCorrect()
    {
        // Act & Assert
        Assert.Equal("192.168.133.2", TestConfiguration.Host);
        Assert.Equal(22, TestConfiguration.Port);
        Assert.Equal("jonathan", TestConfiguration.Username);
        Assert.Equal("Wagener1", TestConfiguration.Password);
        Assert.Equal("/home/jonathan/sftp-integration-tests", TestConfiguration.BaseTestPath);
        Assert.Equal(30000, TestConfiguration.ConnectionTimeout);
        Assert.Equal(60000, TestConfiguration.OperationTimeout);
        Assert.False(TestConfiguration.SkipIntegrationTests);
    }

    [Fact]
    public void CreateTestClientBuilder_WithDefaults_ReturnsConfiguredBuilder()
    {
        // Act
        var builder = TestConfiguration.CreateTestClientBuilder();

        // Assert
        Assert.NotNull(builder);
        
        // Build client to verify configuration
        using var client = builder.Build();
        Assert.NotNull(client);
    }

    [Fact]
    public void UsePrivateKey_WhenNoPrivateKeySet_ReturnsFalse()
    {
        // Act & Assert
        Assert.False(TestConfiguration.UsePrivateKey);
    }

    [Fact]
    public void Configuration_RequiredValues_AreNotEmpty()
    {
        // Act & Assert
        Assert.False(string.IsNullOrWhiteSpace(TestConfiguration.Host));
        Assert.False(string.IsNullOrWhiteSpace(TestConfiguration.Username));
        Assert.False(string.IsNullOrWhiteSpace(TestConfiguration.Password));
        Assert.False(string.IsNullOrWhiteSpace(TestConfiguration.BaseTestPath));
        Assert.True(TestConfiguration.Port > 0);
        Assert.True(TestConfiguration.ConnectionTimeout > 0);
        Assert.True(TestConfiguration.OperationTimeout > 0);
    }
}