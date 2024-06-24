using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

const string serviceName = "demo-random";

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
    var httpClient = new HttpClient();
    await httpClient.GetAsync("https://www.google.com");
    return Random.Shared.Next(0, 10);
}
