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
    public class EntidadesController : BaseController
    {

        private readonly AppDbContext _entidadesContext;
        private readonly IEntidadesRepository _entidadesRepository;
        private readonly IFicheroEntidadService _ficheroEntidadService;
        private readonly ICampoRepository _campoRepository;
        private readonly ITipoMuestraRepository _tipoMuestraRepository;
        private readonly IMuestraRepository _muestraRepository;
        private readonly IMapper _mapper;

        public EntidadesController(IEntidadesRepository entidadesRepository, IFicheroEntidadService ficheroEntidadService, ICampoRepository campoRepository, ITipoMuestraRepository tipoMuestraRepository, IMuestraRepository muestraRepository, IMapper mapper)
        {
            _entidadesRepository = entidadesRepository;
            _ficheroEntidadService = ficheroEntidadService;
            _campoRepository = campoRepository;
            _tipoMuestraRepository = tipoMuestraRepository;
            _muestraRepository = muestraRepository;
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

                ConjuntoEntidad conjuntoEntidad = _ficheroEntidadService.Cargar(file);
                //_muestraRepository.AltaConjuntoMuestra(conjuntoMuestra);
                _entidadesRepository.AltaConjuntoEntidad(conjuntoEntidad);
                Alert($"archivo cargado: {file.FileName} correctamente", NotificationType.success);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Alert($"Error al cargar el archivo: {ex.Message}", NotificationType.error);
            }


            return View();

        }


        public IActionResult GetAll()
        {
            return Json(new { Data = _entidadesRepository.GetAll() });
        }

        public IActionResult Ver(string id)
        {

            IEnumerable<Campo> campos = _campoRepository.GetAll();
            IEnumerable<TiposMuestra> tiposMuestras = _tipoMuestraRepository.GetAll();

            CampoTipoMuestraPrimerCoinidencia campoTipoMuestraPrimerCoinidencia = _entidadesRepository.GetIdCampoIdTipoMuestra(id);

            IEnumerable<CampoDto> camposDto = campos.Select(campo => _mapper.Map<CampoDto>(campo));

            


            IEnumerable<TipoMuestraDto> tiposMuestrasDto = tiposMuestras.Select(tipoMuestra => _mapper.Map<TipoMuestraDto>(tipoMuestra)) ;
            EntidadVM entidadVM = new EntidadVM()
            {
                Id = id,
                EntidadDetalleDto = _entidadesRepository.GetDetalle(id),
                CamposDto = camposDto,
                TiposMuestraDto = tiposMuestrasDto,
                CampoTipoMuestraPrimerCoinidencia = campoTipoMuestraPrimerCoinidencia

            };


            return View(entidadVM);
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            try
            {
                var objFromDeb = _entidadesRepository.GetById(id);
                if (objFromDeb == null)
                {
                    return Json(new { success = false, message = "Error borrando Entidad" });
                }
                _entidadesRepository.Delete(id);
                _muestraRepository.EliminarNombresVariablesSiNoHayMuestras();

                return Json(new { success = true, message = "Entidad borrada" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error borrando Entidad" });
            }
        }
        [HttpPost]
        public IActionResult DeleteMultiple([FromBody] string[] itemIds)
        {
            try
            {
                _entidadesRepository.DeleteMultiple(itemIds);
                _muestraRepository.EliminarNombresVariablesSiNoHayMuestras();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al borrar entidades. Detalle: {ex.Message}" });
            }
            return Json(new { success = true, message = "Entidades borradas" });
        }
    }
}
