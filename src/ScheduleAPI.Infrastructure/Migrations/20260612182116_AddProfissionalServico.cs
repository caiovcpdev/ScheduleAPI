using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProfissionalServico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataHoraInicio",
                table: "Agendamentos",
                newName: "DataHoraInicio");

            migrationBuilder.RenameColumn(
                name: "DataHoraFim",
                table: "Agendamentos",
                newName: "DataHoraFim");

            migrationBuilder.CreateTable(
                name: "ProfissionalServico",
                columns: table => new
                {
                    ProfissionalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfissionalServico", x => new { x.ProfissionalId, x.ServicoId });
                    table.ForeignKey(
                        name: "FK_ProfissionalServico_Profissionais_ProfissionalId",
                        column: x => x.ProfissionalId,
                        principalTable: "Profissionais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfissionalServico_Servicos_ServicoId",
                        column: x => x.ServicoId,
                        principalTable: "Servicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionalServico_ServicoId",
                table: "ProfissionalServico",
                column: "ServicoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfissionalServico");

            migrationBuilder.RenameColumn(
                name: "DataHoraInicio",
                table: "Agendamentos",
                newName: "DataHorarioInicio");

            migrationBuilder.RenameColumn(
                name: "DataHoraFim",
                table: "Agendamentos",
                newName: "DataHorarioFim");
        }
    }
}
