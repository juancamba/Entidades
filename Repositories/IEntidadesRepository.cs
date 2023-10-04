using Entidades.Models;
using Entidades.Models.DTO;

namespace Entidades.Repositories
{
    public interface IEntidadesRepository
    {
        public IEnumerable<EntidadResumenDto> GetAll();

        public IEnumerable<EntidadDetalleDto> GetDetalle(string id);
        public void AltaConjuntoEntidad(ConjuntoEntidad conjuntoEntidad);

        public void Delete(string id);
        public Entidade GetById(string id);

        public IEnumerable<AgrupadoEntidadCantidadMuestras> CantidadMuestrasPorEntidadCampoYTipoMuestra();

    }


}
