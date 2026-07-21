using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ONG_connect.Interfaces;
using ONG_connect.Models;

namespace ONG_connect.Controllers
{
    public class BeneficiarioController : Controller
    {
        private readonly IRepository<Beneficiario> _repository;
        private readonly IRepository<Proyecto> _proyectoRepository;

        public BeneficiarioController(IRepository<Beneficiario> repository, IRepository<Proyecto> proyectoRepository)
        {
            _repository = repository; // Desacoplamiento total
            _proyectoRepository = proyectoRepository;
        }

        // GET: Beneficiario
        public async Task<IActionResult> Index()
        {
            var beneficiarios = await _repository.GetAllAsync();
            return View(beneficiarios);
        }

        // GET: Beneficiario/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var beneficiario = await _repository.GetByIdAsync(id);
            if (beneficiario == null) return NotFound();
            return View(beneficiario);
        }

        // GET: Beneficiario/Create
        public async Task<IActionResult> Create()
        {
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre");
            return View();
        }

        // POST: Beneficiario/Create
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

        // GET: Beneficiario/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var beneficiario = await _repository.GetByIdAsync(id);
            if (beneficiario == null) return NotFound();
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", beneficiario.IdProyecto);
            return View(beneficiario);
        }

        // POST: Beneficiario/Edit/5
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

        // GET: Beneficiario/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var beneficiario = await _repository.GetByIdAsync(id);
            if (beneficiario == null) return NotFound();
            return View(beneficiario);
        }

        // POST: Beneficiario/Delete/5
        [HttpPost, ActionName("Delete")]
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