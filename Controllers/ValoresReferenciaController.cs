using AutoMapper;
using Entidades.Models;
using Entidades.Models.DTO;
using Entidades.Models.ViewModels;
using Entidades.Repositories;
using Entidades.Serivces.Ficheros;
using Entidades.Services;
using Microsoft.AspNetCore.Mvc;
using static Entidades.Models.Enum;

namespace Entidades.Controllers
{
    public class ValoresReferenciaController : BaseController
    {
        private readonly IValoresReferenciaRepository _valoreReferenciaRepository;
        private readonly ITipoMuestraRepository _tipoMuestraRepository;
        private readonly IMapper _mapper;
        private readonly IMuestraRepository _muestraRepository;
        private readonly IFicheroValoresReferenciaService _ficheroValoresReferenciaService;
        public ValoresReferenciaController(IValoresReferenciaRepository valoreReferenciaRepository, ITipoMuestraRepository tipoMuestraRepository, IMapper mapper, IMuestraRepository muestraRepository, IFicheroValoresReferenciaService ficheroValoresReferenciaService)
        {
            _valoreReferenciaRepository = valoreReferenciaRepository;
            _tipoMuestraRepository = tipoMuestraRepository;
            _ficheroValoresReferenciaService = ficheroValoresReferenciaService;
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
        public IActionResult Cargar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cargar(IFormFile file)
        {


            try
            {
                if (file == null)
                {
                    Alert($"Debe cargar un archivo", NotificationType.error);
                    return View();
                }
                List<FicheroValorReferenciaDto> valoresRefencia = _ficheroValoresReferenciaService.Cargar(file);
                _valoreReferenciaRepository.AltaMasiva(valoresRefencia);
                //_muestraRepository.AltaConjuntoMuestra(conjuntoMuestra);
                Alert($"archivo cargado: {file.FileName} correctamente", NotificationType.success);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Alert($"Error al cargar el archivo, revise el formato. Detalle: {ex.Message}", NotificationType.error);
            }


            return View();
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

        [HttpPost]
        public IActionResult DeleteMultiple([FromBody] int[] itemIds)
        {
            try
            {
                _valoreReferenciaRepository.DeleteMultiple(itemIds);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al borrar valores de referencia. Detalle: {ex.Message}" });
            }
            return Json(new { success = true, message = "valores borrados" });
        }
    }
}
