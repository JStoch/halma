using backend.Repositories;
using backend.Services;
using HalmaServer.Hubs;
using HalmaServer.Models;
using HalmaServer.Services;
using HalmaWebApi.DbContexts;
using HalmaWebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Repositories of models to be created by fabric
            Type[] repositoryTypes =
            {
                typeof(GameHistory),
                typeof(GameModel),
                typeof(Statistic),
                typeof(PlayerModel),
                typeof(PiecePositionModel)
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = "https://accounts.google.com";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = Configuration["Authentication:Google:ClientId"]
                };
            })
            .AddGoogle(options =>
            {
                options.ClientId = Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                options.CallbackPath = "/signin-google";
            });

            var halmaDbConnString = Configuration.GetConnectionString("HalmaDbConn");
            services.AddDbContext<HalmaDbContext>(opt =>
            {
                opt.UseSqlServer(halmaDbConnString);
                opt.EnableDetailedErrors();
            });

            // TODO: Adding SSO support via Identity or any other available solution:
            // var identityServerDbConnString = Configuration.GetConnectionString("IdentityServerDbConn");

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddSignalR();
            services.AddScoped<GameRepository>();
            services.AddScoped<GameService>();
            services.AddScoped<BotDispacherService>();

            // Added repo fabric for managing repository existence
            services.AddScoped(provider =>
            {
                var repositoryFabric = new RepositoryFactory<HalmaDbContext>(provider.GetRequiredService<HalmaDbContext>());
                repositoryFabric.CreateAsyncRepositories(repositoryTypes);
                return repositoryFabric;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => 
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Halma API V1");
                    c.RoutePrefix = "swagger"; // Set Swagger at the "swagger" endpoint
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<GameHub>("/game");
            });
        }
    }
}
