using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ONG_connect.Interfaces;
using ONG_connect.Models;
using ONG_connect.ViewModels;

namespace ONG_connect.Controllers
{
    [Authorize]
    public class ActividadController : Controller
    {
        private readonly IRepository<Actividad> _repository;
        private readonly IRepository<Proyecto> _proyectoRepository;

        public ActividadController(IRepository<Actividad> repository, IRepository<Proyecto> proyectoRepository)
        {
            _repository = repository;
            _proyectoRepository = proyectoRepository;
        }

        public async Task<IActionResult> Index()
        {
            var actividades = await _repository.GetAllAsync();
            return View(actividades);
        }

        public async Task<IActionResult> Details(int id)
        {
            var actividad = await _repository.GetByIdAsync(id);
            if (actividad == null) return NotFound();
            return View(actividad);
        }

        public async Task<IActionResult> Create()
        {
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre");
            return View(new ActividadCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActividadCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var actividad = new Actividad
                {
                    Nombre = viewModel.Nombre,
                    Fecha = viewModel.Fecha,
                    Responsable = viewModel.Responsable,
                    IdProyecto = viewModel.IdProyecto
                };

                await _repository.AddAsync(actividad);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", viewModel.IdProyecto);
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var actividad = await _repository.GetByIdAsync(id);
            if (actividad == null) return NotFound();

            var viewModel = new ActividadEditViewModel
            {
                IdActividad = actividad.IdActividad,
                Nombre = actividad.Nombre,
                Fecha = actividad.Fecha,
                Responsable = actividad.Responsable,
                IdProyecto = actividad.IdProyecto
            };

            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", actividad.IdProyecto);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActividadEditViewModel viewModel)
        {
            if (id != viewModel.IdActividad) return NotFound();

            if (ModelState.IsValid)
            {
                var actividad = await _repository.GetByIdAsync(id);
                if (actividad == null) return NotFound();

                actividad.Nombre = viewModel.Nombre;
                actividad.Fecha = viewModel.Fecha;
                actividad.Responsable = viewModel.Responsable;
                actividad.IdProyecto = viewModel.IdProyecto;

                _repository.Update(actividad);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", viewModel.IdProyecto);
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var actividad = await _repository.GetByIdAsync(id);
            if (actividad == null) return NotFound();
            return View(actividad);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actividad = await _repository.GetByIdAsync(id);
            if (actividad != null)
            {
                _repository.Remove(actividad);
                await _repository.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}