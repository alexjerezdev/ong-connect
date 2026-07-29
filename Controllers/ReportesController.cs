using Microsoft.AspNetCore.Mvc;
using ONG_connect.Services;

namespace ONG_connect.Controllers;

public class ReportesController : Controller
{
    private readonly IReporteService _reporteService;

    public ReportesController(IReporteService reporteService)
    {
        _reporteService = reporteService;
    }

    // GET: /Reportes/Dashboard
    public IActionResult Dashboard()
    {
        return View();
    }

    // GET: /Reportes/DescargarExcel
    public async Task<IActionResult> DescargarExcel()
    {
        var archivo = await _reporteService.GenerarExcelDonacionesAsync();
        var nombreArchivo = $"Reporte_Donaciones_{DateTime.Now:yyyyMMdd}.xlsx";
        return File(archivo,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            nombreArchivo);
    }

    // GET: /Reportes/DescargarPdf
    public async Task<IActionResult> DescargarPdf()
    {
        var archivo = await _reporteService.GenerarPdfDonacionesAsync();
        var nombreArchivo = $"Reporte_Donaciones_{DateTime.Now:yyyyMMdd}.pdf";
        return File(archivo, "application/pdf", nombreArchivo);
    }

    // GET: /Reportes/DatosDonacionesPorProyecto
    public async Task<IActionResult> DatosDonacionesPorProyecto()
    {
        var datos = await _reporteService.ObtenerDonacionesPorProyectoAsync();
        return Json(datos);
    }

    // GET: /Reportes/DatosActividadesPorProyecto
    public async Task<IActionResult> DatosActividadesPorProyecto()
    {
        var datos = await _reporteService.ObtenerActividadesPorProyectoAsync();
        return Json(datos);
    }

    // GET: /Reportes/DatosVoluntariosPorEstado
    public async Task<IActionResult> DatosVoluntariosPorEstado()
    {
        var datos = await _reporteService.ObtenerVoluntariosPorEstadoAsync();
        return Json(datos);
    }

    // GET: /Reportes/DatosBeneficiariosPorTipo
    public async Task<IActionResult> DatosBeneficiariosPorTipo()
    {
        var datos = await _reporteService.ObtenerBeneficiariosPorTipoAsync();
        return Json(datos);
    }

    // GET: /Reportes/DatosPresupuestoVsDonado
    public async Task<IActionResult> DatosPresupuestoVsDonado()
    {
        var datos = await _reporteService.ObtenerPresupuestoVsDonadoAsync();
        return Json(datos);
    }
}