using KjbTech.Messaging.Sdk.Configuration.Http;
using KjbTech.Messaging.Sdk.Contacts;
using KjbTech.Messaging.Sdk.Exceptions;
using KjbTech.Messaging.Sdk.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace KjbTech.Messaging.Sdk;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Build all usable helper to interact with APIs.
    /// Ensure to configure in you settings/env, a section called 'Messaging'.
    /// Observability is also defined, adding Trace id and correlation id if missing.
    /// </summary>
    /// <exception cref="MessagingException"></exception>
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = "Messaging")
    {
        services.Configure<MessagingEndpoint>(configuration.GetSection(sectionName));
        services.PostConfigure<MessagingEndpoint>(opt =>
        {
            if (string.IsNullOrWhiteSpace(opt.ApiBaseUrl))
            {
                throw new MessagingException("Messaging API Configuration : API url is mandatory.");
            }
            if (string.IsNullOrWhiteSpace(opt.ApiKey))
            {
                throw new MessagingException("Messaging API Configuration : API key is mandatory.");
            }
        });

        services.AddHttpClient<ContactsHttpApi>((serviceProvider, client) =>
        {
            var messagingEndpoint = serviceProvider
                .GetRequiredService<IOptions<MessagingEndpoint>>().Value;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", messagingEndpoint.ApiKey);
            client.BaseAddress = new Uri(messagingEndpoint.ApiBaseUrl);
        })
            .AddHttpMessageHandler(() => new UpdateUserAgent());

        services.AddHttpClient<MessagesHttpApi>((serviceProvider, client) =>
        {
            var messagingEndpoint = serviceProvider
                .GetRequiredService<IOptions<MessagingEndpoint>>().Value;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", messagingEndpoint.ApiKey);
            client.BaseAddress = new Uri(messagingEndpoint.ApiBaseUrl);
        })
            .AddHttpMessageHandler(() => new UpdateUserAgent());

        return services;
    }
}
