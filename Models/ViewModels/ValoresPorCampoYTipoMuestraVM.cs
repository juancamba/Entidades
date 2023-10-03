using Entidades.Models.DTO;

namespace Entidades.Models.ViewModels
{
    public class ValoresPorCampoYTipoMuestraVM
    {

        public Campo Campo { get; set; }
        public TiposMuestra TipoMuestra { get; set; }
        public IEnumerable<MuestraSalidaDto> Media { get; set; }
        public IEnumerable<MuestraSalidaDto> Minimo { get; set; }
        public IEnumerable<MuestraSalidaDto> Maximo { get; set; }
    }
}
