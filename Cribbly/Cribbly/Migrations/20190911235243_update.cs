using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cribbly.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Divisions_DivisionId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_DivisionId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "DivisionId",
                table: "Teams");

            migrationBuilder.CreateTable(
                name: "PlayInGames",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Team1Id = table.Column<int>(nullable: false),
                    Team2Id = table.Column<int>(nullable: false),
                    Team1TotalScore = table.Column<int>(nullable: false),
                    Team2TotalScore = table.Column<int>(nullable: false),
                    WinningTeamId = table.Column<int>(nullable: false),
                    ScoreDifference = table.Column<int>(nullable: false),
                    GameNumber = table.Column<int>(nullable: false),
                    Team1Name = table.Column<string>(nullable: true),
                    Team2Name = table.Column<string>(nullable: true),
                    WinningTeamName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayInGames", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayInGames");

            migrationBuilder.AddColumn<int>(
                name: "DivisionId",
                table: "Teams",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_DivisionId",
                table: "Teams",
                column: "DivisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Divisions_DivisionId",
                table: "Teams",
                column: "DivisionId",
                principalTable: "Divisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
