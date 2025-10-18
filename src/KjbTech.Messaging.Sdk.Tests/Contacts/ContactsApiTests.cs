using KjbTech.Messaging.Sdk.Contacts;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace KjbTech.Messaging.Sdk.Tests.Contacts;

/// <summary>
/// We want to check that deserialization is ok.
/// </summary>
[Trait("Category", "Contacts")]
public class ContactsApiTests
{
    [Fact]
    public async Task Get_ExistingContact_MustBeAValidObject()
    {
        var expectedResponse = await File.ReadAllTextAsync("Contacts/get_contact_200.json");

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When("*")
                .Respond("application/json", expectedResponse);
        var httpClient = mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri("http://localhost/");
        var api = new ContactsApi(
            new NullLogger<ContactsApi>(),
            httpClient);

        var contact = await api.GetAsync(new ContactId("fefe"));

        Assert.NotNull(contact);
        Assert.Equal("string", contact.Name);
        Assert.Equal("+33601010101", contact.Phone);
    }
}
