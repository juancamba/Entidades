﻿using Entidades.Models;
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
        private const int DECIMALES = 2;
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
                }
                _dbContext.SaveChanges();
                foreach (MuestraDto muestraDto in conjuntoMuestra.Muestras)
                {
                    //alta muestra

                    int idCampo = Convert.ToInt32(muestraDto.IdCampo);
                    int idMuestra = Convert.ToInt32(muestraDto.IdMuestra);

                    if (_dbContext.Campos.Find(idCampo) == null)
                    {
                        throw new InvalidDataException($"No existe el idcampo {idCampo}");
                    }
                    string idEntidad = muestraDto.IdEntidad;
                    if (_dbContext.Entidades.Find(idEntidad) == null || string.IsNullOrEmpty(idEntidad))
                    {
                        throw new InvalidDataException($"idEntidad {idEntidad} no valido");
                    }




                    DateTime fechaMuestra;
                    try
                    {
                        fechaMuestra = DateTime.ParseExact(muestraDto.FechaMuestra, _configuration.GetSection("General")["formatoFechaHora"], CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidDataException($"Fecha incorrecta : {muestraDto.FechaMuestra}, formato: {_configuration.GetSection("General")["formatoFechaHora"]}");
                    }

                    Muestra muestra = _dbContext.Muestras.Find(idMuestra);
                    if (muestra != null)
                    {
                        //Muestra muestra = new Muestra();
                        muestra.IdTipoMuestra = Convert.ToInt32(muestraDto.IdTipoMuestra);
                        muestra.IdEntidad = idEntidad;
                        muestra.IdCampo = idCampo;
                        muestra.Fecha = fechaMuestra;

                    }
                    else
                    {
                        muestra = new Muestra();
                        muestra.IdTipoMuestra = Convert.ToInt32(muestraDto.IdTipoMuestra);
                        muestra.IdEntidad = idEntidad;
                        muestra.IdCampo = idCampo;
                        muestra.Fecha = fechaMuestra;
                        muestra.Id = idMuestra;
                        _dbContext.Muestras.Add(muestra);
                    }
                    _dbContext.SaveChanges();

                    //ALTA VALORES MUESTRA

                    if (muestraDto.ValoresVariablesMuestras.Count != listaNombresVariablesMuestra.Count)
                    {
                        throw new InvalidDataException($"La cantidad de valores de variables no coincide con la cantidad de los nombres de las variables. nombres: {listaNombresVariablesMuestra.Count}, variables: {muestraDto.ValoresVariablesMuestras.Count}");
                    }
                    //borramos los valroes existentes porque los volvemos a crear con lo que viene en el archivo
                    _dbContext.ValoresVariablesMuestras.RemoveRange(_dbContext.ValoresVariablesMuestras.Where(v => v.IdMuestra == muestra.Id));
                    _dbContext.SaveChanges();

                    for (int i = 0; i < muestraDto.ValoresVariablesMuestras.Count; i++)
                    {
                        int id = listaNombresVariablesMuestra[i].Id;
                        string valor = muestraDto.ValoresVariablesMuestras[i];
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
            EliminarNombresVariablesSiNoHayMuestras();
        }

        public void DeleteMultiple(int[] ids)
        {
            _dbContext.Muestras.RemoveRange(_dbContext.Muestras.Where(p => ids.Contains(p.Id)));
            _dbContext.SaveChanges();
            EliminarNombresVariablesSiNoHayMuestras();
        }

        public void EliminarNombresVariablesSiNoHayMuestras()
        {
            //obtener nombres de variables que no tienen valores
            var idsAEliminar = _dbContext.NombresVariablesMuestras
                .Where(nv => !_dbContext.ValoresVariablesMuestras.Any(vv => vv.IdNombreVariableMuestra == nv.Id))
                .Select(nv => nv.Id)
                .ToList();

            // Eliminar las filas de nombresVariablesMuestras que cumplen con la condición
            var nombresVariablesAEliminar = _dbContext.NombresVariablesMuestras
                .Where(nv => idsAEliminar.Contains(nv.Id))
                .ToList();

            _dbContext.NombresVariablesMuestras.RemoveRange(nombresVariablesAEliminar);
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
                            Fecha = m.Fecha.ToString("yyyy-MM-dd"),
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
                    keyValuePairs.Add(valor.IdNombreVariableMuestraNavigation.Nombre, Math.Round(Convert.ToDecimal(valor.Valor), DECIMALES).ToString());
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
                     Valor = Math.Round(grupo.Average(dto => dto.Valor),DECIMALES)
                 });

            var minimo = valores
                .GroupBy(dto => dto.Nombre)
                .Select(grupo => new MuestraSalidaDto
                {
                    Nombre = grupo.Key,
                    Valor = Math.Round(grupo.Min(dto => dto.Valor), DECIMALES)
                });

            var max = valores
                .GroupBy(dto => dto.Nombre)
                .Select(grupo => new MuestraSalidaDto
                {
                    Nombre = grupo.Key,
                    Valor = Math.Round(grupo.Max(dto => dto.Valor), DECIMALES)
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

        public DatosEvolucionOutDto ObtenerDatosEvolucion(DatosEvolucionInDto datosEvolucion)
        {

            DatosEvolucionOutDto datosEvolucionOutDto = new();

            // un objtos con los valores de las variables de la muestra y el nombre de la variable
            // un ojeto con los datos dea muestra
            // otro podria ser con los datos de referencia

            var query = from m in _dbContext.Muestras
                        join v in _dbContext.ValoresVariablesMuestras on m.Id equals v.IdMuestra
                        join n in _dbContext.NombresVariablesMuestras on v.IdNombreVariableMuestra equals n.Id
                        where m.IdCampo == datosEvolucion.IdCampo &&
                            m.IdTipoMuestra == datosEvolucion.IdTipoMuestra &&
                            m.IdEntidad == datosEvolucion.IdEntidad &&
                            datosEvolucion.Variables.Contains(n.Id)

                        //  ((datosEvolucion.FechaDesde == null || m.Fecha >= datosEvolucion.FechaDesde) &&
                        //(datosEvolucion.FechaHasta == null || m.Fecha <= datosEvolucion.FechaHasta))


                        orderby m.Fecha ascending

                        select new
                        {
                            Nombre = n.Nombre,
                            Valor = Math.Round(Convert.ToDouble(v.Valor), DECIMALES),
                            Fecha = m.Fecha.ToString("MM/dd/yyyy"),
                            //Fecha = m.Fecha,
                            IdVariable = n.Id,
                            NombreVariable = n.Nombre
                        };

            //obtener de query la fecha


            var queryFiltramosVariables = query.Where(x => datosEvolucion.Variables.Contains(x.IdVariable)).ToList();

            Dictionary<string, object> vars = new Dictionary<string, object>();

            if (query.Count() > 0)
            {
                var nombreVariables = query.Select(x => x.NombreVariable).Distinct().ToList();

                var listaid = query.Select(x => x.IdVariable).Distinct().ToList();

                var fechas = query.Select(x => x.Fecha).ToList();




                foreach (var item in nombreVariables)
                {
                    var datos = query.Where(x => x.NombreVariable == item).ToList();
                    vars.Add(item, datos);
                }

                //intento obtener los valroes de referencia
                var valoresReferenciaAux = from vr in _dbContext.ValoresReferencia
                                           join n in _dbContext.NombresVariablesMuestras on vr.IdNombreVariableMuestra equals n.Id
                                           where nombreVariables.Contains(n.Nombre)
                                           select new
                                           {
                                               Nombre = n.Nombre,
                                               Minimo = Math.Round(Convert.ToDouble(vr.Minimo), DECIMALES),
                                               Maximo = Math.Round(Convert.ToDouble(vr.Maximo), DECIMALES)
                                           };
                Dictionary<string, object> valoresReferencia = new Dictionary<string, object>();
                foreach (var item in valoresReferenciaAux)
                {
                    var datos = valoresReferenciaAux.Where(x => x.Nombre == item.Nombre).ToList();
                    valoresReferencia.Add(item.Nombre, datos);
                }
                /*
                // Convertir las cadenas a objetos DateTime
                List<DateTime> fechasDateTime = fechas.Select(f => DateTime.ParseExact(f, "yyyy-MM-dd", null)).ToList();

                int disatanciaEntreFechasEndias = (fechasDateTime.Max() - fechasDateTime.Min()).Days;
                //generar un array de fechas desde la fecha minima hasta la fecha maxima
                List<string> fechasCompletas = new List<string>();
                for (int i = 0; i <= disatanciaEntreFechasEndias; i++)
                {
                    fechasCompletas.Add(fechasDateTime.Min().AddDays(i).ToString("yyyy-MM-dd"));
                }

                InformacionFechas informacionFechas = new InformacionFechas()
                {
                    FechaMinima = fechasDateTime.Min().ToString("yyyy-MM-dd"),
                    FechaMaxima = fechasDateTime.Max().ToString("yyyy-MM-dd"),
                    Distancia = disatanciaEntreFechasEndias,
                    FechasCompletas = fechasCompletas
                };




                datosEvolucionOutDto.InformacionFechas = informacionFechas;
                */
                datosEvolucionOutDto.Data = vars;
                datosEvolucionOutDto.ValoresReferencia = valoresReferencia;



                //return Json(new { Data = vars, ValoresReferencia = valoresReferencia });
            }



            return datosEvolucionOutDto;
        }

        public IEnumerable<NombresVariablesMuestra> ObtenerNombresVariablesMuestra(int idTipoMuestra)
        {
            IEnumerable<NombresVariablesMuestra> nombreVariablesMuestra = _dbContext.NombresVariablesMuestras.Where(n => n.IdTipoMuestra == idTipoMuestra).ToList();

            return nombreVariablesMuestra;
        }



    }
}
