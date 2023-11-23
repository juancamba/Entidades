using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre requerido")]
        [Display(Name = "Nombre del campo")]
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Muestra> Muestras { get; set; }
    }
}
