using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSC.Infrastructure.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class FixRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Roles_Descripcion",
                table: "Roles",
                column: "Descripcion",
                unique: true,
                filter: "[Descripcion] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Roles_Descripcion",
                table: "Roles");
        }
    }
}
