using Entidades.Models;
using Entidades.Models.DTO;
using Entidades.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Entidades.Repositories
{
    public interface IMuestraRepository
    {
        public string AltaConjuntoMuestra(ConjuntoMuestra conjuntoMuestra);
        public IEnumerable<MuestraResumenDto> GetAll();

        public IEnumerable<MuestraDetalleDto> GetDetalle(int id);


        public ValoresPorCampoYTipoMuestraVM GetValoresPorCampoYTipoMuestra(int idCampo, int idTipoMuestra);

        public MuestrasYValoresDto GetMuestrasYValores(int idTipoMuestra);

        public IEnumerable<NombresVariablesMuestra> ObtenerNombresVariablesMuestra(int idTipoMuestra);

        public Muestra GetById(int id);
        public void Delete(int id);

        public DatosEvolucionOutDto ObtenerDatosEvolucion(DatosEvolucionInDto datosEvolucion);

    }
}
