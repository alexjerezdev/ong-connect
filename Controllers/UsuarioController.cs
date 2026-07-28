using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ONG_connect.Interfaces;
using ONG_connect.Models;
using ONG_connect.ViewModels;

namespace ONG_connect.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly IRepository<Usuario> _repository;

        public UsuarioController(IRepository<Usuario> repository)
        {
            _repository = repository;
        }

        // GET: Usuario
        public async Task<IActionResult> Index()
        {
            var usuarios = await _repository.GetAllAsync();
            return View(usuarios);
        }

        // GET: Usuario/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        // GET: Usuario/Create
        public IActionResult Create()
        {
            return View(new UsuarioCreateViewModel());
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = new Usuario
            {
                Nombre = model.Nombre,
                Email = model.Email,
                Password = model.Password,
                Rol = "Usuario" // el rol no lo elige quien se registra
            };

            await _repository.AddAsync(usuario);
            await _repository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            if (usuario == null) return NotFound();

            var model = new UsuarioEditViewModel
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol
            };

            return View(model);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioEditViewModel model)
        {
            if (id != model.IdUsuario) return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _repository.GetByIdAsync(id);
            if (usuario == null) return NotFound();

            usuario.Nombre = model.Nombre;
            usuario.Email = model.Email;
            usuario.Rol = model.Rol;

            if (!string.IsNullOrWhiteSpace(model.NuevaPassword))
                usuario.Password = model.NuevaPassword; // ideal: hashearla

            _repository.Update(usuario);
            await _repository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Usuario/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            if (usuario != null)
            {
                _repository.Remove(usuario);
                await _repository.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}