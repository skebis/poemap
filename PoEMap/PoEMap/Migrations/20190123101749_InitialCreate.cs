using Microsoft.EntityFrameworkCore.Migrations;

namespace PoEMap.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stashes",
                columns: table => new
                {
                    StashId = table.Column<string>(nullable: false),
                    Seller = table.Column<string>(nullable: true),
                    StashName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stashes", x => x.StashId);
                });

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    MapId = table.Column<string>(nullable: false),
                    StashId = table.Column<string>(nullable: true),
                    MapName = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true),
                    League = table.Column<string>(nullable: true),
                    IconAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.MapId);
                    table.ForeignKey(
                        name: "FK_Maps_Stashes_StashId",
                        column: x => x.StashId,
                        principalTable: "Stashes",
                        principalColumn: "StashId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Maps_StashId",
                table: "Maps",
                column: "StashId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "Stashes");
        }
    }
}
