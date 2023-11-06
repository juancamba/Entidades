using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Entidades.Models
{
    [Index(nameof(IdNombreVariableMuestra), IsUnique = true)]
    public class ValoresReferencia
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Tipo de muestra")]
        public int IdNombreVariableMuestra { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string? Maximo { get; set; } = "0";
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string? Minimo { get; set; } = "0";
        public virtual NombresVariablesMuestra NombreVariableMuestra { get; set; } = null!;
    }
}
