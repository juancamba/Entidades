using Entidades.Models.DTO;

namespace Entidades.Models.ViewModels
{
    public class EntidadVM
    {
        public string Id { get; set; } = string.Empty;
        public IEnumerable<EntidadDetalleDto> EntidadDetalleDto { get; set; }
    }
}
