using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cribbly.Migrations
{
    public partial class AddTeamDataModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Game1ID",
                table: "PlayInGameDataModels");

            migrationBuilder.DropColumn(
                name: "Game2ID",
                table: "PlayInGameDataModels");

            migrationBuilder.DropColumn(
                name: "Game3ID",
                table: "PlayInGameDataModels");

            migrationBuilder.CreateTable(
                name: "TeamDataModels",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Player1ID = table.Column<string>(nullable: true),
                    Player2ID = table.Column<string>(nullable: true),
                    PlayInGame1ID = table.Column<int>(nullable: false),
                    PlayInGame2ID = table.Column<int>(nullable: false),
                    PlayInGame3ID = table.Column<int>(nullable: false),
                    DivisionID = table.Column<int>(nullable: true),
                    TournamentSeed = table.Column<int>(nullable: false),
                    TournamentRound = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamDataModels", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TeamDataModels_Divisions_DivisionID",
                        column: x => x.DivisionID,
                        principalTable: "Divisions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamDataModels_DivisionID",
                table: "TeamDataModels",
                column: "DivisionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamDataModels");

            migrationBuilder.AddColumn<int>(
                name: "Game1ID",
                table: "PlayInGameDataModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Game2ID",
                table: "PlayInGameDataModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Game3ID",
                table: "PlayInGameDataModels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
