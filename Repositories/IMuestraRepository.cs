using Entidades.Models;
using Entidades.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Entidades.Repositories
{
    public interface IMuestraRepository
    {
        public string AltaConjuntoMuestra(ConjuntoMuestra conjuntoMuestra);
    }
}
