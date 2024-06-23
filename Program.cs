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
          .AddConsoleExporter()
          .AddOtlpExporter()
          )


      .WithMetrics(metrics => metrics
          .AddAspNetCoreInstrumentation()
          .AddConsoleExporter());


var app = builder.Build();

app.MapGet("/",  GetRandomNumber);

app.Run();

int GetRandomNumber([FromServices] ILogger<Program> logger){
    logger.LogInformation("Random");

    var random = Generate();
    logger.LogInformation("End Random", random);
    return random; 
    
}

int Generate(){
return Random.Shared.Next(0,10);
}
