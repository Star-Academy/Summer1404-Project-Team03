using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace etl_backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataTableSchema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TableName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OriginalFileName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataTableSchema", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataTableColumns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ColumnName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    OrdinalPosition = table.Column<int>(type: "integer", nullable: false),
                    ColumnType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "string"),
                    TableId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataTableColumns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataTableColumns_DataTableSchema_TableId",
                        column: x => x.TableId,
                        principalTable: "DataTableSchema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StagedFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OriginalFileName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StoredFilePath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SchemaId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StagedFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StagedFiles_DataTableSchema_SchemaId",
                        column: x => x.SchemaId,
                        principalTable: "DataTableSchema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataTableColumns_TableId_ColumnName",
                table: "DataTableColumns",
                columns: new[] { "TableId", "ColumnName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataTableSchema_TableName",
                table: "DataTableSchema",
                column: "TableName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StagedFiles_SchemaId",
                table: "StagedFiles",
                column: "SchemaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataTableColumns");

            migrationBuilder.DropTable(
                name: "StagedFiles");

            migrationBuilder.DropTable(
                name: "DataTableSchema");
        }
    }
}
