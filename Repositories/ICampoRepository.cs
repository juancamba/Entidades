using Entidades.Models;

namespace Entidades.Repositories
{
    public interface ICampoRepository
    {
        bool SaveChanges();

        IEnumerable<Campo> GetAll();

        Campo GetById(int id);
        void Create(Campo campo);

        void Update(Campo campo);
        void Delete(int id);
    }
}
