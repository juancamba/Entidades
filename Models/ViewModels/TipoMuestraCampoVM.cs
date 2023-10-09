namespace Entidades.Models.ViewModels
{
    public class TipoMuestraCampoVM
    {
        public IEnumerable<TiposMuestra> TiposMuestra { get; set; }
        public IEnumerable<Campo> Campos { get; set; }
    }
}
