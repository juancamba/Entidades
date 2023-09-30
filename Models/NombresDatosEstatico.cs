using System;
using System.Collections.Generic;

namespace Entidades.Models
{
    public partial class NombresDatosEstatico
    {
        public NombresDatosEstatico()
        {
            ValoresDatosEstaticos = new HashSet<ValoresDatosEstatico>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }

        public virtual ICollection<ValoresDatosEstatico> ValoresDatosEstaticos { get; set; }
    }
}
