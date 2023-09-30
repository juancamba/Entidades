using Entidades.Models;

namespace Entidades.Repositories
{
    public interface ITipoMuestraRepository
    {
        bool SaveChanges();

        IEnumerable<TiposMuestra> GetAll();

        TiposMuestra GetById(int id);
        void Create(TiposMuestra Muestra);

        void Update(TiposMuestra Muestra);
        void Delete(int id);
    }
}
