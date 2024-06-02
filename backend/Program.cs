using HalmaServer.Hubs;
using HalmaServer.Services;
using Microsoft.AspNetCore.SignalR;
using HalmaWebApi.DbContexts;
using HalmaWebApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
internal class Program
{


    
        
    private static void Main(string[] args)
    {
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

        builder.Services.AddControllers();

       

        var halmaDbConnString = builder.Configuration.GetConnectionString("HalmaDbConn");
        builder.Services.AddDbContext<HalmaDbContext>(opt => { opt.UseSqlServer(halmaDbConnString); opt.EnableDetailedErrors(); }) ;

        //var identityServerDbConnString = builder.Configuration.GetConnectionString("IdentityServerDbConn");
        


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddSingleton<GameRepository>();
builder.Services.AddSingleton<GameService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapHub<GameHub>("/game");
        app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
