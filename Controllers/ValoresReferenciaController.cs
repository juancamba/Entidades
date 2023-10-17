using AutoMapper;
using Entidades.Models;
using Entidades.Models.DTO;
using Entidades.Models.ViewModels;
using Entidades.Repositories;
using Entidades.Services;
using Microsoft.AspNetCore.Mvc;

namespace Entidades.Controllers
{
    public class ValoresReferenciaController : Controller
    {
        private readonly IValoresReferenciaRepository _valoreReferenciaRepository;
        private readonly ITipoMuestraRepository _tipoMuestraRepository;
        private readonly IMapper _mapper;
        public ValoresReferenciaController(IValoresReferenciaRepository valoreReferenciaRepository, ITipoMuestraRepository tipoMuestraRepository, IMapper mapper)
        {
            _valoreReferenciaRepository = valoreReferenciaRepository;
            _tipoMuestraRepository = tipoMuestraRepository;
            _mapper = mapper;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { Data = _valoreReferenciaRepository.GetAll() });
        }
        [HttpGet]
        public IActionResult Create()
        {

            IEnumerable<TiposMuestra> tiposMuestras = _tipoMuestraRepository.GetAll();
            IEnumerable<TipoMuestraDto> tiposMuestrasDto = tiposMuestras.Select(tipoMuestra => _mapper.Map<TipoMuestraDto>(tipoMuestra));
            ValoresReferenciaVM valoresReferenciaVM = new ValoresReferenciaVM()
            {
                TiposMuestraDto = tiposMuestrasDto
            };

            return View(valoresReferenciaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ValoresReferencia valoreReferencia)
        {
            if (ModelState.IsValid)
            {
                _valoreReferenciaRepository.Create(valoreReferencia);
                return RedirectToAction(nameof(Index));
            }
            return View(valoreReferencia);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var tipoMuestra = _valoreReferenciaRepository.GetById(id);
            if (tipoMuestra == null)
            {
                return NotFound();
            }
            return View(tipoMuestra);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ValoresReferencia valoresReferencia)
        {
            if (ModelState.IsValid)
            {
                _valoreReferenciaRepository.Update(valoresReferencia);
                return RedirectToAction(nameof(Index));
            }
            return View(valoresReferencia);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDeb = _valoreReferenciaRepository.GetById(id);

            if (objFromDeb == null)
            {
                return Json(new { success = false, message = "Error borrando Valores Referencia" });
            }
            _valoreReferenciaRepository.Delete(id);

            return Json(new { success = true, message = "Valores Referencia borrada" });
        }
    }
}
