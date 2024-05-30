using HalmaWebApi.DbContexts;
using HalmaWebApi.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
internal class Program
{
    //Testing other users - connection still failes: 

    private static string IdentityServerConnectionString = @"Server=localhost,1443;Database=IdentityServerDb;User ID=IdentityServerDbUser;Password=P@ssw0rdIdentity;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

    //@"Server=localhost,1443;Database=HalmaDb;User ID=HalmaDbUser;Password=P@ssw0rdHalma;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    private static string HalmaDbConnectionString = @"Server=localhost,1443;Database=IdentityServerDb;User ID=SA;Password=P@ssw0rdSql;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    
    
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        List<TestUser> testUsers = new List<TestUser>() {
            new TestUser() 
            {
               SubjectId = "1",
               Username = "Test1",
               Password = "1234"
            }, 
            new TestUser() 
            {
                SubjectId = "2",
               Username = "Test2",
               Password = "2234"
            } 
        };


        //TODO Add EF sqlServer for identity and halma game 
        builder.Services.AddDbContext<HalmaDbContext>(opt => opt.UseSqlServer(HalmaDbConnectionString));

        builder.Services.AddIdentityServer().AddConfigurationStore(options => 
        {
            options.ConfigureDbContext = dbContext => dbContext.UseSqlServer(IdentityServerConnectionString);

        })
        .AddOperationalStore(options => 
        {
            options.ConfigureDbContext = dbContext => dbContext.UseSqlServer(IdentityServerConnectionString);
        }).
        AddTestUsers(testUsers);


        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseIdentityServer();


        app.MapControllers();

        app.Run();
    }
}