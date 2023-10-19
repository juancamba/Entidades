using Entidades.Models.DTO;

namespace Entidades.Models.ViewModels
{
    public class ValoresReferenciaVM
    {
        public IEnumerable<TipoMuestraDto> TiposMuestraDto { get; set; }

        public ValoresReferencia ValoresReferencia { get; set; }
    }
}
