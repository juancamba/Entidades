using AutoMapper;
using Entidades.Models;
using Entidades.Models.DTO;
using Entidades.Models.ViewModels;
using Entidades.Repositories;
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

            // un objtos con los valores de las variables de la muestra y el nombre de la variable
            // un ojeto con los datos dea muestra
            // otro podria ser con los datos de referencia

            var query = from m in _dbContext.Muestras
                        join v in _dbContext.ValoresVariablesMuestras on m.Id equals v.IdMuestra
                        join n in _dbContext.NombresVariablesMuestras on v.IdNombreVariableMuestra equals n.Id
                        where m.IdCampo == datosEvolucion.IdCampo &&
                            m.IdTipoMuestra == datosEvolucion.IdTipoMuestra &&
                            m.IdEntidad == datosEvolucion.IdEntidad &&
                            datosEvolucion.Variables.Contains(n.Id)
                        orderby m.Fecha ascending
                        select new
                        {
                            Nombre = n.Nombre,
                            Valor = Convert.ToDouble(v.Valor),
                            Fecha = m.Fecha,
                            IdVariable = n.Id,
                            NombreVariable = n.Nombre
                        };

            Dictionary<string, object> vars = new Dictionary<string, object>();

            if (query.Count() > 0)
            {
                var nombreVariables = query.Select(x => x.NombreVariable).Distinct().ToList();



                var fechas = query.Select(x => x.Fecha).ToList();

                foreach (var item in nombreVariables)
                {
                    var datos = query.Where(x => x.NombreVariable == item).ToList();
                    vars.Add(item, datos);
                }

                //intento obtener los valroes de referencia
                var valoresReferenciaAux = from vr in _dbContext.ValoresReferencia
                                           join n in _dbContext.NombresVariablesMuestras on vr.IdNombreVariableMuestra equals n.Id
                                           where nombreVariables.Contains(n.Nombre)
                                           select new
                                           {
                                               Nombre = n.Nombre,
                                               Minimo = vr.Minimo,
                                               Maximo = vr.Maximo
                                           };
                Dictionary<string, object> valoresReferencia = new Dictionary<string, object>();
                foreach (var item in valoresReferenciaAux)
                {
                    var datos = valoresReferenciaAux.Where(x => x.Nombre == item.Nombre).ToList();
                    valoresReferencia.Add(item.Nombre, datos);
                }


                return Json(new { Data = vars, ValoresReferencia = valoresReferencia });
            }



            return Json(new { Data = vars });


        }
    }


}
