using System.Diagnostics;

namespace KjbTech.Messaging.Sdk.Configuration.Http;

/// <summary>
/// Support correlation ID and/or trace id.
/// if missing, we add a new one.
/// </summary>
internal sealed class AddObservability : DelegatingHandler
{
    private static readonly ActivitySource ActivitySource = new(typeof(AddObservability).Assembly.FullName ?? "KjbTech.Messaging.Sdk");

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var correlationHeaderName = "X-Correlation-ID";
        if (!request.Headers.Contains(correlationHeaderName))
        {
            var id = Guid.NewGuid().ToString("D").ToLowerInvariant();

            request.Headers.TryAddWithoutValidation(correlationHeaderName, id);
        }

        // If a trace is already active, we just create a child activity.
        // If not, we start a fresh client Activity so HttpClient injects traceparent/tracestate.
        using var activity = ActivitySource.StartActivity(
                name: $"HTTP {request.Method}",
                kind: ActivityKind.Client);

        if (activity is not null)
        {
            // Helpful tags for backends (OpenTelemetry, Jaeger, Zipkin, etc.)
            activity.SetTag("http.method", request.Method.Method);
            activity.SetTag("http.url", request.RequestUri?.ToString());
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (activity is not null)
        {
            activity.SetTag("http.status_code", (int)response.StatusCode);
            if (response.Headers.TryGetValues(correlationHeaderName, out var values))
            {
                foreach (var v in values)
                {
                    activity.SetTag("correlation.id", v);
                }
            }
        }

        return response;
    }
}
