using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ONG_connect.Interfaces;
using ONG_connect.Models;
using ONG_connect.ViewModels;

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
            return View(new ProyectoCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProyectoCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var proyecto = new Proyecto
                {
                    Nombre = viewModel.Nombre,
                    Responsable = viewModel.Responsable,
                    Presupuesto = viewModel.Presupuesto,
                    Estado = viewModel.Estado,
                    IdUsuario = viewModel.IdUsuario
                };

                await _repository.AddAsync(proyecto);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var usuarios = await _usuarioRepository.GetAllAsync();
            ViewData["IdUsuario"] = new SelectList(usuarios, "IdUsuario", "Nombre", viewModel.IdUsuario);
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var proyecto = await _repository.GetByIdAsync(id);
            if (proyecto == null) return NotFound();

            var viewModel = new ProyectoEditViewModel
            {
                IdProyecto = proyecto.IdProyecto,
                Nombre = proyecto.Nombre,
                Responsable = proyecto.Responsable,
                Presupuesto = proyecto.Presupuesto,
                Estado = proyecto.Estado,
                IdUsuario = proyecto.IdUsuario
            };

            var usuarios = await _usuarioRepository.GetAllAsync();
            ViewData["IdUsuario"] = new SelectList(usuarios, "IdUsuario", "Nombre", proyecto.IdUsuario);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProyectoEditViewModel viewModel)
        {
            if (id != viewModel.IdProyecto) return NotFound();

            if (ModelState.IsValid)
            {
                var proyecto = await _repository.GetByIdAsync(id);
                if (proyecto == null) return NotFound();

                proyecto.Nombre = viewModel.Nombre;
                proyecto.Responsable = viewModel.Responsable;
                proyecto.Presupuesto = viewModel.Presupuesto;
                proyecto.Estado = viewModel.Estado;
                proyecto.IdUsuario = viewModel.IdUsuario;

                _repository.Update(proyecto);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var usuarios = await _usuarioRepository.GetAllAsync();
            ViewData["IdUsuario"] = new SelectList(usuarios, "IdUsuario", "Nombre", viewModel.IdUsuario);
            return View(viewModel);
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