using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ONG_connect.Interfaces;
using ONG_connect.Models;

namespace ONG_connect.Controllers
{
    public class ActividadController : Controller
    {
        private readonly IRepository<Actividad> _repository;
        private readonly IRepository<Proyecto> _proyectoRepository;

        public ActividadController(IRepository<Actividad> repository, IRepository<Proyecto> proyectoRepository)
        {
            _repository = repository; // Desacoplamiento total
            _proyectoRepository = proyectoRepository;
        }

        // GET: Actividad
        public async Task<IActionResult> Index()
        {
            var actividades = await _repository.GetAllAsync();
            return View(actividades);
        }

        // GET: Actividad/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var actividad = await _repository.GetByIdAsync(id);
            if (actividad == null) return NotFound();
            return View(actividad);
        }

        // GET: Actividad/Create
        public async Task<IActionResult> Create()
        {
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre");
            return View();
        }

        // POST: Actividad/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Fecha,Responsable,IdProyecto")] Actividad actividad)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(actividad);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", actividad.IdProyecto);
            return View(actividad);
        }

        // GET: Actividad/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var actividad = await _repository.GetByIdAsync(id);
            if (actividad == null) return NotFound();
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", actividad.IdProyecto);
            return View(actividad);
        }

        // POST: Actividad/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdActividad,Nombre,Fecha,Responsable,IdProyecto")] Actividad actividad)
        {
            if (id != actividad.IdActividad) return NotFound();

            if (ModelState.IsValid)
            {
                _repository.Update(actividad);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", actividad.IdProyecto);
            return View(actividad);
        }

        // GET: Actividad/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var actividad = await _repository.GetByIdAsync(id);
            if (actividad == null) return NotFound();
            return View(actividad);
        }

        // POST: Actividad/Delete/5
        [HttpPost, ActionName("Delete")]
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