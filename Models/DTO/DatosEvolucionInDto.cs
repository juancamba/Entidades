using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades.Models.DTO
{
    public class DatosEvolucionInDto
    {
        [Required]
        public string IdEntidad { get; set; }


        [Required(ErrorMessage = "Fecha desde no puede ser vacio")]
        [NotMapped] // Evita que Entity Framework intente mapear esta propiedad a una columna en la base de datos
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)] // Formato de fecha en español
        [DataType(DataType.Date)]
        public DateTime FechaDesde { get; set; }


        [Required(ErrorMessage = "Fecha hasta no puede ser vacio")]
        [NotMapped] // Evita que Entity Framework intente mapear esta propiedad a una columna en la base de datos
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)] // Formato de fecha en español
        [DataType(DataType.Date)]
        public DateTime FechaHasta { get; set; }
        [Required]
        public int IdCampo { get; set; }
        [Required]
        public int IdTipoMuestra { get; set; }


        public List<int> Variables { get; set; }
    }
}
