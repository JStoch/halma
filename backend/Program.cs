using HalmaServer.Hubs;
using HalmaServer.Services;
using Microsoft.AspNetCore.SignalR;
using HalmaWebApi.DbContexts;
using HalmaWebApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using backend.Repositories;
using static NuGet.Protocol.Core.Types.Repository;
using System.Drawing.Text;
using HalmaServer.Models;
using Microsoft.AspNetCore.Hosting;
using backend;
using Microsoft.AspNetCore.Connections;
using backend.Controllers;
internal class Program
{

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(serverOptions =>
                {
                    serverOptions.ListenAnyIP(5000, listenOptions =>
                    {
                        //listenOptions.UseConnectionHandler<?>()
                        //listenOptions.UseHttps(); // Use HTTPS for port 5000
                    });
                    serverOptions.ListenAnyIP(8080, listenOptions =>
                    {
                        listenOptions.UseHub<GameHub>();
                       // listenOptions.UseHttps(); // Use HTTPS for port 8080
                    });
                    serverOptions.ListenAnyIP(8081, listenOptions =>
                    {
                        listenOptions.UseHttps(); // Use HTTPS for Swagger on port 49157
                    });
                });
                webBuilder.UseStartup<Startup>();
            });
    private static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
        //var builder = WebApplication.CreateBuilder(args);

        //// Add services to the container.

        //builder.Services.AddControllers();

        

        ////repositories of models to be created by fabric:
        //Type[] repositoryTypes =
        //[
        //    typeof(GameHistory), 
        //    typeof(GameModel), 
        //    typeof(Statistic), 
        //    typeof(PlayerModel),
        //    typeof(PiecePositionModel)
        //];

        //var halmaDbConnString = builder.Configuration.GetConnectionString("HalmaDbConn");
        //builder.Services.AddDbContext<HalmaDbContext>(opt => { opt.UseSqlServer(halmaDbConnString); opt.EnableDetailedErrors(); });

        //// TODO: Adding SSO support via Identity or any other available solution:
        ////var identityServerDbConnString = builder.Configuration.GetConnectionString("IdentityServerDbConn");


        //// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        //builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();
        //builder.Services.AddSignalR();
        //builder.Services.AddScoped<GameRepository>();
        //builder.Services.AddScoped<GameService>();
        
        ////Added repo fabric for managing repository existance:
        //builder.Services.AddScoped(provider =>
        //    {
        //        var repositoryFabric = new RepositoryFactory<HalmaDbContext>(provider.GetRequiredService<HalmaDbContext>());
        //        repositoryFabric.CreateAsyncRepositories(repositoryTypes);
        //        return repositoryFabric;
        //    });

        //var app = builder.Build();

        //// Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        //    app.UseSwagger();
        //    app.UseSwaggerUI();
        //}

        //app.UseHttpsRedirection();



        //app.MapHub<GameHub>("/game");
        //app.MapControllers();

        //app.Run();

    }
}
