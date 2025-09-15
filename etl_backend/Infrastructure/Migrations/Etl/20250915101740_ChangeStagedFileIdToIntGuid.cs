using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.Etl
{
    /// <inheritdoc />
    public partial class ChangeStagedFileIdToIntGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TempGuidId",
                table: "staged_files",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()");

            migrationBuilder.DropPrimaryKey(
                name: "PK_staged_files",
                table: "staged_files");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "staged_files");

            migrationBuilder.RenameColumn(
                name: "TempGuidId",
                table: "staged_files",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_staged_files",
                table: "staged_files",
                column: "Id");

            // ➤ DROP before CREATE
            migrationBuilder.Sql("DROP INDEX IF EXISTS \"IX_staged_files_StoredFilePath\"");
            migrationBuilder.Sql("DROP INDEX IF EXISTS \"IX_staged_files_Stage_Status\"");
            migrationBuilder.Sql("DROP INDEX IF EXISTS \"IX_staged_files_UploadedAt\"");

            migrationBuilder.CreateIndex(
                name: "IX_staged_files_StoredFilePath",
                table: "staged_files",
                column: "StoredFilePath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_staged_files_Stage_Status",
                table: "staged_files",
                columns: new[] { "Stage", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_staged_files_UploadedAt",
                table: "staged_files",
                column: "UploadedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_staged_files",
                table: "staged_files");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "staged_files");

            migrationBuilder.AddColumn<int>(
                name: "TempIntId",
                table: "staged_files",
                type: "integer",
                nullable: false)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.RenameColumn(
                name: "TempIntId",
                table: "staged_files",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_staged_files",
                table: "staged_files",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_staged_files_StoredFilePath",
                table: "staged_files",
                column: "StoredFilePath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_staged_files_Stage_Status",
                table: "staged_files",
                columns: new[] { "Stage", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_staged_files_UploadedAt",
                table: "staged_files",
                column: "UploadedAt");
        }
    }
}
