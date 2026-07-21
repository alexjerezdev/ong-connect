using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ONG_connect.Interfaces;
using ONG_connect.Models;

namespace ONG_connect.Controllers
{
    public class DonacionController : Controller
    {
        private readonly IRepository<Donacion> _repository;
        private readonly IRepository<Proyecto> _proyectoRepository;

        public DonacionController(IRepository<Donacion> repository, IRepository<Proyecto> proyectoRepository)
        {
            _repository = repository; // Desacoplamiento total
            _proyectoRepository = proyectoRepository;
        }

        // GET: Donacion
        public async Task<IActionResult> Index()
        {
            var donaciones = await _repository.GetAllAsync();
            return View(donaciones);
        }

        // GET: Donacion/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var donacion = await _repository.GetByIdAsync(id);
            if (donacion == null) return NotFound();
            return View(donacion);
        }

        // GET: Donacion/Create
        public async Task<IActionResult> Create()
        {
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre");
            return View();
        }

        // POST: Donacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreDonante,TipoDonacion,ValorEconomico,FechaDonacion,IdProyecto")] Donacion donacion)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(donacion);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", donacion.IdProyecto);
            return View(donacion);
        }

        // GET: Donacion/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var donacion = await _repository.GetByIdAsync(id);
            if (donacion == null) return NotFound();
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", donacion.IdProyecto);
            return View(donacion);
        }

        // POST: Donacion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDonacion,NombreDonante,TipoDonacion,ValorEconomico,FechaDonacion,IdProyecto")] Donacion donacion)
        {
            if (id != donacion.IdDonacion) return NotFound();

            if (ModelState.IsValid)
            {
                _repository.Update(donacion);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", donacion.IdProyecto);
            return View(donacion);
        }

        // GET: Donacion/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var donacion = await _repository.GetByIdAsync(id);
            if (donacion == null) return NotFound();
            return View(donacion);
        }

        // POST: Donacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donacion = await _repository.GetByIdAsync(id);
            if (donacion != null)
            {
                _repository.Remove(donacion);
                await _repository.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}