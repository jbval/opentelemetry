# opentelemetry

Add OpenTelemetry :

```shell
dotnet add package OpenTelemetry.Extensions.Hosting 
dotnet add package OpenTelemetry.Instrumentation.AspNetCore
dotnet add package OpenTelemetry.Exporter.Console
```



Add OTLP package :

```shell 
dotnet add package OpenTelemetry.Exporter.OpenTelemetryProtocol
```

Start Jaeger for demo : 
```shell
docker run --rm -e COLLECTOR_ZIPKIN_HOST_PORT=:9411  -p 16686:16686 -p 4317:4317 -p 4318:4318 -p 9411:9411  jaegertracing/all-in-one:latest
```