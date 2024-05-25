using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cp_randomcard.Migrations
{
    /// <inheritdoc />
    public partial class CreatingCardTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CARD",
                columns: table => new
                {
                    CARD_ID = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CARD_TITLE = table.Column<string>(type: "NVARCHAR2(255)", nullable: false),
                    CARD_ATRIBUTE = table.Column<string>(type: "NVARCHAR2(255)", nullable: false),
                    CARD_POWER = table.Column<int>(type: "INTEGER", nullable: false),
                    CARD_HEALTH = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CARD", x => x.CARD_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CARD");
        }
    }
}
