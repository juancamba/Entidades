using Entidades.Models.DTO;
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
            return View(entidades);
        }
    }
}
