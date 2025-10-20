using KjbTech.Messaging.Sdk.Contacts;
using RichardSzalay.MockHttp;

namespace KjbTech.Messaging.Sdk.Tests.Contacts;

/// <summary>
/// We want to check that deserialization is ok.
/// </summary>
[Trait("Category", "Contacts")]
public class ContactsHttpApiTests
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
        var api = new ContactsHttpApi(httpClient);

        var getContact = await api.GetAsync(new ContactId("fefe"));

        Assert.True(getContact.IsSuccess);
        Assert.Equal("string", getContact.Value.Name);
        Assert.Equal("+33601010101", getContact.Value.Phone);
    }
}
