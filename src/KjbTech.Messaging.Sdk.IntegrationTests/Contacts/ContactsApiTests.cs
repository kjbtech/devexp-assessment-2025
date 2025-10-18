using KjbTech.Messaging.Sdk.Contacts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KjbTech.Messaging.Sdk.IntegrationTests.Contacts;

/// <summary>
/// We want to check that deserialization is ok.
/// </summary>
[Trait("Category", "Contacts")]
public class ContactsApiTests
{
    private readonly ContactsApi _contactsApi;

    public ContactsApiTests()
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
        var provider = services.BuildServiceProvider();

        // And check that HttpClient is properly configured
        _contactsApi = provider.GetRequiredService<ContactsApi>();
    }

    [Fact]
    public async Task Create_MustSuccess()
    {
        var contactCreated = await _contactsApi.CreateAsync(
            new ContactToCreate()
            {
                Name = "Harry Potter",
                Phone = "+33601010101"
            }
        );

        Assert.True(contactCreated.IsSuccess);
        Assert.Equal("Harry Potter", contactCreated.Value.Name);
        Assert.Equal("+33601010101", contactCreated.Value.Phone);

        await CleanDataWithAssertAsync(new ContactId(contactCreated.Value.Id));
    }

    [Fact]
    public async Task Create_WithExistingContact_MustFail()
    {
        var firstContactCreated = await _contactsApi.CreateAsync(
            new ContactToCreate()
            {
                Name = "Harry Potter",
                Phone = "+33601010101"
            }
        );

        var secondContactCreated = await _contactsApi.CreateAsync(
            new ContactToCreate()
            {
                Name = "Harry Potter",
                Phone = "+33601010101"
            }
        );

        Assert.True(secondContactCreated.HasFailed);
        Assert.NotNull(secondContactCreated.Error);

        await CleanDataWithAssertAsync(new ContactId(firstContactCreated.Value!.Id));
    }

    [Fact]
    public async Task Get_ExistingContact_MustBeAValidObject()
    {
        var contactCreated = await _contactsApi.CreateAsync(
            new ContactToCreate()
            {
                Name = "Harry Potter",
                Phone = "+33601010101"
            }
        );

        var contact = await _contactsApi.GetAsync(new ContactId(contactCreated.Value!.Id));

        Assert.NotNull(contact);
        Assert.Equal("Harry Potter", contact.Name);
        Assert.Equal("+33601010101", contact.Phone);

        await CleanDataWithAssertAsync(new ContactId(contactCreated.Value!.Id));
    }

    private async Task CleanDataWithAssertAsync(ContactId contactId)
    {
        var contactDeletion = await _contactsApi.DeleteAsync(contactId);

        Assert.True(contactDeletion.IsSuccess);
    }
}
