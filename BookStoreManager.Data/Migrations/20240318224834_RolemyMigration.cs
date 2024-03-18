using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreManager.Data.Migrations
{
    public partial class RolemyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Authors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Authors");
        }
    }
}
