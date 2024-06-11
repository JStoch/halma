using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class change_ref_Statistic_PlayerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerModels_Statistics_StatisticGuid",
                table: "PlayerModels");

            migrationBuilder.DropIndex(
                name: "IX_PlayerModels_StatisticGuid",
                table: "PlayerModels");

            migrationBuilder.DropColumn(
                name: "StatisticGuid",
                table: "PlayerModels");

            migrationBuilder.AddColumn<string>(
                name: "PlayerGuid",
                table: "Statistics",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_PlayerGuid",
                table: "Statistics",
                column: "PlayerGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Statistics_PlayerModels_PlayerGuid",
                table: "Statistics",
                column: "PlayerGuid",
                principalTable: "PlayerModels",
                principalColumn: "PlayerGuid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Statistics_PlayerModels_PlayerGuid",
                table: "Statistics");

            migrationBuilder.DropIndex(
                name: "IX_Statistics_PlayerGuid",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "PlayerGuid",
                table: "Statistics");

            migrationBuilder.AddColumn<string>(
                name: "StatisticGuid",
                table: "PlayerModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerModels_StatisticGuid",
                table: "PlayerModels",
                column: "StatisticGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerModels_Statistics_StatisticGuid",
                table: "PlayerModels",
                column: "StatisticGuid",
                principalTable: "Statistics",
                principalColumn: "StatisticGuid");
        }
    }
}
