﻿// <auto-generated />
using System;
using HalmaWebApi.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace backend.Migrations
{
    [DbContext(typeof(HalmaDbContext))]
    partial class HalmaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HalmaServer.Models.GameModel", b =>
                {
                    b.Property<string>("GameGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsGameActive")
                        .HasColumnType("bit");

                    b.Property<string>("Player1Guid")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Player2Guid")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("GameGuid");

                    b.HasIndex("Player1Guid");

                    b.HasIndex("Player2Guid");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("HalmaServer.Models.PiecePositionModel", b =>
                {
                    b.Property<string>("PieceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GameGuid")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PlayerGuid")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("X")
                        .HasColumnType("int");

                    b.Property<int>("Y")
                        .HasColumnType("int");

                    b.HasKey("PieceId");

                    b.HasIndex("GameGuid");

                    b.HasIndex("PlayerGuid");

                    b.ToTable("PiecePositionModel");
                });

            modelBuilder.Entity("HalmaServer.Models.PlayerModel", b =>
                {
                    b.Property<string>("PlayerGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConnectionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StatisticGuid")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserGuid")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PlayerGuid");

                    b.HasIndex("StatisticGuid");

                    b.HasIndex("UserGuid");

                    b.ToTable("PlayerModels");
                });

            modelBuilder.Entity("HalmaWebApi.Models.GameHistory", b =>
                {
                    b.Property<string>("GameHistoryGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GameModelGuid")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("GameHistoryGuid");

                    b.HasIndex("GameModelGuid");

                    b.ToTable("GamesHistory");
                });

            modelBuilder.Entity("HalmaWebApi.Models.Statistic", b =>
                {
                    b.Property<string>("StatisticGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("AvgWinRate")
                        .HasColumnType("float");

                    b.Property<int>("GamesPlayed")
                        .HasColumnType("int");

                    b.Property<int>("GamesWon")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastPlayedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("StatisticGuid");

                    b.ToTable("Statistics");
                });

            modelBuilder.Entity("HalmaWebApi.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLoggedIn")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HalmaServer.Models.GameModel", b =>
                {
                    b.HasOne("HalmaServer.Models.PlayerModel", "Player1")
                        .WithMany()
                        .HasForeignKey("Player1Guid")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("HalmaServer.Models.PlayerModel", "Player2")
                        .WithMany()
                        .HasForeignKey("Player2Guid")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Player1");

                    b.Navigation("Player2");
                });

            modelBuilder.Entity("HalmaServer.Models.PiecePositionModel", b =>
                {
                    b.HasOne("HalmaServer.Models.GameModel", "Game")
                        .WithMany("Pieces")
                        .HasForeignKey("GameGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HalmaServer.Models.PlayerModel", "Owner")
                        .WithMany()
                        .HasForeignKey("PlayerGuid");

                    b.Navigation("Game");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("HalmaServer.Models.PlayerModel", b =>
                {
                    b.HasOne("HalmaWebApi.Models.Statistic", "Statistic")
                        .WithMany()
                        .HasForeignKey("StatisticGuid");

                    b.HasOne("HalmaWebApi.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserGuid");

                    b.Navigation("Statistic");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HalmaWebApi.Models.GameHistory", b =>
                {
                    b.HasOne("HalmaServer.Models.GameModel", "GameModel")
                        .WithMany()
                        .HasForeignKey("GameModelGuid");

                    b.Navigation("GameModel");
                });

            modelBuilder.Entity("HalmaWebApi.Models.User", b =>
                {
                    b.HasOne("HalmaWebApi.Models.User", null)
                        .WithMany("FriendsList")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("HalmaServer.Models.GameModel", b =>
                {
                    b.Navigation("Pieces");
                });

            modelBuilder.Entity("HalmaWebApi.Models.User", b =>
                {
                    b.Navigation("FriendsList");
                });
#pragma warning restore 612, 618
        }
    }
}
