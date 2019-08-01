using Microsoft.EntityFrameworkCore.Migrations;

namespace Cribbly.Migrations
{
    public partial class removediv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Division",
                table: "Teams");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Division",
                table: "Teams",
                nullable: false,
                defaultValue: "");
        }
    }
}
