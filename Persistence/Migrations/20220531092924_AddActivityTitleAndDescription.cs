using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddActivityTitleAndDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityDetail_Activities_ActivityId1",
                table: "ActivityDetail");

            migrationBuilder.DropIndex(
                name: "IX_ActivityDetail_ActivityId1",
                table: "ActivityDetail");

            migrationBuilder.DropColumn(
                name: "ActivityId1",
                table: "ActivityDetail");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Activities",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Activities",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Activities");

            migrationBuilder.AddColumn<Guid>(
                name: "ActivityId1",
                table: "ActivityDetail",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDetail_ActivityId1",
                table: "ActivityDetail",
                column: "ActivityId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityDetail_Activities_ActivityId1",
                table: "ActivityDetail",
                column: "ActivityId1",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
