namespace ONG_connect.ViewModels;

public record DonacionReporteDto(
    string NombreDonante,
    string TipoDonacion,
    decimal ValorEconomico,
    DateTime FechaDonacion,
    string NombreProyecto);