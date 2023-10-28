using Entidades.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace Entidades.Models
{
    public partial class NombresVariablesMuestra
    {
        public NombresVariablesMuestra()
        {
            ValoresReferencia = new HashSet<ValoresReferencia>();
            ValoresVariablesMuestras = new HashSet<ValoresVariablesMuestra>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int IdTipoMuestra { get; set; }

        public virtual TiposMuestra IdTipoMuestraNavigation { get; set; } = null!;
        public virtual ICollection<ValoresReferencia> ValoresReferencia { get; set; }
        public virtual ICollection<ValoresVariablesMuestra> ValoresVariablesMuestras { get; set; }

    }
}
