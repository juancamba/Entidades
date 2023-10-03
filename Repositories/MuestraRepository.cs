using Entidades.Models;
using Entidades.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;

namespace Entidades.Repositories
{
    public class MuestraRepository : IMuestraRepository
    {
        private readonly AppDbContext _dbContext;
        readonly IConfiguration _configuration;
        public MuestraRepository(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
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


            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                foreach (string nombreVariable in conjuntoMuestra.NombreVariable.Nombres)
                {



                    //consulto si existen los nombre de las variables para el idTipoMuestra
                    NombresVariablesMuestra? nombresVariablesMuestra = _dbContext.NombresVariablesMuestras
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
                        _dbContext.NombresVariablesMuestras.Add(nombresVariablesMuestra);
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
                    try
                    {
                        muestra.Fecha = DateTime.ParseExact(muestraDto.FechaMuestra, _configuration.GetSection("General")["formatoFechaHora"], CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidDataException($"Fecha incorrecta : {muestraDto.FechaMuestra}, formato: {_configuration.GetSection("General")["formatoFechaHora"]}");
                    }

                    //ALTA VALORES MUESTRA

                    _dbContext.Muestras.Add(muestra);
                    _dbContext.SaveChanges();

                    for (int i = 0; i < muestraDto.ValoresVariablesMuestras.Count; i++)
                    {
                        int id = listaNombresVariablesMuestra[i].Id;
                        string valor = muestraDto.ValoresVariablesMuestras[i];
                        //idVariableValor.Add(id, valor);
                        ValoresVariablesMuestra valoresVariablesMuestra = new ValoresVariablesMuestra();
                        valoresVariablesMuestra.Valor = valor;
                        valoresVariablesMuestra.IdNombreVariableMuestra = id;
                        valoresVariablesMuestra.IdMuestra = muestra.Id;

                        _dbContext.ValoresVariablesMuestras.Add(valoresVariablesMuestra);

                    }
                    _dbContext.SaveChanges();
                }


                transaction.Commit();

            }
            catch (Exception ex)
            {

                transaction.Rollback();
                throw new InvalidDataException($"Error al cargar el archivo. {ex.Message}");
            }




            return string.Empty;
        }

        public IEnumerable<MuestraResumenDto> GetAll()
        {
            //return _dbContext.Muestras.ToList();

            var query = from m in _dbContext.Muestras
                        join t in _dbContext.TiposMuestras on m.IdTipoMuestra equals t.Id
                        join c in _dbContext.Campos on m.IdCampo equals c.Id
                        select new MuestraResumenDto
                        {
                            Id = m.Id,
                            IdEntidad = m.IdEntidad,
                            Fecha = m.Fecha,
                            NombreCampo = c.Nombre,
                            TipoMuestra = t.Nombre,



                        };

            return query.ToList();
        }
        public IEnumerable<MuestraDetalleDto> GetDetalle(int id)
        {
            var query = from m in _dbContext.Muestras
                        join v in _dbContext.ValoresVariablesMuestras on m.Id equals v.IdMuestra
                        join n in _dbContext.NombresVariablesMuestras on v.IdNombreVariableMuestra equals n.Id
                        where m.Id == id
                        select new MuestraDetalleDto
                        {
                            Nombre = n.Nombre,
                            Valor = v.Valor,
                        };

            return query.ToList();

        }

        public IEnumerable<MuestraSalidaDto> GetValoresPorCampoYTipoMuestra(int idCampo, int idTipoMuestra)
        {
            var query = from m in _dbContext.Muestras
                        join v in _dbContext.ValoresVariablesMuestras on m.Id equals v.IdMuestra
                        join n in _dbContext.NombresVariablesMuestras on v.IdNombreVariableMuestra equals n.Id
                        where m.IdCampo == idCampo && m.IdTipoMuestra == idTipoMuestra
                        //group v.Valor by n.Nombre into g
                        select new MuestraSalidaDto
                        {
                            Nombre = n.Nombre,
                            Valor = Convert.ToDouble(v.Valor),
                        };

            return query.ToList();
        }

    }
}
