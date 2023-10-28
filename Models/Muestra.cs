using System;
using System.Collections.Generic;

namespace Entidades.Models
{
    public partial class Muestra
    {
        public Muestra()
        {
            ValoresVariablesMuestras = new HashSet<ValoresVariablesMuestra>();
        }
        public int Id { get; set; }
        public string IdEntidad { get; set; } = null!;
        public int IdTipoMuestra { get; set; }
        public int IdCampo { get; set; }
        public DateTime Fecha { get; set; }

        public virtual Campo IdCampoNavigation { get; set; } = null!;
        public virtual Entidade IdEntidadNavigation { get; set; } = null!;
        public virtual TiposMuestra IdTipoMuestraNavigation { get; set; } = null!;
        public virtual ICollection<ValoresVariablesMuestra> ValoresVariablesMuestras { get; set; }
    }
}
