using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using ONG_connect.Data;
using ONG_connect.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace ONG_connect.Services;

public class ReporteService : IReporteService
{
    private readonly ApplicationDbContext _context;

    public ReporteService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Consulta optimizada: solo proyecta lo necesario (ODS 9), sin Include innecesario
    private async Task<List<DonacionReporteDto>> ObtenerDonacionesParaReporteAsync()
    {
        return await _context.Donaciones
            .AsNoTracking()
            .OrderByDescending(d => d.FechaDonacion)
            .Select(d => new DonacionReporteDto(
                d.NombreDonante,
                d.TipoDonacion,
                d.ValorEconomico,
                d.FechaDonacion,
                d.Proyecto.Nombre))
            .ToListAsync();
    }

    public async Task<byte[]> GenerarExcelDonacionesAsync()
    {
        var donaciones = await ObtenerDonacionesParaReporteAsync();
        var presupuestoVsDonado = await ObtenerPresupuestoVsDonadoAsync();
        var voluntarios = await ObtenerVoluntariosPorEstadoAsync();
        var beneficiarios = await ObtenerBeneficiariosPorTipoAsync();

        using var libro = new XLWorkbook();

        // ---- Hoja 1: Donaciones ----
        var hojaDonaciones = libro.Worksheets.Add("Donaciones");

        hojaDonaciones.Cell(1, 1).Value = "Donante";
        hojaDonaciones.Cell(1, 2).Value = "Tipo";
        hojaDonaciones.Cell(1, 3).Value = "Valor económico (Bs)";
        hojaDonaciones.Cell(1, 4).Value = "Fecha";
        hojaDonaciones.Cell(1, 5).Value = "Proyecto";

        var encabezadoDonaciones = hojaDonaciones.Range("A1:E1");
        encabezadoDonaciones.Style.Font.Bold = true;
        encabezadoDonaciones.Style.Fill.BackgroundColor = XLColor.FromHtml("#2572A9");
        encabezadoDonaciones.Style.Font.FontColor = XLColor.White;

        for (int i = 0; i < donaciones.Count; i++)
        {
            var fila = i + 2;
            var d = donaciones[i];

            hojaDonaciones.Cell(fila, 1).Value = d.NombreDonante;
            hojaDonaciones.Cell(fila, 2).Value = d.TipoDonacion;

            hojaDonaciones.Cell(fila, 3).Value = d.ValorEconomico;
            hojaDonaciones.Cell(fila, 3).Style.NumberFormat.Format = "0.00"; // ODS 8: precisión decimal

            hojaDonaciones.Cell(fila, 4).Value = d.FechaDonacion;
            hojaDonaciones.Cell(fila, 4).Style.DateFormat.Format = "dd/MM/yyyy";

            hojaDonaciones.Cell(fila, 5).Value = d.NombreProyecto;
        }

        hojaDonaciones.Columns().AdjustToContents();

        // ---- Hoja 2: Presupuesto vs Donado ----
        var hojaPresupuesto = libro.Worksheets.Add("Presupuesto vs Donado");

        hojaPresupuesto.Cell(1, 1).Value = "Proyecto";
        hojaPresupuesto.Cell(1, 2).Value = "Presupuesto (Bs)";
        hojaPresupuesto.Cell(1, 3).Value = "Total donado (Bs)";

        var encabezadoPresupuesto = hojaPresupuesto.Range("A1:C1");
        encabezadoPresupuesto.Style.Font.Bold = true;
        encabezadoPresupuesto.Style.Fill.BackgroundColor = XLColor.FromHtml("#2572A9");
        encabezadoPresupuesto.Style.Font.FontColor = XLColor.White;

        for (int i = 0; i < presupuestoVsDonado.Count; i++)
        {
            var fila = i + 2;
            var p = presupuestoVsDonado[i];

            hojaPresupuesto.Cell(fila, 1).Value = p.NombreProyecto;

            hojaPresupuesto.Cell(fila, 2).Value = p.Presupuesto;
            hojaPresupuesto.Cell(fila, 2).Style.NumberFormat.Format = "0.00";

            hojaPresupuesto.Cell(fila, 3).Value = p.TotalDonado;
            hojaPresupuesto.Cell(fila, 3).Style.NumberFormat.Format = "0.00";
        }

        hojaPresupuesto.Columns().AdjustToContents();

        // ---- Hoja 3: Voluntarios ----
        var hojaVoluntarios = libro.Worksheets.Add("Voluntarios");

        hojaVoluntarios.Cell(1, 1).Value = "Estado";
        hojaVoluntarios.Cell(1, 2).Value = "Cantidad";

        var encabezadoVoluntarios = hojaVoluntarios.Range("A1:B1");
        encabezadoVoluntarios.Style.Font.Bold = true;
        encabezadoVoluntarios.Style.Fill.BackgroundColor = XLColor.FromHtml("#2572A9");
        encabezadoVoluntarios.Style.Font.FontColor = XLColor.White;

        for (int i = 0; i < voluntarios.Count; i++)
        {
            var fila = i + 2;
            hojaVoluntarios.Cell(fila, 1).Value = voluntarios[i].Etiqueta;
            hojaVoluntarios.Cell(fila, 2).Value = voluntarios[i].Valor;
        }

        hojaVoluntarios.Columns().AdjustToContents();

        // ---- Hoja 4: Beneficiarios ----
        var hojaBeneficiarios = libro.Worksheets.Add("Beneficiarios");

        hojaBeneficiarios.Cell(1, 1).Value = "Tipo";
        hojaBeneficiarios.Cell(1, 2).Value = "Cantidad";

        var encabezadoBeneficiarios = hojaBeneficiarios.Range("A1:B1");
        encabezadoBeneficiarios.Style.Font.Bold = true;
        encabezadoBeneficiarios.Style.Fill.BackgroundColor = XLColor.FromHtml("#2572A9");
        encabezadoBeneficiarios.Style.Font.FontColor = XLColor.White;

        for (int i = 0; i < beneficiarios.Count; i++)
        {
            var fila = i + 2;
            hojaBeneficiarios.Cell(fila, 1).Value = beneficiarios[i].Etiqueta;
            hojaBeneficiarios.Cell(fila, 2).Value = beneficiarios[i].Valor;
        }

        hojaBeneficiarios.Columns().AdjustToContents();

        using var memoria = new MemoryStream();
        libro.SaveAs(memoria);
        return memoria.ToArray();
    }

    public async Task<byte[]> GenerarPdfDonacionesAsync()
    {
        // Se traen todos los datos del reporte general, no solo donaciones
        var donaciones = await ObtenerDonacionesParaReporteAsync();
        var presupuestoVsDonado = await ObtenerPresupuestoVsDonadoAsync();
        var voluntarios = await ObtenerVoluntariosPorEstadoAsync();
        var beneficiarios = await ObtenerBeneficiariosPorTipoAsync();

        var totalDonado = donaciones.Sum(d => d.ValorEconomico);
        var totalVoluntarios = voluntarios.Sum(v => v.Valor);
        var totalBeneficiarios = beneficiarios.Sum(b => b.Valor);

        var documento = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Column(col =>
                {
                    col.Item().Text("Reporte General - ONG_connect")
                        .FontSize(18).SemiBold().FontColor("#2572A9");
                    col.Item().Text($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}")
                        .FontSize(9).FontColor(Colors.Grey.Darken1);
                });

                page.Content().PaddingTop(15).Column(content =>
                {
                    content.Spacing(15);

                    // ---- Resumen ejecutivo (KPIs) ----
                    content.Item().Text("Resumen general").FontSize(13).SemiBold().FontColor("#2572A9");
                    content.Item().Row(row =>
                    {
                        row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Column(c =>
                        {
                            c.Item().Text("Total donado (Bs)").FontSize(9).FontColor(Colors.Grey.Darken1);
                            c.Item().Text(totalDonado.ToString("N2")).FontSize(14).SemiBold();
                        });
                        row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Column(c =>
                        {
                            c.Item().Text("Voluntarios (total)").FontSize(9).FontColor(Colors.Grey.Darken1);
                            c.Item().Text(totalVoluntarios.ToString("N0")).FontSize(14).SemiBold();
                        });
                        row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Column(c =>
                        {
                            c.Item().Text("Beneficiarios (total)").FontSize(9).FontColor(Colors.Grey.Darken1);
                            c.Item().Text(totalBeneficiarios.ToString("N0")).FontSize(14).SemiBold();
                        });
                        row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Column(c =>
                        {
                            c.Item().Text("Proyectos").FontSize(9).FontColor(Colors.Grey.Darken1);
                            c.Item().Text(presupuestoVsDonado.Count.ToString()).FontSize(14).SemiBold();
                        });
                    });

                    // ---- Presupuesto vs donado por proyecto ----
                    content.Item().Text("Presupuesto vs. donado por proyecto").FontSize(13).SemiBold().FontColor("#2572A9");
                    content.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Proyecto").SemiBold();
                            header.Cell().Text("Presupuesto (Bs)").SemiBold();
                            header.Cell().Text("Donado (Bs)").SemiBold();

                            header.Cell().ColumnSpan(3).PaddingTop(5)
                                .BorderBottom(1).BorderColor(Colors.Grey.Lighten1);
                        });

                        foreach (var p in presupuestoVsDonado)
                        {
                            table.Cell().Text(p.NombreProyecto);
                            table.Cell().Text(p.Presupuesto.ToString("N2"));
                            table.Cell().Text(p.TotalDonado.ToString("N2"));
                        }
                    });

                    // ---- Voluntarios y beneficiarios ----
                    content.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("Voluntarios por estado").FontSize(13).SemiBold().FontColor("#2572A9");
                            c.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Text("Estado").SemiBold();
                                    header.Cell().Text("Cantidad").SemiBold();
                                    header.Cell().ColumnSpan(2).PaddingTop(5)
                                        .BorderBottom(1).BorderColor(Colors.Grey.Lighten1);
                                });

                                foreach (var v in voluntarios)
                                {
                                    table.Cell().Text(v.Etiqueta);
                                    table.Cell().Text(v.Valor.ToString("N0"));
                                }
                            });
                        });

                        row.ConstantItem(20); // separación entre columnas

                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("Beneficiarios por tipo").FontSize(13).SemiBold().FontColor("#2572A9");
                            c.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Text("Tipo").SemiBold();
                                    header.Cell().Text("Cantidad").SemiBold();
                                    header.Cell().ColumnSpan(2).PaddingTop(5)
                                        .BorderBottom(1).BorderColor(Colors.Grey.Lighten1);
                                });

                                foreach (var b in beneficiarios)
                                {
                                    table.Cell().Text(b.Etiqueta);
                                    table.Cell().Text(b.Valor.ToString("N0"));
                                }
                            });
                        });
                    });

                    // ---- Detalle de donaciones ----
                    content.Item().Text("Detalle de donaciones").FontSize(13).SemiBold().FontColor("#2572A9");
                    content.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Donante").SemiBold();
                            header.Cell().Text("Tipo").SemiBold();
                            header.Cell().Text("Valor (Bs)").SemiBold();
                            header.Cell().Text("Fecha").SemiBold();
                            header.Cell().Text("Proyecto").SemiBold();

                            header.Cell().ColumnSpan(5).PaddingTop(5)
                                .BorderBottom(1).BorderColor(Colors.Grey.Lighten1);
                        });

                        foreach (var d in donaciones)
                        {
                            table.Cell().Text(d.NombreDonante);
                            table.Cell().Text(d.TipoDonacion);
                            table.Cell().Text(d.ValorEconomico.ToString("N2")); // 2 decimales
                            table.Cell().Text(d.FechaDonacion.ToString("dd/MM/yyyy"));
                            table.Cell().Text(d.NombreProyecto);
                        }
                    });
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Página ");
                    x.CurrentPageNumber();
                    x.Span(" de ");
                    x.TotalPages();
                });
            });
        });

        return documento.GeneratePdf();
    }

    public async Task<List<DatoGraficoDto>> ObtenerDonacionesPorProyectoAsync()
    {
        return await _context.Donaciones
            .AsNoTracking()
            .GroupBy(d => d.Proyecto.Nombre)
            .Select(g => new DatoGraficoDto(g.Key, g.Sum(d => d.ValorEconomico)))
            .ToListAsync();
    }

    public async Task<List<DatoGraficoDto>> ObtenerActividadesPorProyectoAsync()
    {
        return await _context.Actividades
            .AsNoTracking()
            .GroupBy(a => a.Proyecto.Nombre)
            .Select(g => new DatoGraficoDto(g.Key, g.Count()))
            .ToListAsync();
    }

    // Cuenta voluntarios activos vs inactivos (Estado = true/false)
    public async Task<List<DatoGraficoDto>> ObtenerVoluntariosPorEstadoAsync()
    {
        return await _context.Voluntarios
            .AsNoTracking()
            .GroupBy(v => v.Estado)
            .Select(g => new DatoGraficoDto(g.Key ? "Activos" : "Inactivos", g.Count()))
            .ToListAsync();
    }

    // Cuenta beneficiarios agrupados por tipo (ej: Familia, Individuo, Institución)
    public async Task<List<DatoGraficoDto>> ObtenerBeneficiariosPorTipoAsync()
    {
        return await _context.Beneficiarios
            .AsNoTracking()
            .GroupBy(b => b.TipoBeneficiario)
            .Select(g => new DatoGraficoDto(g.Key, g.Count()))
            .ToListAsync();
    }

    // Compara el presupuesto asignado a cada proyecto contra lo efectivamente donado
    public async Task<List<PresupuestoProyectoDto>> ObtenerPresupuestoVsDonadoAsync()
    {
        return await _context.Proyectos
            .AsNoTracking()
            .Select(p => new PresupuestoProyectoDto(
                p.Nombre,
                p.Presupuesto,
                p.Donaciones.Sum(d => (decimal?)d.ValorEconomico) ?? 0))
            .ToListAsync();
    }
}