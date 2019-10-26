using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cribbly.Migrations
{
    public partial class a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    WinningTeamName = table.Column<string>(nullable: true),
                    Division = table.Column<string>(nullable: true),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    Team3Id = table.Column<int>(nullable: true),
                    Team3Name = table.Column<string>(nullable: true),
                    Team3TotalScore = table.Column<int>(nullable: true)
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

        }
    }
}
