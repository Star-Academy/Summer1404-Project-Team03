using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace etl_backend.Migrations
{
    /// <inheritdoc />
    public partial class TriadAndSchemaRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataTableColumns_DataTableSchema_TableId",
                table: "DataTableColumns");

            migrationBuilder.DropForeignKey(
                name: "FK_StagedFiles_DataTableSchema_SchemaId",
                table: "StagedFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StagedFiles",
                table: "StagedFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DataTableSchema",
                table: "DataTableSchema");

            migrationBuilder.DropColumn(
                name: "State",
                table: "StagedFiles");

            migrationBuilder.RenameTable(
                name: "StagedFiles",
                newName: "staged_files");

            migrationBuilder.RenameTable(
                name: "DataTableSchema",
                newName: "DataTableSchemas");

            migrationBuilder.RenameColumn(
                name: "TableId",
                table: "DataTableColumns",
                newName: "DataTableSchemaId");

            migrationBuilder.RenameIndex(
                name: "IX_DataTableColumns_TableId_ColumnName",
                table: "DataTableColumns",
                newName: "IX_DataTableColumns_DataTableSchemaId_ColumnName");

            migrationBuilder.RenameIndex(
                name: "IX_StagedFiles_SchemaId",
                table: "staged_files",
                newName: "IX_staged_files_SchemaId");

            migrationBuilder.RenameIndex(
                name: "IX_DataTableSchema_TableName",
                table: "DataTableSchemas",
                newName: "IX_DataTableSchemas_TableName");

            migrationBuilder.AddColumn<string>(
                name: "OriginalColumnName",
                table: "DataTableColumns",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UploadedAt",
                table: "staged_files",
                type: "timestamptz",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "StoredFilePath",
                table: "staged_files",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "OriginalFileName",
                table: "staged_files",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ErrorMessage",
                table: "staged_files",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrorCode",
                table: "staged_files",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Stage",
                table: "staged_files",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "staged_files",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_staged_files",
                table: "staged_files",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataTableSchemas",
                table: "DataTableSchemas",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_DataTableColumns_DataTableSchemas_DataTableSchemaId",
                table: "DataTableColumns",
                column: "DataTableSchemaId",
                principalTable: "DataTableSchemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_staged_files_DataTableSchemas_SchemaId",
                table: "staged_files",
                column: "SchemaId",
                principalTable: "DataTableSchemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataTableColumns_DataTableSchemas_DataTableSchemaId",
                table: "DataTableColumns");

            migrationBuilder.DropForeignKey(
                name: "FK_staged_files_DataTableSchemas_SchemaId",
                table: "staged_files");

            migrationBuilder.DropPrimaryKey(
                name: "PK_staged_files",
                table: "staged_files");

            migrationBuilder.DropIndex(
                name: "IX_staged_files_Stage_Status",
                table: "staged_files");

            migrationBuilder.DropIndex(
                name: "IX_staged_files_StoredFilePath",
                table: "staged_files");

            migrationBuilder.DropIndex(
                name: "IX_staged_files_UploadedAt",
                table: "staged_files");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DataTableSchemas",
                table: "DataTableSchemas");

            migrationBuilder.DropColumn(
                name: "OriginalColumnName",
                table: "DataTableColumns");

            migrationBuilder.DropColumn(
                name: "ErrorCode",
                table: "staged_files");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "staged_files");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "staged_files");

            migrationBuilder.RenameTable(
                name: "staged_files",
                newName: "StagedFiles");

            migrationBuilder.RenameTable(
                name: "DataTableSchemas",
                newName: "DataTableSchema");

            migrationBuilder.RenameColumn(
                name: "DataTableSchemaId",
                table: "DataTableColumns",
                newName: "TableId");

            migrationBuilder.RenameIndex(
                name: "IX_DataTableColumns_DataTableSchemaId_ColumnName",
                table: "DataTableColumns",
                newName: "IX_DataTableColumns_TableId_ColumnName");

            migrationBuilder.RenameIndex(
                name: "IX_staged_files_SchemaId",
                table: "StagedFiles",
                newName: "IX_StagedFiles_SchemaId");

            migrationBuilder.RenameIndex(
                name: "IX_DataTableSchemas_TableName",
                table: "DataTableSchema",
                newName: "IX_DataTableSchema_TableName");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UploadedAt",
                table: "StagedFiles",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamptz");

            migrationBuilder.AlterColumn<string>(
                name: "StoredFilePath",
                table: "StagedFiles",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024);

            migrationBuilder.AlterColumn<string>(
                name: "OriginalFileName",
                table: "StagedFiles",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "ErrorMessage",
                table: "StagedFiles",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "StagedFiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StagedFiles",
                table: "StagedFiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DataTableSchema",
                table: "DataTableSchema",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DataTableColumns_DataTableSchema_TableId",
                table: "DataTableColumns",
                column: "TableId",
                principalTable: "DataTableSchema",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StagedFiles_DataTableSchema_SchemaId",
                table: "StagedFiles",
                column: "SchemaId",
                principalTable: "DataTableSchema",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
