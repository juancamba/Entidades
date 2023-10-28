using System;
using System.Collections.Generic;

namespace Entidades.Models
{
    public partial class ValoresVariablesMuestra
    {
        public int Id { get; set; }
        public int IdNombreVariableMuestra { get; set; }
        public int IdMuestra { get; set; }
        public string? Valor { get; set; }

        public virtual Muestra IdMuestraNavigation { get; set; } = null!;
        public virtual NombresVariablesMuestra IdNombreVariableMuestraNavigation { get; set; } = null!;
    }
}
