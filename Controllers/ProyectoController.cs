using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ONG_connect.Interfaces;
using ONG_connect.Models;

namespace ONG_connect.Controllers
{
    [Authorize]
    public class ProyectoController : Controller
    {
        private readonly IRepository<Proyecto> _repository;
        private readonly IRepository<Usuario> _usuarioRepository;

        public ProyectoController(IRepository<Proyecto> repository, IRepository<Usuario> usuarioRepository)
        {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IActionResult> Index()
        {
            var proyectos = await _repository.GetAllAsync();
            return View(proyectos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var proyecto = await _repository.GetByIdAsync(id);
            if (proyecto == null) return NotFound();
            return View(proyecto);
        }

        public async Task<IActionResult> Create()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            ViewData["IdUsuario"] = new SelectList(usuarios, "IdUsuario", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Responsable,Presupuesto,Estado,IdUsuario")] Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(proyecto);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var usuarios = await _usuarioRepository.GetAllAsync();
            ViewData["IdUsuario"] = new SelectList(usuarios, "IdUsuario", "Nombre", proyecto.IdUsuario);
            return View(proyecto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var proyecto = await _repository.GetByIdAsync(id);
            if (proyecto == null) return NotFound();
            var usuarios = await _usuarioRepository.GetAllAsync();
            ViewData["IdUsuario"] = new SelectList(usuarios, "IdUsuario", "Nombre", proyecto.IdUsuario);
            return View(proyecto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProyecto,Nombre,Responsable,Presupuesto,Estado,IdUsuario")] Proyecto proyecto)
        {
            if (id != proyecto.IdProyecto) return NotFound();

            if (ModelState.IsValid)
            {
                _repository.Update(proyecto);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var usuarios = await _usuarioRepository.GetAllAsync();
            ViewData["IdUsuario"] = new SelectList(usuarios, "IdUsuario", "Nombre", proyecto.IdUsuario);
            return View(proyecto);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var proyecto = await _repository.GetByIdAsync(id);
            if (proyecto == null) return NotFound();
            return View(proyecto);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proyecto = await _repository.GetByIdAsync(id);
            if (proyecto != null)
            {
                _repository.Remove(proyecto);
                await _repository.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}