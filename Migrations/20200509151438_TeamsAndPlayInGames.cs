using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cribbly.Migrations
{
    public partial class TeamsAndPlayInGames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayInGame",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Team1Score = table.Column<int>(nullable: false),
                    Team2Score = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayInGame", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TeamPlayInGame",
                columns: table => new
                {
                    TeamID = table.Column<int>(nullable: false),
                    PlayInGameID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamPlayInGame", x => new { x.TeamID, x.PlayInGameID });
                    table.ForeignKey(
                        name: "FK_TeamPlayInGame_PlayInGame_PlayInGameID",
                        column: x => x.PlayInGameID,
                        principalTable: "PlayInGame",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamPlayInGame_Teams_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamPlayInGame_PlayInGameID",
                table: "TeamPlayInGame",
                column: "PlayInGameID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamPlayInGame");

            migrationBuilder.DropTable(
                name: "PlayInGame");
        }
    }
}
