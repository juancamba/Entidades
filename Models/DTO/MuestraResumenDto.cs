using System.ComponentModel;

namespace Entidades.Models.DTO
{
    public class MuestraResumenDto
    {
        [DisplayName("Id")]
        public int Id { get; set; }

        [DisplayName("IdEntidad")]
        public string IdEntidad { get; set; } = string.Empty;
        [DisplayName("Fecha")]
        public DateTime? Fecha { get; set; }
        [DisplayName("NombreCampo")]
        public string NombreCampo { get; set; } = string.Empty;
        [DisplayName("TipoMuestra")]
        public string TipoMuestra { get; set; } = string.Empty;




    }
}
