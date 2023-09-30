﻿using System;
using System.Collections.Generic;

namespace Entidades.Models
{
    public partial class ValoresDatosEstatico
    {
        public int Id { get; set; }
        public string? Valor { get; set; }
        public string? IdEntidad { get; set; }
        public int? IdNombreDatoEstatico { get; set; }

        public virtual Entidade? IdEntidadNavigation { get; set; }
        public virtual NombresDatosEstatico? IdNombreDatoEstaticoNavigation { get; set; }
    }
}
