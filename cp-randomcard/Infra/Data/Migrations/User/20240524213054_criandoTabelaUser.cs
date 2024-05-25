using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cp_randomcard.Migrations.User
{
    /// <inheritdoc />
    public partial class criandoTabelaUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    USER_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    USER_USERNAME = table.Column<string>(type: "NVARCHAR2(255)", nullable: false),
                    USER_PASSWORD = table.Column<string>(type: "NVARCHAR2(255)", nullable: false),
                    USER_ISACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER", x => x.USER_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "USER");
        }
    }
}
