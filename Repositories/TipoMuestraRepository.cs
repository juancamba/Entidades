using Entidades.Models;

namespace Entidades.Repositories
{
    public class TipoMuestraRepository : ITipoMuestraRepository
    {
        private readonly AppDbContext _context;

        public TipoMuestraRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Create(TiposMuestra Muestra)
        {
            if (Muestra != null)
            {
                _context.TiposMuestras.Add(Muestra);
                _context.SaveChanges();
            }
            else
            {
                throw new System.ArgumentNullException(nameof(Muestra));
            }
        }

        public void Delete(int id)
        {
            var muestra = _context.TiposMuestras.Find(id);
            if (muestra != null)
            {
                _context.TiposMuestras.Remove(muestra);
                _context.SaveChanges();
            }
            else
            {
                throw new System.ArgumentNullException(nameof(muestra));
            }
        }

        public System.Collections.Generic.IEnumerable<TiposMuestra> GetAll()
        {
            return _context.TiposMuestras.ToList();
        }

        public TiposMuestra GetById(int id)
        {
            return _context.TiposMuestras.Find(id);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public void Update(TiposMuestra tipoMuestra)
        {
            var objDb = _context.TiposMuestras.FirstOrDefault(p => p.Id == tipoMuestra.Id);
            if (objDb != null)
            {
                objDb.Nombre = tipoMuestra.Nombre;
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException(nameof(tipoMuestra));
            }
        }
    }
}
