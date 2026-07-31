using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ONG_connect.Interfaces;
using ONG_connect.Models;
using ONG_connect.ViewModels;

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
            return View(new DonacionCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DonacionCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var donacion = new Donacion
                {
                    NombreDonante = viewModel.NombreDonante,
                    TipoDonacion = viewModel.TipoDonacion,
                    ValorEconomico = viewModel.ValorEconomico,
                    FechaDonacion = viewModel.FechaDonacion,
                    IdProyecto = viewModel.IdProyecto
                };

                await _repository.AddAsync(donacion);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", viewModel.IdProyecto);
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var donacion = await _repository.GetByIdAsync(id);
            if (donacion == null) return NotFound();

            var viewModel = new DonacionEditViewModel
            {
                IdDonacion = donacion.IdDonacion,
                NombreDonante = donacion.NombreDonante,
                TipoDonacion = donacion.TipoDonacion,
                ValorEconomico = donacion.ValorEconomico,
                FechaDonacion = donacion.FechaDonacion,
                IdProyecto = donacion.IdProyecto
            };

            var proyectos = await _proyectoRepository.GetAllAsync();
            ViewData["IdProyecto"] = new SelectList(proyectos, "IdProyecto", "Nombre", donacion.IdProyecto);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DonacionEditViewModel viewModel)
        {
            if (id != viewModel.IdDonacion) return NotFound();

            if (ModelState.IsValid)
            {
                var donacion = await _repository.GetByIdAsync(id);
                if (donacion == null) return NotFound();

                donacion.NombreDonante = viewModel.NombreDonante;
                donacion.TipoDonacion = viewModel.TipoDonacion;
                donacion.ValorEconomico = viewModel.ValorEconomico;
                donacion.FechaDonacion = viewModel.FechaDonacion;
                donacion.IdProyecto = viewModel.IdProyecto;

                _repository.Update(donacion);
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