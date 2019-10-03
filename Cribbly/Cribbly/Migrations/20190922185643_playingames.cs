using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cribbly.Migrations
{
    public partial class playingames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateMade",
                table: "PlayInGames",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "PlayInGames",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEdited",
                table: "PlayInGames",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SubmittedBy",
                table: "PlayInGames",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateMade",
                table: "PlayInGames");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "PlayInGames");

            migrationBuilder.DropColumn(
                name: "LastEdited",
                table: "PlayInGames");

            migrationBuilder.DropColumn(
                name: "SubmittedBy",
                table: "PlayInGames");
        }
    }
}
