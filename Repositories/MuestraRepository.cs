using Entidades.Models;
using Entidades.Models.DTO;
using Entidades.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
                        int IdTipoMuestra = Convert.ToInt32(conjuntoMuestra.NombreVariable.IdTipoMuestra);
                        if (_dbContext.TiposMuestras.Find(IdTipoMuestra) == null)
                        {
                            throw new InvalidDataException($"No existe el tipo de muestra {IdTipoMuestra}");
                        }
                        nombresVariablesMuestra = new NombresVariablesMuestra();
                        nombresVariablesMuestra.Nombre = nombreVariable;
                        nombresVariablesMuestra.IdTipoMuestra = IdTipoMuestra;
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

                    int idCampo = Convert.ToInt32(muestraDto.IdCampo);
                    if (_dbContext.Campos.Find(idCampo) == null)
                    {
                        throw new InvalidDataException($"No existe el idcampo {idCampo}");
                    }
                    string idEntidad = muestraDto.IdEntidad;
                    if (_dbContext.Entidades.Find(idEntidad) == null || string.IsNullOrEmpty(idEntidad))
                    {
                        throw new InvalidDataException($"idEntidad {idEntidad} no valido");
                    }
                    Muestra muestra = new Muestra();
                    muestra.IdTipoMuestra = Convert.ToInt32(muestraDto.IdTipoMuestra);
                    muestra.IdEntidad = idEntidad;
                    muestra.IdCampo = idCampo;
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
            catch (InvalidDataException)
            {
                transaction.Rollback();
                throw;
            }
            catch (Exception ex)
            {

                transaction.Rollback();
                throw new InvalidDataException($"Error al cargar el archivo. {ex.Message}");
            }




            return string.Empty;
        }

        public void Delete(int id)
        {

            _dbContext.Muestras.Remove(GetById(id));
            _dbContext.SaveChanges();
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

        public Muestra GetById(int id)
        {
            return _dbContext.Muestras.FirstOrDefault(p => p.Id == id);
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

        public MuestrasYValoresDto GetMuestrasYValores(int idTipoMuestra)
        {

            var muestrasDB = _dbContext.Muestras
                .Where(m => m.IdTipoMuestra == idTipoMuestra)
               .Include(m => m.ValoresVariablesMuestras)
               .ThenInclude(v => v.IdNombreVariableMuestraNavigation)
               .ToList();
            MuestrasYValoresDto muestraYValoresDtos = new MuestrasYValoresDto();


            foreach (var item in muestrasDB)
            {
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("IdMuestra", item.Id.ToString());
                keyValuePairs.Add("IdEntidad", item.IdEntidad.ToString());
                keyValuePairs.Add("Fecha", item.Fecha.ToString());
                foreach (var valor in item.ValoresVariablesMuestras)
                {
                    keyValuePairs.Add(valor.IdNombreVariableMuestraNavigation.Nombre, valor.Valor);
                }
                muestraYValoresDtos.listaMuestrasSalida.Add(keyValuePairs);
            }

            //obtenemos los nombres de las variables de tipo de muestra x
            var query2 = _dbContext.NombresVariablesMuestras
                .Where(n => n.IdTipoMuestra == idTipoMuestra)
                .Select(t => t.Nombre)

                .Distinct()
                .OrderBy(t => t)
                .ToList();

            muestraYValoresDtos.NombresVariables = query2;

            return muestraYValoresDtos;
        }


        public ValoresPorCampoYTipoMuestraVM GetValoresPorCampoYTipoMuestra(int idCampo, int idTipoMuestra)
        {


            //ValoresPorCampoYTipoMuestraVM 
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

            var valores = query.ToList();

            var media = valores
                 .GroupBy(dto => dto.Nombre)
                 .Select(grupo => new MuestraSalidaDto
                 {
                     Nombre = grupo.Key,
                     Valor = grupo.Average(dto => dto.Valor)
                 });

            var minimo = valores
                .GroupBy(dto => dto.Nombre)
                .Select(grupo => new MuestraSalidaDto
                {
                    Nombre = grupo.Key,
                    Valor = grupo.Min(dto => dto.Valor)
                });

            var max = valores
                .GroupBy(dto => dto.Nombre)
                .Select(grupo => new MuestraSalidaDto
                {
                    Nombre = grupo.Key,
                    Valor = grupo.Max(dto => dto.Valor)
                });

            int cantidadMuestras = _dbContext.Muestras.Where(x => x.IdCampo == idCampo && x.IdTipoMuestra == idTipoMuestra).Count();


            ValoresPorCampoYTipoMuestraVM valoresPorCampoYTipoMuestraVM = new ValoresPorCampoYTipoMuestraVM()
            {
                Campo = _dbContext.Campos.FirstOrDefault(p => p.Id == idCampo),
                TipoMuestra = _dbContext.TiposMuestras.FirstOrDefault(p => p.Id == idTipoMuestra),
                Media = media,
                Minimo = minimo,
                Maximo = max,
                CantidadMuestras = cantidadMuestras

            };

            return valoresPorCampoYTipoMuestraVM;
        }

        public IEnumerable<NombresVariablesMuestra> ObtenerNombresVariablesMuestra(int idTipoMuestra)
        {
            IEnumerable<NombresVariablesMuestra> nombreVariablesMuestra = _dbContext.NombresVariablesMuestras.Where(n => n.IdTipoMuestra == idTipoMuestra).ToList();

            return nombreVariablesMuestra;
        }



    }
}
