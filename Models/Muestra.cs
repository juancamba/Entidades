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
        public string? IdEntidad { get; set; }
        public int? IdTipoMuestra { get; set; }
        public int? IdCampo { get; set; }
        public DateTime? Fecha { get; set; }

        public virtual Campo? IdCampoNavigation { get; set; }
        public virtual Entidade? IdEntidadNavigation { get; set; }
        public virtual TiposMuestra? IdTipoMuestraNavigation { get; set; }
        public virtual ICollection<ValoresVariablesMuestra> ValoresVariablesMuestras { get; set; }
    }
}
