using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/",  GetRandomNumber);

app.Run();

int GetRandomNumber([FromServices] ILogger<Program> logger){
    logger.LogInformation("Random");

    var random=Random.Shared.Next(0,10);
    logger.LogInformation("End Random", random);
    return random; 
    
}
