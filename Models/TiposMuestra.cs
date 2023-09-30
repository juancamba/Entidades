using System;
using System.Collections.Generic;

namespace Entidades.Models
{
    public partial class TiposMuestra
    {
        public TiposMuestra()
        {
            Muestras = new HashSet<Muestra>();
            NombresVariablesMuestras = new HashSet<NombresVariablesMuestra>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }

        public virtual ICollection<Muestra> Muestras { get; set; }
        public virtual ICollection<NombresVariablesMuestra> NombresVariablesMuestras { get; set; }
    }
}
