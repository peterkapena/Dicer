using Microsoft.EntityFrameworkCore.Migrations;

namespace Dicer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    DeviceID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mcAddress = table.Column<string>(nullable: true),
                    deviceTypeName = table.Column<string>(nullable: true),
                    deviceOwnerName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.DeviceID);
                });

            migrationBuilder.CreateTable(
                name: "Gamed",
                columns: table => new
                {
                    gamedID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonID = table.Column<string>(nullable: true),
                    DeviceID = table.Column<string>(nullable: true),
                    DateGamed = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gamed", x => x.gamedID);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    PersonID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    IDNumber = table.Column<string>(nullable: true),
                    PassPortNumber = table.Column<string>(nullable: true),
                    OtherIdentityNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.PersonID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropTable(
                name: "Gamed");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
