using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cribbly.Migrations
{
    public partial class addstandings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Standings",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TeamName = table.Column<string>(nullable: true),
                    Division = table.Column<string>(nullable: true),
                    G1Score = table.Column<int>(nullable: false),
                    G1WinLoss = table.Column<string>(nullable: false),
                    G2Score = table.Column<int>(nullable: false),
                    G2WinLoss = table.Column<string>(nullable: false),
                    G3Score = table.Column<int>(nullable: false),
                    G3WinLoss = table.Column<string>(nullable: false),
                    TotalScore = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Standings", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Standings");
        }
    }
}
