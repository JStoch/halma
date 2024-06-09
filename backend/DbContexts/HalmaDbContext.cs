using HalmaServer.Models;
using HalmaWebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HalmaWebApi.DbContexts
{
    public class HalmaDbContext : DbContext
    {
        public HalmaDbContext(DbContextOptions<HalmaDbContext> options) : base(options)
        {
        }


        public DbSet<GameModel> Games { get; set; }  
        
        public DbSet<PiecePositionModel> PiecePositionModels { get; set; }
        public DbSet<PlayerModel> PlayerModels { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<GameHistory> GamesHistory { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure GameModel entity
            modelBuilder.Entity<GameModel>()
                .HasKey(g => g.GameGuid);

            // Configure GamesHistoryModel entity
            modelBuilder.Entity<GameHistory>()
                        .HasKey(gh => gh.GameHistoryGuid);


            modelBuilder.Entity<GameModel>()
                        .HasOne(g => g.Player1)
                        .WithMany()
                        .HasForeignKey(g => g.Player1Guid)
                        .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<GameModel>()
                .HasOne(g => g.Player2)
                .WithMany()
                .HasForeignKey(g => g.Player2Guid)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Statistic>()
               .HasOne(p => p.Owner)
               .WithMany()
               .HasForeignKey(p => p.PlayerGuid);

            modelBuilder.Entity<PiecePositionModel>()
                .HasOne(p => p.Owner)
                .WithMany()
                .HasForeignKey(p => p.OwnerGuid);

            base.OnModelCreating(modelBuilder);
        }
    }
}
