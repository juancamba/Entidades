using Entidades.Models.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Entidades.Models.ViewModels
{
    public class ValoresReferenciaVM
    {
        //public IEnumerable<TipoMuestraDto> TiposMuestraDto { get; set; }
        public IEnumerable<SelectListItem> Items { get; set; }
        public ValoresReferencia ValoresReferencia { get; set; }
    }
}
