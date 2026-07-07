using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ScheduleAPI.Application.Templates
{
    public static class EmailLembreteTemplate
    {
        public static string Gerar(string nomeCliente, string nomeProfissional, string nomeServico, DateTime dataHora, decimal preco)
        {
            return $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                      <meta charset=""utf-8""/>
                      <style>
                        body {{ font-family: Arial, sans-serif; background: #f4f4f4; padding: 20px; }}
                        .container {{ background: white; max-width: 500px; margin: 0 auto;
                                      border-radius: 8px; padding: 30px; }}
                        .header {{ background: #2563eb; color: white; padding: 20px;
                                   border-radius: 8px 8px 0 0; text-align: center; margin: -30px -30px 20px; }}
                        .info {{ background: #f8fafc; border-left: 4px solid #2563eb;
                                 padding: 15px; border-radius: 4px; margin: 15px 0; }}
                        .label {{ color: #64748b; font-size: 12px; text-transform: uppercase; }}
                        .value {{ font-size: 16px; font-weight: bold; color: #1e293b; }}
                        .footer {{ text-align: center; color: #94a3b8; font-size: 12px; margin-top: 20px; }}
                      </style>
                    </head>
                    <body>
                      <div class=""container"">
                        <div class=""header"">
                          <h2>🗓️ Lembrete de Agendamento</h2>
                        </div>
                        <p>Olá, <strong>{nomeCliente}</strong>!</p>
                        <p>Este é um lembrete do seu agendamento para <strong>{dataHora:dd/MM/yyyy}</strong>:</p>

                        <div class=""info"">
                          <div class=""label"">Profissional</div>
                          <div class=""value"">{nomeProfissional}</div>
                        </div>
                        <div class=""info"">
                          <div class=""label"">Serviço</div>
                          <div class=""value"">{nomeServico}</div>
                        </div>
                        <div class=""info"">
                          <div class=""label"">Data e Hora</div>
                          <div class=""value"">{dataHora:dd/MM/yyyy} às {dataHora:HH:mm}</div>
                        </div>
                        <div class=""info"">
                          <div class=""label"">Valor</div>
                          <div class=""value"">R$ {preco:F2}</div>
                        </div>

                        <p>Caso precise cancelar, entre em contato com antecedência.</p>
                        <div class=""footer"">ScheduleAPI — Sistema de Agendamentos</div>
                      </div>
                    </body>
                    </html>
                    ";
        }
    }
}
