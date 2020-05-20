using Microsoft.EntityFrameworkCore.Migrations;

namespace Cribbly.Migrations
{
    public partial class UseValueConversion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Team1Score",
                table: "PlayInGame");

            migrationBuilder.DropColumn(
                name: "Team2Score",
                table: "PlayInGame");

            migrationBuilder.AddColumn<string>(
                name: "Scores",
                table: "PlayInGame",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Scores",
                table: "PlayInGame");

            migrationBuilder.AddColumn<int>(
                name: "Team1Score",
                table: "PlayInGame",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Team2Score",
                table: "PlayInGame",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
