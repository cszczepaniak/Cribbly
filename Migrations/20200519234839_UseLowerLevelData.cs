using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cribbly.Migrations
{
    public partial class UseLowerLevelData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayInGameDataModels",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Game1ID = table.Column<int>(nullable: false),
                    Game2ID = table.Column<int>(nullable: false),
                    Game3ID = table.Column<int>(nullable: false),
                    Team1ID = table.Column<int>(nullable: false),
                    Team2ID = table.Column<int>(nullable: false),
                    Team1Score = table.Column<int>(nullable: false),
                    Team2Score = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayInGameDataModels", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayInGameDataModels");
        }
    }
}
