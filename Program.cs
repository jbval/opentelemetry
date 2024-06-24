using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

const string serviceName = "demo-random";
ActivitySource source = new ActivitySource(serviceName, "1.0.0");


builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName))
        .AddConsoleExporter();
});
builder.Services.AddOpenTelemetry()
      .ConfigureResource(resource => resource.AddService(serviceName))
      .WithTracing(tracing => tracing
          .AddAspNetCoreInstrumentation()
          .AddHttpClientInstrumentation()
          .AddConsoleExporter()
          .AddOtlpExporter()
          .AddSource(serviceName)
          )


      .WithMetrics(metrics => metrics
          .AddAspNetCoreInstrumentation()
          .AddHttpClientInstrumentation()
          .AddConsoleExporter());


var app = builder.Build();

app.MapGet("/", GetRandomNumberAsync);

app.Run();

async Task<int> GetRandomNumberAsync([FromServices] ILogger<Program> logger)
{
    logger.LogInformation("Random");

    var random = await GenerateAsync();
    logger.LogInformation("End Random", random);
    return random;

}

async Task<int> GenerateAsync()
{
    using (Activity activity = source.StartActivity("Generate"))
    {
        var httpClient = new HttpClient();
        await httpClient.GetAsync("https://www.google.com");
        return Random.Shared.Next(0, 10);
    }
}
