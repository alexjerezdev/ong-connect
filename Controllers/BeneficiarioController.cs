using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ONG_connect.Interfaces;
using ONG_connect.Models;

namespace ONG_connect.Controllers
{
    [Authorize]
    public class BeneficiarioController : Controller
    {
        private readonly IRepository<Beneficiario> _repository;
        private readonly IRepository<Proyecto> _proyectoRepository;

        public BeneficiarioController(IRepository<Beneficiario> repository, IRepository<Proyecto> proyectoRepository)
        {
            _repository = repository;
            _proyectoRepository = proyectoRepository;
        }

        public async Task<IActionResult> Index()
        {
            var beneficiarios = await _repository.GetAllAsync();
            return View(beneficiarios);
        }

        public async Task<IActionResult> Details(int id)
        {
            var beneficiario = await _repository.GetByIdAsync(id);
            if (beneficiario == null) return NotFound();
            return View(beneficiario);
        }

        public async Task<IActionResult> Create()
        {
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,TipoBeneficiario,CantidadAyuda,IdProyecto")] Beneficiario beneficiario)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(beneficiario);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", beneficiario.IdProyecto);
            return View(beneficiario);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var beneficiario = await _repository.GetByIdAsync(id);
            if (beneficiario == null) return NotFound();
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", beneficiario.IdProyecto);
            return View(beneficiario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdBeneficiario,Nombre,TipoBeneficiario,CantidadAyuda,IdProyecto")] Beneficiario beneficiario)
        {
            if (id != beneficiario.IdBeneficiario) return NotFound();

            if (ModelState.IsValid)
            {
                _repository.Update(beneficiario);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", beneficiario.IdProyecto);
            return View(beneficiario);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var beneficiario = await _repository.GetByIdAsync(id);
            if (beneficiario == null) return NotFound();
            return View(beneficiario);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var beneficiario = await _repository.GetByIdAsync(id);
            if (beneficiario != null)
            {
                _repository.Remove(beneficiario);
                await _repository.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}