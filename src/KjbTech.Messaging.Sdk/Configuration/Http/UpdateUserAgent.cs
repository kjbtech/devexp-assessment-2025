using System.Globalization;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;

namespace KjbTech.Messaging.Sdk;

internal class UpdateUserAgent : DelegatingHandler
{
    private readonly ProductInfoHeaderValue _sdkProduct;
    private readonly string _comment;

    public UpdateUserAgent()
    {
        var name = "KjbTech.Messaging.Sdk";

        var version = GetInformationalVersion() ?? GetAssemblyVersion() ?? "0.0.0";

        _sdkProduct = new ProductInfoHeaderValue(name, version);

        var framework = RuntimeInformation.FrameworkDescription; // ".NET 8.0.x"
        var os = RuntimeInformation.OSDescription;               // "Windows 10.0.22631"
        var osArch = RuntimeInformation.OSArchitecture.ToString();
        var culture = CultureInfo.CurrentUICulture.Name;         // "fr-FR", etc.

        _comment = $"({framework}; {os}; {osArch}; {culture})";
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var existingUserAgents = request.Headers.UserAgent;

        if (!existingUserAgents.Any(p => p.Product?.Name == _sdkProduct.Product?.Name))
        {
            existingUserAgents.Add(_sdkProduct);
            existingUserAgents.Add(new ProductInfoHeaderValue(_comment));
        }

        return base.SendAsync(request, ct);
    }

    private static string? GetInformationalVersion() =>
        typeof(UpdateUserAgent).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;

    private static string? GetAssemblyVersion() =>
        typeof(UpdateUserAgent).Assembly.GetName().Version?.ToString();
}
