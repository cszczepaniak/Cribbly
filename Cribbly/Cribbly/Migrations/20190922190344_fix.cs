using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cribbly.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateMade",
                table: "PlayInGames");

            migrationBuilder.DropColumn(
                name: "SubmittedBy",
                table: "PlayInGames");

            migrationBuilder.RenameColumn(
                name: "LastEdited",
                table: "PlayInGames",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "EditedBy",
                table: "PlayInGames",
                newName: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "PlayInGames",
                newName: "EditedBy");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "PlayInGames",
                newName: "LastEdited");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateMade",
                table: "PlayInGames",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SubmittedBy",
                table: "PlayInGames",
                nullable: false,
                defaultValue: 0);
        }
    }
}
