using Microsoft.EntityFrameworkCore.Migrations;

namespace SandwichClub.Api.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    AvatarUrl = table.Column<string>(nullable: true),
                    BankDetails = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FacebookId = table.Column<string>(nullable: true),
                    FirstLogin = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Shopper = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Weeks",
                columns: table => new
                {
                    WeekId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Cost = table.Column<double>(nullable: false),
                    ShopperUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weeks", x => x.WeekId);
                });

            migrationBuilder.CreateTable(
                name: "WeekUserLinks",
                columns: table => new
                {
                    WeekId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Paid = table.Column<double>(nullable: false),
                    Slices = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekUserLinks", x => new { x.WeekId, x.UserId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Weeks");

            migrationBuilder.DropTable(
                name: "WeekUserLinks");
        }
    }
}
