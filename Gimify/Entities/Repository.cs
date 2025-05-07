using Gimify.Entities;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Gimify.DAL.Repositories
{
    public class Repository<T> where T : BaseEntity
    {
        private readonly IDataStorage<T> _storage;

        public Repository(IDataStorage<T> storage)
        {
            _storage = storage;
        }

        public List<T> GetAll() => _storage.GetAll();
        public void Add(T entity) => _storage.Add(entity);
        public void Update(T entity) => _storage.Update(entity);
        public void Delete(int id) => _storage.Delete(id);
        public T? GetById(int id) => _storage.GetById(id);

        public async Task<List<T>> GetAllAsync() => await _storage.GetAllAsync();
        public async Task AddAsync(T entity) => await _storage.AddAsync(entity);
        public async Task UpdateAsync(T entity) => await _storage.UpdateAsync(entity);
        public async Task DeleteAsync(int id) => await _storage.DeleteAsync(id);
        public async Task<T?> GetByIdAsync(int id) => await _storage.GetByIdAsync(id);
    }
}