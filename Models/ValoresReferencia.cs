namespace Entidades.Models
{
    public class ValoresReferencia
    {
        public int Id { get; set; }
        public int? IdNombreVariableMuestra { get; set; }

        public string? Maximo { get; set; }
        public string? Minimo { get; set; }
        public virtual NombresVariablesMuestra? NombreVariableMuestraNavigation { get; set; }
    }
}
