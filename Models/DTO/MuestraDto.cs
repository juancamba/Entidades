namespace Entidades.Models.DTO
{
    public class MuestraDto
    {
        public string IdEntidad { get; set; }
        public string IdCampo { get; set; }
        public string IdTipoMuestra { get; set; }
        public string FechaMuestra { get; set; }


        public List<string> ValoresVariablesMuestras { get; set; }
    }
}
