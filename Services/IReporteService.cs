using ONG_connect.ViewModels;

namespace ONG_connect.Services;

public interface IReporteService
{
    Task<byte[]> GenerarExcelDonacionesAsync();
    Task<byte[]> GenerarPdfDonacionesAsync();
    Task<List<DatoGraficoDto>> ObtenerDonacionesPorProyectoAsync();
    Task<List<DatoGraficoDto>> ObtenerActividadesPorProyectoAsync();
    Task<List<DatoGraficoDto>> ObtenerVoluntariosPorEstadoAsync();
    Task<List<DatoGraficoDto>> ObtenerBeneficiariosPorTipoAsync();
    Task<List<PresupuestoProyectoDto>> ObtenerPresupuestoVsDonadoAsync();
}