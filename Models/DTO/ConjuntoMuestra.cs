namespace Entidades.Models.DTO
{
    public class ConjuntoMuestra
    {
        //  public List<string> NombresVariablesMuestras { get; set; }
        public NombreVariableTipoMuestra NombreVariable { get; set; }
        public List<MuestraDto> Muestras { get; set; }
    }
}
