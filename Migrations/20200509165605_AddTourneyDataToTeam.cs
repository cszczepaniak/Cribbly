using Microsoft.EntityFrameworkCore.Migrations;

namespace Cribbly.Migrations
{
    public partial class AddTourneyDataToTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TournamentRound",
                table: "Teams",
                nullable: false,
                defaultValue: -1);

            migrationBuilder.AddColumn<int>(
                name: "TournamentSeed",
                table: "Teams",
                nullable: false,
                defaultValue: -1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TournamentRound",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TournamentSeed",
                table: "Teams");
        }
    }
}
