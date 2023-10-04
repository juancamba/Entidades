using Entidades.Models.DTO;
using Entidades.Models.ViewModels;
using Entidades.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Entidades.Controllers
{
    public class InformesController : Controller
    {
        private readonly IMuestraRepository _muestraRepository;
        private readonly IEntidadesRepository _entidadesRepository;

        public InformesController(IEntidadesRepository entidadesRepository)
        {
            _entidadesRepository = entidadesRepository;

        }


        //informe 1
        public IActionResult ListaEntidades()
        {

            IEnumerable<EntidadResumenDto> entidades = _entidadesRepository.GetAll();

            ListaEntidadesVM listaEntidadesVM = new ListaEntidadesVM
            {
                ListaEntidades = entidades
            };
            return View(listaEntidadesVM);
        }

        public IActionResult CantidadMuestrasPorCampo()
        {
            IEnumerable<AgrupadoEntidadCantidadMuestras> entidades = _entidadesRepository.CantidadMuestrasPorEntidadCampoYTipoMuestra();

            CantidadMuestrasPorCampoVM cantidadMuestrasPorCampoVM = new CantidadMuestrasPorCampoVM
            {
                CantidadMuestrasPorCampo = entidades
            };
            return View(cantidadMuestrasPorCampoVM);

        }

    }
}
