using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleAuth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterProfissionalId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remova somente se o índice realmente existir
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1
                    FROM sys.indexes
                    WHERE name = 'IX_Usuarios_ProfissionalId'
                        AND object_id = OBJECT_ID('Auth.Usuarios')
                )
                DROP INDEX IX_Usuarios_ProfissionalId ON Auth.Usuarios;
            ");

            migrationBuilder.DropColumn(
                name: "ProfissionalId",
                schema: "Auth",
                table: "Usuarios");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfissionalId",
                schema: "Auth",
                table: "Usuarios",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_ProfissionalId",
                schema: "Auth",
                table: "Usuarios",
                column: "ProfissionalId",
                unique: true,
                filter: "[ProfissionalId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuarios_ProfissionalId",
                schema: "Auth",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ProfissionalId",
                schema: "Auth",
                table: "Usuarios");

            migrationBuilder.AddColumn<int>(
                name: "ProfissionalId",
                schema: "Auth",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_ProfissionalId",
                schema: "Auth",
                table: "Usuarios",
                column: "ProfissionalId",
                unique: true,
                filter: "[ProfissionalId] IS NOT NULL");
        }
    }
}
