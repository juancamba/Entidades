using Entidades.Models;
using Entidades.Repositories;
using Entidades.Services;
using Microsoft.AspNetCore.Mvc;

namespace Entidades.Controllers
{
    public class TipoMuestraController : Controller
    {
        private readonly ITipoMuestraRepository _tipoMuestraRepository;
        public TipoMuestraController(ITipoMuestraRepository tipoMuestraRepository)
        {
            _tipoMuestraRepository = tipoMuestraRepository;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { Data = _tipoMuestraRepository.GetAll() });
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TiposMuestra tipoMuestra)
        {
            if (ModelState.IsValid)
            {
                _tipoMuestraRepository.Create(tipoMuestra);
                return RedirectToAction(nameof(Index));
            }
            return View(tipoMuestra);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var tipoMuestra = _tipoMuestraRepository.GetById(id);
            if (tipoMuestra == null)
            {
                return NotFound();
            }
            return View(tipoMuestra);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TiposMuestra tipoMuestra)
        {
            if (ModelState.IsValid)
            {
                _tipoMuestraRepository.Update(tipoMuestra);
                return RedirectToAction(nameof(Index));
            }
            return View(tipoMuestra);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDeb = _tipoMuestraRepository.GetById(id);

            if (objFromDeb == null)
            {
                return Json(new { success = false, message = "Error borrando tipo de muestra" });
            }
            

            try
            {
                _tipoMuestraRepository.Delete(id);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"No puede borrar un Tipo de muestra con muestras. Detalle: {ex.Message}" });
            }

            return Json(new { success = true, message = "Tipo de muestra borrada" });
        }

    }
}
