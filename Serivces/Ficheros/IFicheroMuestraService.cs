using Entidades.Models.DTO;

namespace Entidades.Services.Ficheros
{
    public interface IFicheroMuestraService
    {
        public ConjuntoMuestra Cargar(IFormFile file);

    }
}
