using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Etl
{
    /// <inheritdoc />
    public partial class ChangeWorkflowDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TableId",
                table: "Workflows",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workflows_TableId",
                table: "Workflows",
                column: "TableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Workflows_TableId",
                table: "Workflows");

            migrationBuilder.DropColumn(
                name: "TableId",
                table: "Workflows");
        }
    }
}
