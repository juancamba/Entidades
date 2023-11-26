using Entidades.Models;
using Entidades.Models.DTO;

namespace Entidades.Repositories
{
    public interface IValoresReferenciaRepository
    {

        bool SaveChanges();

        IEnumerable<ValoresReferencia> GetAll();

        ValoresReferencia GetById(int id);
        void Create(ValoresReferencia valoresReferencia);

        void Update(ValoresReferencia valoresReferencia);
        void Delete(int id);
        public IEnumerable<NombresVariablesMuestra> ObtenerVariablesSinValoresReferencia(int idTipoMuestra);
        public void AltaMasiva(List<FicheroValorReferenciaDto> ficheroValorReferenciaDto);
        public void DeleteMultiple(int[] ids);
    }
}
