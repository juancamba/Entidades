using Entidades.Models;
using Entidades.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Entidades.Repositories
{
    public class EntidadesRepository : IEntidadesRepository
    {

        private readonly AppDbContext _appDbContext;

        public EntidadesRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<EntidadResumenDto> GetAll()
        {


            var entities = _appDbContext.Entidades
                .Include(e => e.ValoresDatosEstaticos)
                .ToList();

            var result = entities
                .Select(e => new EntidadResumenDto
                {
                    Datos = string.Join(", ", e.ValoresDatosEstaticos.Select(v => v.Valor)),
                    Id = e.Id
                })
             .ToList();

            return result;
        }

        public IEnumerable<EntidadDetalleDto> GetDetalle(string id)
        {


            var query = from e in _appDbContext.Entidades
                        join v in _appDbContext.ValoresDatosEstaticos on e.Id equals v.IdEntidad
                        join n in _appDbContext.NombresDatosEstaticos on v.IdNombreDatoEstatico equals n.Id
                        where e.Id == id
                        select new EntidadDetalleDto
                        {
                            Nombre = n.Nombre,
                            Valor = v.Valor,
                        };
            return query.ToList();

        }

        public void AltaConjuntoEntidad(ConjuntoEntidad conjuntoEntidad)
        {


            using var transaction = _appDbContext.Database.BeginTransaction();
            try
            {
                List<NombresDatosEstatico> listanombresDatosEstaticos = new List<NombresDatosEstatico>();
                foreach (var e in conjuntoEntidad.NombresDatosEstaticos)
                {
                    var nombreDatoEstatico = _appDbContext.NombresDatosEstaticos.FirstOrDefault(n => n.Nombre.Equals(e));
                    if (nombreDatoEstatico != null)
                    {
                        listanombresDatosEstaticos.Add(nombreDatoEstatico);
                    }
                    else
                    {
                        var nombreDatoEstaticoNuevo = new NombresDatosEstatico { Nombre = e };

                        _appDbContext.NombresDatosEstaticos.Add(nombreDatoEstaticoNuevo);
                        _appDbContext.SaveChanges();
                        // aqui supuestamente ya se ha guardado en la base de datos y se ha generado el id
                        listanombresDatosEstaticos.Add(nombreDatoEstaticoNuevo);
                    }

                }
                foreach (var entidadDto in conjuntoEntidad.Entidades)
                {
                    var entidad = _appDbContext.Entidades.FirstOrDefault(e => e.Id.Equals(entidadDto.IdEntidad));
                    // creamos la entidad si no existe
                    if (entidad == null)
                    {
                        entidad = new Entidade { Id = entidadDto.IdEntidad };
                        entidad.FechaAlta = DateTime.Now;
                        _appDbContext.Entidades.Add(entidad);
                        _appDbContext.SaveChanges();
                    }

                    //_appDbContext.ValoresDatosEstaticos.Where(p=>p.IdEntidad.Equals(entidad.Id)).ToList();
                    //eliminamos los valores de datos estaticos de la entidad, para volver a crearlos ( update)
                    _appDbContext.ValoresDatosEstaticos.RemoveRange(
                        _appDbContext.ValoresDatosEstaticos.Where(p => p.IdEntidad.Equals(entidad.Id)).ToList()
                        );
                    _appDbContext.SaveChanges();

                    for (int i = 0; i < entidadDto.ValoresDatosEstaticos.Count; i++)
                    {
                        int idNombreDatoEstatico = listanombresDatosEstaticos[i].Id;
                        string valor = entidadDto.ValoresDatosEstaticos[i];
                        ValoresDatosEstatico valoresDatosEstatico = new ValoresDatosEstatico();
                        valoresDatosEstatico.IdEntidad = entidad.Id;
                        valoresDatosEstatico.IdNombreDatoEstatico = idNombreDatoEstatico;
                        valoresDatosEstatico.Valor = valor;
                        _appDbContext.ValoresDatosEstaticos.Add(valoresDatosEstatico);

                    }
                    _appDbContext.SaveChanges();
                }
                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new InvalidDataException("Error al cargar el archivo");
            }





        }
        /// <summary>
        /// Borrar una entidad y sus valores de datos estaticos y muestras y valores de variables
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="InvalidDataException"></exception>
        public void Delete(string id)
        {
            using var transaction = _appDbContext.Database.BeginTransaction();
            try
            {


                //muestas entidad
                var muestras = _appDbContext.Muestras.Where(p => p.IdEntidad.Equals(id)).ToList();

                //_appDbContext.Muestras.RemoveRange(muestras);
                if (muestras.Count > 0)
                {
                    var query = from m in _appDbContext.Muestras
                                join v in _appDbContext.ValoresVariablesMuestras on m.Id equals v.IdMuestra
                                where m.IdEntidad == id
                                select new
                                {

                                    IdValorVariableMuestra = v.Id
                                };

                    _appDbContext.ValoresVariablesMuestras.RemoveRange(
                                               _appDbContext.ValoresVariablesMuestras.Where(p => query.Select(q => q.IdValorVariableMuestra).Contains(p.Id)).ToList()
                                                                      );


                }
                var entidad = GetById(id);

                var valoresDatosEstaticos = _appDbContext.ValoresDatosEstaticos.Where(p => p.IdEntidad.Equals(id)).ToList();
                _appDbContext.ValoresDatosEstaticos.RemoveRange(valoresDatosEstaticos);



                _appDbContext.Entidades.Remove(entidad);
                _appDbContext.SaveChanges();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new InvalidDataException("Error al eliminar el registro");
            }

        }

        public Entidade GetById(string id)
        {
            return _appDbContext.Entidades.FirstOrDefault(e => e.Id.Equals(id));
        }
    }
}
