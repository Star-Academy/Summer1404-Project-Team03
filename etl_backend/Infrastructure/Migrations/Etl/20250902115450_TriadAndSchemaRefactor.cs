#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Migrations.Etl
{
    /// <inheritdoc />
    public partial class TriadAndSchemaRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataTableSchemas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TableName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OriginalFileName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataTableSchemas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataTableColumns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ColumnName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    OriginalColumnName = table.Column<string>(type: "text", nullable: true),
                    OrdinalPosition = table.Column<int>(type: "integer", nullable: false),
                    ColumnType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "string"),
                    DataTableSchemaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataTableColumns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataTableColumns_DataTableSchemas_DataTableSchemaId",
                        column: x => x.DataTableSchemaId,
                        principalTable: "DataTableSchemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "staged_files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OriginalFileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    StoredFilePath = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    Stage = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ErrorCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    SchemaId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_staged_files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_staged_files_DataTableSchemas_SchemaId",
                        column: x => x.SchemaId,
                        principalTable: "DataTableSchemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataTableColumns_DataTableSchemaId_ColumnName",
                table: "DataTableColumns",
                columns: new[] { "DataTableSchemaId", "ColumnName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataTableSchemas_TableName",
                table: "DataTableSchemas",
                column: "TableName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_staged_files_SchemaId",
                table: "staged_files",
                column: "SchemaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_staged_files_Stage_Status",
                table: "staged_files",
                columns: new[] { "Stage", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_staged_files_StoredFilePath",
                table: "staged_files",
                column: "StoredFilePath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_staged_files_UploadedAt",
                table: "staged_files",
                column: "UploadedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataTableColumns");

            migrationBuilder.DropTable(
                name: "staged_files");

            migrationBuilder.DropTable(
                name: "DataTableSchemas");
        }
    }
}
