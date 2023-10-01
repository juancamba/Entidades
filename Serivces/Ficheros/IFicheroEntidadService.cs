using Entidades.Models.DTO;

namespace Entidades.Serivces.Ficheros
{
    public interface IFicheroEntidadService
    {
        ConjuntoEntidad Cargar(IFormFile formFile);
    }
}