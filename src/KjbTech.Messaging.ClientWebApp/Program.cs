using KjbTech.Messaging.Sdk.Messages;
using KjbTech.Messaging.Sdk.Webhook;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

var secret = Environment.GetEnvironmentVariable("MessagingApi__Secret") ?? "alohomora";

app.MapPost("/", async (HttpRequest httpRequest, HttpResponse httpResponse) =>
{
    httpRequest.EnableBuffering(); // allow multiple reads
    using var reader = new StreamReader(httpRequest.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: false);
    var body = await reader.ReadToEndAsync();

    var auth = httpRequest.Headers.Authorization.ToString();

    if (WebhookHelper.TryParse<MessageDeliveryEvent>(body, secret!, auth, out var @event))
    {
        Console.WriteLine($" Message {@event!.Id} -> {@event.Status}");
        return Results.Ok();
    }

    Console.WriteLine($"We are not able to handle this payload: '{body}'");

    return Results.BadRequest("We did not handle this event.");
});

app.Run();
