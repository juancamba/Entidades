using Entidades.Models;
using Entidades.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Entidades.Repositories
{
    public interface IMuestraRepository
    {
        public string AltaConjuntoMuestra(ConjuntoMuestra conjuntoMuestra);
        public IEnumerable<MuestraResumenDto> GetAll();

        public IEnumerable<MuestraDetalleDto> GetDetalle(int id);
        public IEnumerable<MuestraSalidaDto> GetValoresPorCampoYTipoMuestra(int idCampo, int idTipoMuestra);
    }
}
