using KjbTech.Messaging.Sdk.Contacts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KjbTech.Messaging.Sdk.IntegrationTests;

public abstract class MessagingHttpApiTestsBase
{
    protected readonly ServiceProvider _serviceProvider;
    protected readonly ContactsHttpApi _contactsApi;

    protected MessagingHttpApiTestsBase()
    {
        var config = new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Messaging:ApiBaseUrl"] = "http://localhost:3000",
            ["Messaging:ApiKey"] = "there-is-no-key"
        })
        .Build();

        var services = new ServiceCollection();

        services.AddMessaging(config);

        // Build provider
        _serviceProvider = services.BuildServiceProvider();

        // And check that HttpClient is properly configured
        _contactsApi = _serviceProvider.GetRequiredService<ContactsHttpApi>();
    }

    protected async Task CleanContactWithAssertAsync(ContactId contactId)
    {
        var contactDeletion = await _contactsApi.DeleteAsync(contactId);

        Assert.True(contactDeletion.IsSuccess);
    }

    protected async Task CleanContactsWithAssertAsync()
    {
        var contactsList = await _contactsApi.ListAsync(new PaginationParameter());

        Assert.True(contactsList.IsSuccess);

        foreach (var existingContact in contactsList.Value.Items)
        {
            var contactDeletion = await _contactsApi.DeleteAsync(new ContactId(existingContact.Id));

            Assert.True(contactDeletion.IsSuccess);
        }
    }
}
