using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ONG_connect.Interfaces;
using ONG_connect.Models;

namespace ONG_connect.Controllers
{
    [Authorize]
    public class DonacionController : Controller
    {
        private readonly IRepository<Donacion> _repository;
        private readonly IRepository<Proyecto> _proyectoRepository;

        public DonacionController(IRepository<Donacion> repository, IRepository<Proyecto> proyectoRepository)
        {
            _repository = repository;
            _proyectoRepository = proyectoRepository;
        }

        public async Task<IActionResult> Index()
        {
            var donaciones = await _repository.GetAllAsync();
            return View(donaciones);
        }

        public async Task<IActionResult> Details(int id)
        {
            var donacion = await _repository.GetByIdAsync(id);
            if (donacion == null) return NotFound();
            return View(donacion);
        }

        public async Task<IActionResult> Create()
        {
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre");
            return View();
        }

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

        public async Task<IActionResult> Edit(int id)
        {
            var donacion = await _repository.GetByIdAsync(id);
            if (donacion == null) return NotFound();
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", donacion.IdProyecto);
            return View(donacion);
        }

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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var donacion = await _repository.GetByIdAsync(id);
            if (donacion == null) return NotFound();
            return View(donacion);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
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