using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ONG_connect.Interfaces;
using ONG_connect.Models;

namespace ONG_connect.Controllers
{
    public class VoluntarioController : Controller
    {
        private readonly IRepository<Voluntario> _repository;
        private readonly IRepository<Proyecto> _proyectoRepository;

        public VoluntarioController(IRepository<Voluntario> repository, IRepository<Proyecto> proyectoRepository)
        {
            _repository = repository; // Desacoplamiento total
            _proyectoRepository = proyectoRepository;
        }

        // GET: Voluntario
        public async Task<IActionResult> Index()
        {
            var voluntarios = await _repository.GetAllAsync();
            return View(voluntarios);
        }

        // GET: Voluntario/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var voluntario = await _repository.GetByIdAsync(id);
            if (voluntario == null) return NotFound();
            return View(voluntario);
        }

        // GET: Voluntario/Create
        public async Task<IActionResult> Create()
        {
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre");
            return View();
        }

        // POST: Voluntario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Telefono,Email,Estado,IdProyecto")] Voluntario voluntario)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(voluntario);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", voluntario.IdProyecto);
            return View(voluntario);
        }

        // GET: Voluntario/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var voluntario = await _repository.GetByIdAsync(id);
            if (voluntario == null) return NotFound();
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", voluntario.IdProyecto);
            return View(voluntario);
        }

        // POST: Voluntario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVoluntario,Nombre,Telefono,Email,Estado,IdProyecto")] Voluntario voluntario)
        {
            if (id != voluntario.IdVoluntario) return NotFound();

            if (ModelState.IsValid)
            {
                _repository.Update(voluntario);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", voluntario.IdProyecto);
            return View(voluntario);
        }

        // GET: Voluntario/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var voluntario = await _repository.GetByIdAsync(id);
            if (voluntario == null) return NotFound();
            return View(voluntario);
        }

        // POST: Voluntario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var voluntario = await _repository.GetByIdAsync(id);
            if (voluntario != null)
            {
                _repository.Remove(voluntario);
                await _repository.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}