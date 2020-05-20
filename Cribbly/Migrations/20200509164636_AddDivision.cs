using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cribbly.Migrations
{
    public partial class AddDivision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamPlayInGame_PlayInGame_PlayInGameID",
                table: "TeamPlayInGame");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayInGame",
                table: "PlayInGame");

            migrationBuilder.RenameTable(
                name: "PlayInGame",
                newName: "PlayInGames");

            migrationBuilder.AddColumn<int>(
                name: "DivisionID",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayInGames",
                table: "PlayInGames",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "Divisions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divisions", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_DivisionID",
                table: "Teams",
                column: "DivisionID");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPlayInGame_PlayInGames_PlayInGameID",
                table: "TeamPlayInGame",
                column: "PlayInGameID",
                principalTable: "PlayInGames",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Divisions_DivisionID",
                table: "Teams",
                column: "DivisionID",
                principalTable: "Divisions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamPlayInGame_PlayInGames_PlayInGameID",
                table: "TeamPlayInGame");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Divisions_DivisionID",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "Divisions");

            migrationBuilder.DropIndex(
                name: "IX_Teams_DivisionID",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayInGames",
                table: "PlayInGames");

            migrationBuilder.DropColumn(
                name: "DivisionID",
                table: "Teams");

            migrationBuilder.RenameTable(
                name: "PlayInGames",
                newName: "PlayInGame");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayInGame",
                table: "PlayInGame",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPlayInGame_PlayInGame_PlayInGameID",
                table: "TeamPlayInGame",
                column: "PlayInGameID",
                principalTable: "PlayInGame",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
