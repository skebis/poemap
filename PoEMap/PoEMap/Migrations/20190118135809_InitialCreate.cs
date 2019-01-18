using Microsoft.EntityFrameworkCore.Migrations;

namespace PoEMap.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StashesDb",
                columns: table => new
                {
                    StashId = table.Column<int>(nullable: false),
                    Seller = table.Column<string>(nullable: true),
                    StashName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StashesDb", x => x.StashId);
                });

            migrationBuilder.CreateTable(
                name: "MapsDb",
                columns: table => new
                {
                    MapId = table.Column<int>(nullable: false),
                    StashId = table.Column<int>(nullable: false),
                    MapName = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true),
                    League = table.Column<string>(nullable: true),
                    IconAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapsDb", x => x.MapId);
                    table.ForeignKey(
                        name: "FK_MapsDb_StashesDb_StashId",
                        column: x => x.StashId,
                        principalTable: "StashesDb",
                        principalColumn: "StashId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapsDb_StashId",
                table: "MapsDb",
                column: "StashId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapsDb");

            migrationBuilder.DropTable(
                name: "StashesDb");
        }
    }
}
