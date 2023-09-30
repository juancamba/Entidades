using Entidades.Models.DTO;

namespace Entidades.Models.ViewModels
{
    public class MuestraVM
    {

        public MuestraResumenDto MuestraResumenDto { get; set; }
        public IEnumerable<MuestraDetalleDto> MuestraDetalleDto { get; set; }
    }
}
