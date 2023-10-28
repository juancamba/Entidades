using Entidades.Models;
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
        public void Create(ValoresReferencia valoresReferencia)
        {

            if (valoresReferencia != null)

            {
                _context.ValoresReferencia.Add(valoresReferencia);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException(nameof(valoresReferencia));
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
                            IdNombreVariableMuestraNavigation = n,
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



    }
}
