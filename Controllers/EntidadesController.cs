using Entidades.Models;
using Microsoft.AspNetCore.Mvc;

namespace Entidades.Controllers
{
    public class EntidadesController : Controller
    {

        private readonly AppDbContext _entidadesContext;

        public EntidadesController(AppDbContext entidadesContext)
        {
            _entidadesContext = entidadesContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        // cargar entidades por ficher
        // mostrar entidades
        // eliminar entidad ? (eliminar todas las muestras asociadas)
    }
}
