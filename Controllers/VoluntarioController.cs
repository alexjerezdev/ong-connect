using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ONG_connect.Interfaces;
using ONG_connect.Models;
using ONG_connect.ViewModels;

namespace ONG_connect.Controllers
{
    [Authorize]
    public class VoluntarioController : Controller
    {
        private readonly IRepository<Voluntario> _repository;
        private readonly IRepository<Proyecto> _proyectoRepository;

        public VoluntarioController(IRepository<Voluntario> repository, IRepository<Proyecto> proyectoRepository)
        {
            _repository = repository;
            _proyectoRepository = proyectoRepository;
        }

        public async Task<IActionResult> Index()
        {
            var voluntarios = await _repository.GetAllAsync();
            return View(voluntarios);
        }

        public async Task<IActionResult> Details(int id)
        {
            var voluntario = await _repository.GetByIdAsync(id);
            if (voluntario == null) return NotFound();
            return View(voluntario);
        }

        public async Task<IActionResult> Create()
        {
            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre");
            return View(new VoluntarioCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VoluntarioCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var voluntario = new Voluntario
                {
                    Nombre = viewModel.Nombre,
                    Telefono = viewModel.Telefono,
                    Email = viewModel.Email,
                    Estado = viewModel.Estado,
                    IdProyecto = viewModel.IdProyecto
                };

                await _repository.AddAsync(voluntario);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", viewModel.IdProyecto);
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var voluntario = await _repository.GetByIdAsync(id);
            if (voluntario == null) return NotFound();

            var viewModel = new VoluntarioEditViewModel
            {
                IdVoluntario = voluntario.IdVoluntario,
                Nombre = voluntario.Nombre,
                Telefono = voluntario.Telefono,
                Email = voluntario.Email,
                Estado = voluntario.Estado,
                IdProyecto = voluntario.IdProyecto
            };

            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", voluntario.IdProyecto);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VoluntarioEditViewModel viewModel)
        {
            if (id != viewModel.IdVoluntario) return NotFound();

            if (ModelState.IsValid)
            {
                var voluntario = await _repository.GetByIdAsync(id);
                if (voluntario == null) return NotFound();

                voluntario.Nombre = viewModel.Nombre;
                voluntario.Telefono = viewModel.Telefono;
                voluntario.Email = viewModel.Email;
                voluntario.Estado = viewModel.Estado;
                voluntario.IdProyecto = viewModel.IdProyecto;

                _repository.Update(voluntario);
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
            var voluntario = await _repository.GetByIdAsync(id);
            if (voluntario == null) return NotFound();
            return View(voluntario);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
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