using Entidades.Models.DTO;

namespace Entidades.Serivces.Ficheros
{
    public interface IFicheroValoresReferenciaService
    {
        List<FicheroValorReferenciaDto> Cargar(IFormFile file);
    }
}