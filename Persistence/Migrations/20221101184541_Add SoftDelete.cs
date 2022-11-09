using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddSoftDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InActive",
                table: "SubTask",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "InActive",
                table: "MarketingTasks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "InActive",
                table: "Contacts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "InActive",
                table: "Comment",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "InActive",
                table: "Campaigns",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "InActive",
                table: "Activities",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InActive",
                table: "SubTask");

            migrationBuilder.DropColumn(
                name: "InActive",
                table: "MarketingTasks");

            migrationBuilder.DropColumn(
                name: "InActive",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "InActive",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "InActive",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "InActive",
                table: "Activities");
        }
    }
}
