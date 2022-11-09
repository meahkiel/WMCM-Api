using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddingRecurrent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityDetail");

            migrationBuilder.AddColumn<int>(
                name: "Days",
                table: "Activities",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EmailCc",
                table: "Activities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "From",
                table: "Activities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecurrent",
                table: "Activities",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Days",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "EmailCc",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "From",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "IsRecurrent",
                table: "Activities");

            migrationBuilder.CreateTable(
                name: "ActivityDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uuid", nullable: true),
                    Key = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityDetail_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDetail_ActivityId",
                table: "ActivityDetail",
                column: "ActivityId");
        }
    }
}
