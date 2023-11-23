using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entidades.Models
{
    public partial class TiposMuestra
    {
        public TiposMuestra()
        {
            Muestras = new HashSet<Muestra>();
            NombresVariablesMuestras = new HashSet<NombresVariablesMuestra>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Muestra> Muestras { get; set; }
        public virtual ICollection<NombresVariablesMuestra> NombresVariablesMuestras { get; set; }
    }
}
