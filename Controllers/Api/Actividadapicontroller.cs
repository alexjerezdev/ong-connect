using Microsoft.AspNetCore.Mvc;
using ONG_connect.Interfaces;
using ONG_connect.Models;
using ONG_connect.ViewModels;

namespace ONG_connect.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActividadApiController : ControllerBase
    {
        private readonly IRepository<Actividad> _repository;
        private readonly IRepository<Proyecto> _proyectoRepository;

        public ActividadApiController(IRepository<Actividad> repository, IRepository<Proyecto> proyectoRepository)
        {
            _repository = repository;
            _proyectoRepository = proyectoRepository;
        }

        // GET: api/ActividadApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActividadDto>>> GetActividades()
        {
            var actividades = await _repository.GetAllAsync();
            var proyectos = (await _proyectoRepository.GetAllAsync())
                .ToDictionary(p => p.IdProyecto, p => p.Nombre);

            var dtos = actividades.Select(a => new ActividadDto(
                a.IdActividad,
                a.Nombre,
                a.Fecha,
                a.Responsable,
                proyectos.GetValueOrDefault(a.IdProyecto, "Sin proyecto")
            ));

            return Ok(dtos);
        }

        // GET: api/ActividadApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActividadDto>> GetActividad(int id)
        {
            var actividad = await _repository.GetByIdAsync(id);

            if (actividad == null)
                return NotFound();

            var proyecto = await _proyectoRepository.GetByIdAsync(actividad.IdProyecto);

            var dto = new ActividadDto(
                actividad.IdActividad,
                actividad.Nombre,
                actividad.Fecha,
                actividad.Responsable,
                proyecto?.Nombre ?? "Sin proyecto"
            );

            return Ok(dto);
        }
    }
}