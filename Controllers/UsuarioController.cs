using Microsoft.AspNetCore.Mvc;
using ONG_connect.Interfaces;
using ONG_connect.Models;

namespace ONG_connect.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IRepository<Usuario> _repository;

        public UsuarioController(IRepository<Usuario> repository)
        {
            _repository = repository; // Desacoplamiento total
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
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Email,Password,Rol")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(usuario);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,Nombre,Email,Password,Rol")] Usuario usuario)
        {
            if (id != usuario.IdUsuario) return NotFound();

            if (ModelState.IsValid)
            {
                _repository.Update(usuario);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _repository.GetByIdAsync(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
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