using KjbTech.Messaging.Sdk.Contacts;

namespace KjbTech.Messaging.Sdk.IntegrationTests.Contacts;

[Trait("Category", "Contacts")]
[Trait("Category", "Integration")]
public class ContactsHttpApiTests : MessagingHttpApiTestsBase
{
    public ContactsHttpApiTests() : base()
    {
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

        await CleanContactWithAssertAsync(new ContactId(contactCreated.Value.Id));
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

        await CleanContactWithAssertAsync(new ContactId(firstContactCreated.Value!.Id));
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

        await CleanContactWithAssertAsync(new ContactId(contactCreated.Value!.Id));
    }

    [Fact]
    public async Task List_MustBeAValidObject()
    {
        var contactCreated = await _contactsApi.CreateAsync(
            new ContactToCreate()
            {
                Name = "Harry Potter",
                Phone = "+33601010101"
            }
        );

        var listContactResult = await _contactsApi.ListAsync(new PaginationParameter());

        Assert.True(listContactResult.IsSuccess);
        Assert.NotEmpty(listContactResult.Value.Items);
        Assert.Equal(1, listContactResult.Value.PageNumber);
        Assert.Equal(10, listContactResult.Value.PageSize);

        await CleanContactWithAssertAsync(new ContactId(contactCreated.Value!.Id));
    }
}
