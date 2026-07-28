using Microsoft.AspNetCore.Mvc;
using ONG_connect.Interfaces;
using ONG_connect.Models;
using ONG_connect.ViewModels;

namespace ONG_connect.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonacionesApiController : ControllerBase
    {
        private readonly IRepository<Donacion> _repository;

        public DonacionesApiController(IRepository<Donacion> repository) => _repository = repository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonacionDto>>> GetDonaciones()
        {
            var donaciones = await _repository.GetAllAsync();
            var dtos = donaciones.Select(d => new DonacionDto(
                d.IdDonacion, d.NombreDonante, d.TipoDonacion, d.ValorEconomico, d.FechaDonacion));
            return Ok(dtos);
        }
    }
}