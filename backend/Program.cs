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
        builder.Services.AddDbContext<HalmaDbContext>(opt => { opt.UseSqlServer(halmaDbConnString); opt.EnableDetailedErrors(); });

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



        app.MapHub<GameHub>("/game");
        app.MapControllers();

        app.Run();

    }
}
