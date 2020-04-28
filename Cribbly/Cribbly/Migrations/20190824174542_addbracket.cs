using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cribbly.Migrations
{
    public partial class addbracket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BracketId",
                table: "Standings",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Bracket",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsSeeded = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bracket", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Standings_BracketId",
                table: "Standings",
                column: "BracketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Standings_Bracket_BracketId",
                table: "Standings",
                column: "BracketId",
                principalTable: "Bracket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Standings_Bracket_BracketId",
                table: "Standings");

            migrationBuilder.DropTable(
                name: "Bracket");

            migrationBuilder.DropIndex(
                name: "IX_Standings_BracketId",
                table: "Standings");

            migrationBuilder.DropColumn(
                name: "BracketId",
                table: "Standings");
        }
    }
}
