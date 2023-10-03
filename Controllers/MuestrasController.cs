using Entidades.Models;
using Entidades.Models.DTO;
using Entidades.Models.ViewModels;
using Entidades.Repositories;
using Entidades.Services.Ficheros;
using Microsoft.AspNetCore.Mvc;
using static Entidades.Models.Enum;

namespace Entidades.Controllers
{
    public class MuestrasController : BaseController
    {


        private readonly AppDbContext _entidadesContext;
        private readonly IFicheroMuestraService _ficheroMuestraService;
        private readonly IMuestraRepository _muestraRepository;

        public MuestrasController(AppDbContext entidadesContext, IFicheroMuestraService ficheroMuestraService, IMuestraRepository muestraRepository)
        {
            _entidadesContext = entidadesContext;
            _ficheroMuestraService = ficheroMuestraService;
            _muestraRepository = muestraRepository;


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

        // graficas
        public IActionResult FormGraficaCampoYTipoMuestra()
        {
            return View();
        }
        public IActionResult GraficaPorCampoYTipoMuestra()
        {

            IEnumerable<MuestraSalidaDto> valores = _muestraRepository.GetValoresPorCampoYTipoMuestra(1, 13);

            var media = valores
            .GroupBy(dto => dto.Nombre)
            .Select(grupo => new MuestraSalidaDto
            {
                Nombre = grupo.Key,
                Valor = grupo.Average(dto => dto.Valor)
            });

            var minimo = valores
            .GroupBy(dto => dto.Nombre)
            .Select(grupo => new MuestraSalidaDto
            {
                Nombre = grupo.Key,
                Valor = grupo.Min(dto => dto.Valor)
            });

            var max = valores
    .GroupBy(dto => dto.Nombre)
    .Select(grupo => new MuestraSalidaDto
    {
        Nombre = grupo.Key,
        Valor = grupo.Max(dto => dto.Valor)
    });


            ValoresPorCampoYTipoMuestraVM valoresPorCampoYTipoMuestraVM = new ValoresPorCampoYTipoMuestraVM()
            {
                Campo = _entidadesContext.Campos.FirstOrDefault(p => p.Id == 1),
                TipoMuestra = _entidadesContext.TiposMuestras.FirstOrDefault(p => p.Id == 13),
                Media = media,
                Minimo = minimo,
                Maximo = max,

            };

            return View(valoresPorCampoYTipoMuestraVM);
        }
    }
}
