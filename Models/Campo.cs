using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Entidades.Models
{
    public partial class Campo
    {
        public Campo()
        {
            Muestras = new HashSet<Muestra>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre requerido")]
        [Display(Name = "Nombre del campo")]
        public string? Nombre { get; set; }

        public virtual ICollection<Muestra> Muestras { get; set; }
    }
}
