using System;
using System.Collections.Generic;

namespace Entidades.Models
{
    public partial class NombresVariablesMuestra
    {
        public NombresVariablesMuestra()
        {
            ValoresVariablesMuestras = new HashSet<ValoresVariablesMuestra>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int? IdTipoMuestra { get; set; }

        public virtual TiposMuestra? IdTipoMuestraNavigation { get; set; }
        public virtual ICollection<ValoresVariablesMuestra> ValoresVariablesMuestras { get; set; }
        public virtual ValoresReferencia? ValoresReferencia { get; set; }

    }
}
