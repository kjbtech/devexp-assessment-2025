using KjbTech.Messaging.Sdk.Contacts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace KjbTech.Messaging.Sdk.Tests.Contacts;

/// <summary>
/// We want to check that deserialization is ok.
/// </summary>
[Trait("Category", "DI")]
public class CheckDependencyInjection
{
    [Fact]
    public void Validate()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Messaging:ApiBaseUrl"] = "https://api.test.local",
                ["Messaging:ApiKey"] = "test-api-key"
            })
            .Build();

        var services = new ServiceCollection();

        services.AddMessaging(config);

        // Build provider
        using var provider = services.BuildServiceProvider();

        var options = provider.GetRequiredService<IOptions<MessagingEndpoint>>().Value;
        Assert.Equal("https://api.test.local", options.ApiBaseUrl);
        Assert.Equal("test-api-key", options.ApiKey);

        // And check that HttpClient is properly configured
        var contactsApi = provider.GetRequiredService<ContactsApi>();
        Assert.NotNull(contactsApi);
    }
}
