using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class updateonshadowproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdateOn",
                table: "MarketingTasks",
                newName: "LastUpdatedOn");

            migrationBuilder.RenameColumn(
                name: "LastUpdateOn",
                table: "Comment",
                newName: "LastUpdatedOn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdatedOn",
                table: "MarketingTasks",
                newName: "LastUpdateOn");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedOn",
                table: "Comment",
                newName: "LastUpdateOn");
        }
    }
}
