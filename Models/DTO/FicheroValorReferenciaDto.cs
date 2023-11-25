namespace Entidades.Models.DTO
{
    public class FicheroValorReferenciaDto
    {
        public string Variable { get; set; }

        public string? Minimo { get; set; } = "0";
        public string? Maximo { get; set; } = "0";
    }
}
