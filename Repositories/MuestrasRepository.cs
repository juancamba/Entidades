using Entidades.Models;
using Entidades.Models.DTO;
using System.Globalization;
using System.Linq;

namespace Entidades.Repositories
{
    public class MuestrasRepository : IMuestraRepository
    {

        private readonly AppDbContext _entidadesContext;
        readonly IConfiguration _configuration;
        public MuestrasRepository(AppDbContext entidadesContext, IConfiguration configuration)
        {
            _entidadesContext = entidadesContext;
            _configuration = configuration;
        }


        /// <summary>
        /// 
        /// La entidad, el campo, tipo de muestra debe existir, en una transaccion
        /// 
        /// Si existe crea los registros en este orden
        /// 
        /// crea muestra
        /// crea nombresVariablesMuestras
        /// crea valoresVariablesMuestras
        /// 
        /// 
        /// </summary>
        /// <param name="conjuntoMuestra"></param>
        /// <returns></returns>       
        public string AltaConjuntoMuestra(ConjuntoMuestra conjuntoMuestra)
        {
            List<NombresVariablesMuestra> listaNombresVariablesMuestra = new List<NombresVariablesMuestra>();


            using var transaction = _entidadesContext.Database.BeginTransaction();
            try
            {
                foreach (string nombreVariable in conjuntoMuestra.NombreVariable.Nombres)
                {



                    //consulto si existen los nombre de las variables para el idTipoMuestra
                    NombresVariablesMuestra? nombresVariablesMuestra = _entidadesContext.NombresVariablesMuestras
                        .Where(n => n.Nombre == nombreVariable && n.IdTipoMuestra == Convert.ToInt32(conjuntoMuestra.NombreVariable.IdTipoMuestra))
                        .FirstOrDefault();
                    if (nombresVariablesMuestra != null)
                    {
                        // agregamos al listado, para llevar control luego
                        listaNombresVariablesMuestra.Add(nombresVariablesMuestra);

                    }
                    else
                    {
                        //agregar a base de datos
                        nombresVariablesMuestra = new NombresVariablesMuestra();
                        nombresVariablesMuestra.Nombre = nombreVariable;
                        nombresVariablesMuestra.IdTipoMuestra = Convert.ToInt32(conjuntoMuestra.NombreVariable.IdTipoMuestra);
                        _entidadesContext.NombresVariablesMuestras.Add(nombresVariablesMuestra);
                        listaNombresVariablesMuestra.Add(nombresVariablesMuestra);
                    }

                    //string idNombreVariable = CrearNombreVariableMuestraSiNoExiste(nombreVariable, conjuntoMuestra.NombreVariable.IdTipoMuestra);
                    //nombresVariblesConId.Add(idNombreVariable, nombreVariable);
                    //listaIdNombreVariable.Add(idNombreVariable);

                }


                foreach (MuestraDto muestraDto in conjuntoMuestra.Muestras)
                {
                    //alta muestra
                    Muestra muestra = new Muestra();
                    muestra.IdTipoMuestra = Convert.ToInt32(muestraDto.IdTipoMuestra);
                    muestra.IdEntidad = muestraDto.IdEntidad;
                    muestra.IdCampo = Convert.ToInt32(muestraDto.IdCampo);
                    muestra.Fecha = DateTime.ParseExact(muestraDto.FechaMuestra, _configuration.GetSection("General")["formatoFechaHora"], CultureInfo.InvariantCulture);
                    //ALTA VALORES MUESTRA

                    _entidadesContext.Muestras.Add(muestra);
                    _entidadesContext.SaveChanges();

                    for (int i = 0; i < muestraDto.ValoresVariablesMuestras.Count; i++)
                    {
                        int id = listaNombresVariablesMuestra[i].Id;
                        string valor = muestraDto.ValoresVariablesMuestras[i];
                        //idVariableValor.Add(id, valor);
                        ValoresVariablesMuestra valoresVariablesMuestra = new ValoresVariablesMuestra();
                        valoresVariablesMuestra.Valor = valor;
                        valoresVariablesMuestra.IdNombreVariableMuestra = id;
                        valoresVariablesMuestra.IdMuestra = muestra.Id;

                        _entidadesContext.ValoresVariablesMuestras.Add(valoresVariablesMuestra);

                    }
                    _entidadesContext.SaveChanges();
                }


                transaction.Commit();

            }
            catch (Exception ex)
            {

                transaction.Rollback();
            }




            return string.Empty;
        }
    }
}
