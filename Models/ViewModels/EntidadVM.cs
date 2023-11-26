using Entidades.Models.DTO;

namespace Entidades.Models.ViewModels
{
    public class EntidadVM
    {
        public string Id { get; set; } = string.Empty;
        public IEnumerable<EntidadDetalleDto> EntidadDetalleDto { get; set; }
        public IEnumerable<TipoMuestraDto> TiposMuestraDto { get; set; }
        public IEnumerable<CampoDto> CamposDto { get; set; }

        public CampoTipoMuestraPrimerCoinidencia CampoTipoMuestraPrimerCoinidencia { get; set; }
    }
}
