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
    }
}
