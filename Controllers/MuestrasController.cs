using AutoMapper;
using Entidades.Models;
using Entidades.Models.DTO;
using Entidades.Models.ViewModels;
using Entidades.Repositories;
using Entidades.Services;
using Entidades.Services.Ficheros;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Entidades.Models.Enum;

namespace Entidades.Controllers
{
    public class MuestrasController : BaseController
    {


        private readonly AppDbContext _dbContext;
        private readonly IFicheroMuestraService _ficheroMuestraService;
        private readonly IMuestraRepository _muestraRepository;
        private readonly ICampoRepository _campoRepository;
        private readonly ITipoMuestraRepository _tipoMuestraRepository;
        private readonly IMapper _mapper;

        public MuestrasController(AppDbContext entidadesContext, IFicheroMuestraService ficheroMuestraService, IMuestraRepository muestraRepository, ICampoRepository campoRepository, ITipoMuestraRepository tipoMuestraRepository, IMapper mapper)
        {
            _dbContext = entidadesContext;
            _ficheroMuestraService = ficheroMuestraService;
            _muestraRepository = muestraRepository;
            _campoRepository = campoRepository;
            _tipoMuestraRepository = tipoMuestraRepository;
            _mapper = mapper;


        }

        public IActionResult Index()
        {
            return View();
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
                ConjuntoMuestra conjuntoMuestra = _ficheroMuestraService.Cargar(file);
                _muestraRepository.AltaConjuntoMuestra(conjuntoMuestra);
                Alert($"archivo cargado: {file.FileName} correctamente", NotificationType.success);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Alert($"Error al cargar el archivo, revise el formato. Detalle: {ex.Message}", NotificationType.error);
            }


            return View();
        }

        public IActionResult GetAll()
        {
            return Json(new { Data = _muestraRepository.GetAll() });
        }

        public IActionResult Ver(int? id)
        {

            MuestraVM muestraVM = new MuestraVM()
            {
                MuestraResumenDto = _muestraRepository.GetAll().FirstOrDefault(x => x.Id == id.GetValueOrDefault()),
                MuestraDetalleDto = _muestraRepository.GetDetalle(id.GetValueOrDefault())
            };

            return View(muestraVM);
        }
        [HttpDelete()]
        public IActionResult Delete(int id)
        {
            var objFromDeb = _muestraRepository.GetById(id);

            if (objFromDeb == null)
            {
                return Json(new { success = false, message = "Error borrando Muestra" });
            }

            try
            {
                _muestraRepository.Delete(id);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al borrar muestras. Detalle: {ex.Message}" });
            }


            return Json(new { success = true, message = "Muestra borrada" });
        }
        [HttpPost]
        public IActionResult DeleteMultiple([FromBody] int[] itemIds)
        {
            try
            {
                _muestraRepository.DeleteMultiple(itemIds);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al borrar muestras. Detalle: {ex.Message}" });
            }
            return Json(new { success = true, message = "Muestras borradas" });
        }

        // graficas
        public IActionResult FormGraficaCampoYTipoMuestra()
        {

            IEnumerable<TiposMuestra> tiposMuestra = _tipoMuestraRepository.GetAll();
            IEnumerable<Campo> campos = _campoRepository.GetAll();
            TipoMuestraCampoVM tipoMuestraCampoVM = new TipoMuestraCampoVM()
            {
                TiposMuestra = tiposMuestra,
                Campos = campos
            };

            return View(tipoMuestraCampoVM);
        }
        [HttpPost]
        public IActionResult GraficaPorCampoYTipoMuestra(int idCampo, int idTipoMuestra)
        {

            ValoresPorCampoYTipoMuestraVM valoresPorCampoYTipoMuestraVM = _muestraRepository.GetValoresPorCampoYTipoMuestra(idCampo, idTipoMuestra);

            return View(valoresPorCampoYTipoMuestraVM);
        }
        [HttpPost]
        public ActionResult DatosGraficaPorCampoYTipoMuestra(int idCampo, int idTipoMuestra)
        {
            ValoresPorCampoYTipoMuestraVM valoresPorCampoYTipoMuestraVM = _muestraRepository.GetValoresPorCampoYTipoMuestra(idCampo, idTipoMuestra);
            return Json(new { valoresPorCampoYTipoMuestraVM });
        }

        public IActionResult ObtenerNombresVariables(int idTipoMuestra)
        {

            IEnumerable<NombresVariablesMuestra> nombreVariableMuestra = _muestraRepository.ObtenerNombresVariablesMuestra(idTipoMuestra);
            IEnumerable<NombreVariableMuestraDto> nombreVariableMuestraDto = nombreVariableMuestra.Select(t => _mapper.Map<NombreVariableMuestraDto>(t));

            return Json(new { Data = nombreVariableMuestraDto });
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult ObtenerDatosEvolucion(DatosEvolucionInDto datosEvolucion)
        {

            DatosEvolucionOutDto datosEvolucionOutDto = _muestraRepository.ObtenerDatosEvolucion(datosEvolucion);

            return Json(new { Data = datosEvolucionOutDto.Data, ValoresReferencia = datosEvolucionOutDto.ValoresReferencia });
            //return Json(datosEvolucionOutDto);


        }
    }


}
