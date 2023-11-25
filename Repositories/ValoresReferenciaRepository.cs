using Entidades.Models;
using Entidades.Models.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Entidades.Repositories
{
    public class ValoresReferenciaRepository : IValoresReferenciaRepository
    {

        private readonly AppDbContext _context;

        public ValoresReferenciaRepository(AppDbContext context)
        {
            _context = context;

        }
        public void Create(ValoresReferencia valoresReferenciavm)
        {

            if (valoresReferenciavm != null)
            {
                ValoresReferencia valoresReferencia = new ValoresReferencia();
                var nombreVariableMuestra = _context.NombresVariablesMuestras.FirstOrDefault(p => p.Id == valoresReferenciavm.IdNombreVariableMuestra);
                valoresReferencia.NombreVariableMuestra = nombreVariableMuestra;
                valoresReferencia.IdNombreVariableMuestra = nombreVariableMuestra.Id;
                valoresReferencia.Maximo = valoresReferenciavm.Maximo;
                valoresReferencia.Minimo = valoresReferenciavm.Minimo;

                _context.ValoresReferencia.Add(valoresReferencia);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException(nameof(valoresReferenciavm));
            }
        }

        public void Delete(int id)
        {
            _context.ValoresReferencia.Remove(GetById(id));
            _context.SaveChanges();
        }

        public IEnumerable<ValoresReferencia> GetAll()
        {

            var query = from v in _context.ValoresReferencia
                        join n in _context.NombresVariablesMuestras on v.IdNombreVariableMuestra equals n.Id
                        select new ValoresReferencia
                        {
                            Id = v.Id,
                            IdNombreVariableMuestra = v.IdNombreVariableMuestra,
                            NombreVariableMuestra = n,
                            Minimo = v.Minimo,
                            Maximo = v.Maximo,

                        }
                        ;
            return query.ToList();
            /*
            return _context.ValoresReferencia
                .Include(p => p.NombreVariableMuestraNavigation)

                .ToList();*/
        }

        public ValoresReferencia GetById(int id)
        {
            return _context.ValoresReferencia.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public void Update(ValoresReferencia valoresReferencia)
        {
            var objDb = _context.ValoresReferencia.FirstOrDefault(p => p.Id == valoresReferencia.Id);
            if (objDb != null)
            {
                objDb.Minimo = valoresReferencia.Minimo;
                objDb.Maximo = valoresReferencia.Maximo;
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException(nameof(valoresReferencia));
            }
        }

        public IEnumerable<SelectListItem> GetListaNombreVariables()
        {
            return _context.NombresVariablesMuestras.Select(i => new SelectListItem()
            {
                Text = i.Nombre,
                Value = i.Id.ToString()
            });
        }
        public IEnumerable<NombresVariablesMuestra> ObtenerVariablesSinValoresReferencia(int idTipoMuestra)
        {
            IEnumerable<NombresVariablesMuestra> nombreVariablesMuestra = _context.NombresVariablesMuestras
                .Where(n => n.IdTipoMuestra == idTipoMuestra && !_context.ValoresReferencia.Any(v => v.IdNombreVariableMuestra == n.Id))

                .ToList();

            return nombreVariablesMuestra;
        }

        public void AltaMasiva(List<FicheroValorReferenciaDto> ficheroValorReferenciaDto)
        {

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                foreach (FicheroValorReferenciaDto valor in ficheroValorReferenciaDto)
                {
                    ValoresReferencia valoresReferencia = new ValoresReferencia();
                    var nombreVariableMuestra = _context.NombresVariablesMuestras.FirstOrDefault(p => p.Nombre.ToUpper() == valor.Variable.ToUpper());
                    //comprobar que existe en valoresReferencia
                    if (nombreVariableMuestra != null)
                    {

                        if (_context.ValoresReferencia.Any(v => v.IdNombreVariableMuestra == nombreVariableMuestra.Id))
                        {
                            var objDb = _context.ValoresReferencia.FirstOrDefault(p => p.IdNombreVariableMuestra == nombreVariableMuestra.Id);
                            if (objDb != null)
                            {
                                objDb.Minimo = valor.Minimo;
                                objDb.Maximo = valor.Maximo;
                            }
                            else
                            {
                                throw new ArgumentNullException(nameof(valoresReferencia));
                            }
                        }
                        else
                        {
                            valoresReferencia.NombreVariableMuestra = nombreVariableMuestra;
                            valoresReferencia.IdNombreVariableMuestra = nombreVariableMuestra.Id;
                            valoresReferencia.Maximo = valor.Maximo;
                            valoresReferencia.Minimo = valor.Minimo;
                            _context.ValoresReferencia.Add(valoresReferencia);
                        }
                    }
                }
                _context.SaveChanges();
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

        }
    }
}
