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
        private readonly IMuestraRepository _muestraRepository;
        public ValoresReferenciaController(IValoresReferenciaRepository valoreReferenciaRepository, ITipoMuestraRepository tipoMuestraRepository, IMapper mapper, IMuestraRepository muestraRepository)
        {
            _valoreReferenciaRepository = valoreReferenciaRepository;
            _tipoMuestraRepository = tipoMuestraRepository;
            _mapper = mapper;
            _muestraRepository = muestraRepository;
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



            ValoresReferenciaVM valoresReferenciaVM = new ValoresReferenciaVM()
            {
                Items = _tipoMuestraRepository.GetListaTiposMuestra(),
                //TiposMuestraDto = tiposMuestrasDto,
            };

            return View(valoresReferenciaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ValoresReferenciaVM valoreReferenciavm)
        {

            ValoresReferencia valoresReferencia = new ValoresReferencia()
            {
                Maximo = valoreReferenciavm.ValoresReferencia.Maximo,
                Minimo = valoreReferenciavm.ValoresReferencia.Minimo,
                IdNombreVariableMuestra = valoreReferenciavm.ValoresReferencia.IdNombreVariableMuestra,


            };


            _valoreReferenciaRepository.Create(valoresReferencia);
            return RedirectToAction(nameof(Index));


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

            _valoreReferenciaRepository.Update(valoresReferencia);
            return RedirectToAction(nameof(Index));


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
        public IActionResult ObtenerNombresVariables(int idTipoMuestra)
        {


            IEnumerable<NombresVariablesMuestra> nombreVariableMuestra = _valoreReferenciaRepository.ObtenerVariablesSinValoresReferencia(idTipoMuestra);
            IEnumerable<NombreVariableMuestraDto> nombreVariableMuestraDto = nombreVariableMuestra.Select(t => _mapper.Map<NombreVariableMuestraDto>(t));

            return Json(new { Data = nombreVariableMuestraDto });
        }
    }
}
