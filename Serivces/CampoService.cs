using Entidades.Models;
using Entidades.Repositories;

namespace Entidades.Services
{
    public class CampoService
    {
        private readonly ICampoRepository _campoRepository;

        public CampoService(ICampoRepository campoRepository)
        {
            _campoRepository = campoRepository;
        }

        public void Create(Campo campo)
        {
            _campoRepository.Create(campo);
        }

        public void Delete(int id)
        {
            _campoRepository.Delete(id);
        }

        public IEnumerable<Campo> GetAll()
        {
            return _campoRepository.GetAll();
        }

        public Campo GetById(int id)
        {
            return _campoRepository.GetById(id);
        }

        public bool SaveChanges()
        {
            return _campoRepository.SaveChanges();
        }

        public void Update(Campo campo)
        {
            _campoRepository.Update(campo);
        }


    }
}
