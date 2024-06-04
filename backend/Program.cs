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
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        //repositories of models to be created by fabric:
        Type[] repositoryTypes =
        [
            typeof(GameHistory), 
            typeof(GameModel), 
            typeof(Statistic), 
            typeof(PlayerModel),
            typeof(PiecePositionModel)
        ];

        var halmaDbConnString = builder.Configuration.GetConnectionString("HalmaDbConn");
        builder.Services.AddDbContext<HalmaDbContext>(opt => { opt.UseSqlServer(halmaDbConnString); opt.EnableDetailedErrors(); });

        // TODO: Adding SSO support via Identity or any other available solution:
        //var identityServerDbConnString = builder.Configuration.GetConnectionString("IdentityServerDbConn");


        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSignalR();
        builder.Services.AddScoped<GameRepository>();
        builder.Services.AddScoped<GameService>();
        
        //Added repo fabric for managing repository existance:
        builder.Services.AddScoped(provider =>
            {
                var repositoryFabric = new RepositoryFactory<HalmaDbContext>(provider.GetRequiredService<HalmaDbContext>());
                repositoryFabric.CreateAsyncRepositories(repositoryTypes);
                return repositoryFabric;
            });

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
