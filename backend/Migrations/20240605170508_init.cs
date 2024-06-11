using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    StatisticGuid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GamesPlayed = table.Column<int>(type: "int", nullable: false),
                    GamesWon = table.Column<int>(type: "int", nullable: false),
                    AvgWinRate = table.Column<double>(type: "float", nullable: false),
                    LastPlayedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => x.StatisticGuid);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsLoggedIn = table.Column<bool>(type: "bit", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlayerModels",
                columns: table => new
                {
                    PlayerGuid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConnectionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserGuid = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StatisticGuid = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerModels", x => x.PlayerGuid);
                    table.ForeignKey(
                        name: "FK_PlayerModels_Statistics_StatisticGuid",
                        column: x => x.StatisticGuid,
                        principalTable: "Statistics",
                        principalColumn: "StatisticGuid");
                    table.ForeignKey(
                        name: "FK_PlayerModels_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameGuid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Player1Guid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Player2Guid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsGameActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameGuid);
                    table.ForeignKey(
                        name: "FK_Games_PlayerModels_Player1Guid",
                        column: x => x.Player1Guid,
                        principalTable: "PlayerModels",
                        principalColumn: "PlayerGuid");
                    table.ForeignKey(
                        name: "FK_Games_PlayerModels_Player2Guid",
                        column: x => x.Player2Guid,
                        principalTable: "PlayerModels",
                        principalColumn: "PlayerGuid");
                });

            migrationBuilder.CreateTable(
                name: "GamesHistory",
                columns: table => new
                {
                    GameHistoryGuid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GameModelGuid = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamesHistory", x => x.GameHistoryGuid);
                    table.ForeignKey(
                        name: "FK_GamesHistory_Games_GameModelGuid",
                        column: x => x.GameModelGuid,
                        principalTable: "Games",
                        principalColumn: "GameGuid");
                });

            migrationBuilder.CreateTable(
                name: "PiecePositionModels",
                columns: table => new
                {
                    PieceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    GameGuid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OwnerGuid = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PiecePositionModels", x => x.PieceId);
                    table.ForeignKey(
                        name: "FK_PiecePositionModels_Games_GameGuid",
                        column: x => x.GameGuid,
                        principalTable: "Games",
                        principalColumn: "GameGuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PiecePositionModels_PlayerModels_OwnerGuid",
                        column: x => x.OwnerGuid,
                        principalTable: "PlayerModels",
                        principalColumn: "PlayerGuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_Player1Guid",
                table: "Games",
                column: "Player1Guid");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Player2Guid",
                table: "Games",
                column: "Player2Guid");

            migrationBuilder.CreateIndex(
                name: "IX_GamesHistory_GameModelGuid",
                table: "GamesHistory",
                column: "GameModelGuid");

            migrationBuilder.CreateIndex(
                name: "IX_PiecePositionModels_GameGuid",
                table: "PiecePositionModels",
                column: "GameGuid");

            migrationBuilder.CreateIndex(
                name: "IX_PiecePositionModels_OwnerGuid",
                table: "PiecePositionModels",
                column: "OwnerGuid");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerModels_StatisticGuid",
                table: "PlayerModels",
                column: "StatisticGuid");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerModels_UserGuid",
                table: "PlayerModels",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamesHistory");

            migrationBuilder.DropTable(
                name: "PiecePositionModels");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "PlayerModels");

            migrationBuilder.DropTable(
                name: "Statistics");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
