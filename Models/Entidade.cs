using System;
using System.Collections.Generic;

namespace Entidades.Models
{
    public partial class Entidade
    {
        public Entidade()
        {
            Muestras = new HashSet<Muestra>();
            ValoresDatosEstaticos = new HashSet<ValoresDatosEstatico>();
        }

        public string Id { get; set; } = null!;
        public DateTime FechaAlta { get; set; }

        public virtual ICollection<Muestra> Muestras { get; set; }
        public virtual ICollection<ValoresDatosEstatico> ValoresDatosEstaticos { get; set; }
    }
}
