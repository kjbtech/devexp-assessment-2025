using KjbTech.Messaging.Sdk.Contacts;
using KjbTech.Messaging.Sdk.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace KjbTech.Messaging.Sdk.IntegrationTests.Messages;

[Trait("Category", "Messages")]
[Trait("Category", "Integration")]
public class MessagesHttpApiTests : MessagingHttpApiTestsBase
{
    private readonly MessagesHttpApi _messagesApi;

    public MessagesHttpApiTests() : base()
    {
        _messagesApi = _serviceProvider.GetRequiredService<MessagesHttpApi>();
    }

    [Fact]
    public async Task ListEnqueuedMessageToSendAsync_MustSuccess()
    {
        // Arrange
        var messageSentResult1 = await _messagesApi.EnqueueToSendAsync(
            new MessageToSendToANumber("Voldemort", "+33601010102")
            {
                Content = "Experliarmus!",
                From = "+33601010101"
            }
        );

        var messageSentResult2 = await _messagesApi.EnqueueToSendAsync(
            new MessageToSendToANumber("Harry Potter", "+33601010101")
            {
                Content = "Avada Kedavra!",
                From = "+33601010102"
            }
        );

        // Act
        var listResult = await _messagesApi.ListEnqueuedMessageToSendAsync(
            new MessagesPaginationParameter()
        );
        await CleanContactsWithAssertAsync();

        // Assert
        Assert.True(listResult.IsSuccess);
        Assert.True(listResult.Value.PageSize >= 2);
        var avadaMessage = listResult.Value.Items.OrderByDescending(m => m.CreatedAt).First();
        Assert.Equal("Avada Kedavra!", avadaMessage.Content);
        Assert.Equal("+33601010102", avadaMessage.From);
        Assert.True(avadaMessage.CreatedAt >= DateTime.MinValue);
    }

    [Fact]
    public async Task EnqueueToSendAsync_ToAPhone_MustSuccess()
    {
        var messageSentResult = await _messagesApi.EnqueueToSendAsync(
            new MessageToSendToANumber("Voldemort", "+33601010102")
            {
                Content = "Experliarmus!",
                From = "+33601010101"
            }
        );

        await CleanContactsWithAssertAsync();

        Assert.True(messageSentResult.IsSuccess);
        Assert.Equal("Experliarmus!", messageSentResult.Value.Content);
        Assert.Equal("+33601010101", messageSentResult.Value.From);
        Assert.NotNull(messageSentResult.Value.RecipientContactId);
        Assert.True(messageSentResult.Value.CreatedAt >= DateTime.MinValue);
    }

    [Fact]
    public async Task EnqueueToSendAsync_ToARegisteredContact_MustSuccess()
    {
        var contactCreated = await _contactsApi.CreateAsync(
            new ContactToCreate()
            {
                Name = "Voldemort",
                Phone = "+33601010102"
            }
        );
        Assert.True(contactCreated.IsSuccess);

        var messageSentResult = await _messagesApi.EnqueueToSendAsync(
            new MessageToSendToARegisteredContact()
            {
                To = new ExistingContactForAMessageToSend { ContactId = contactCreated.Value!.Id },
                Content = "Experliarmus!",
                From = "+33601010101"
            }
        );

        await CleanContactWithAssertAsync(new ContactId(contactCreated.Value.Id));

        Assert.True(messageSentResult.IsSuccess);
        Assert.Equal("Experliarmus!", messageSentResult.Value.Content);
        Assert.Equal("+33601010101", messageSentResult.Value.From);
        Assert.NotNull(messageSentResult.Value.RecipientContactId);
        Assert.True(messageSentResult.Value.CreatedAt >= DateTime.MinValue);
    }
}
