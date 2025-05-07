using System.Threading.Tasks;

namespace Gimify.Entities
{
    public interface IDataStorage<T> where T : BaseEntity
    {
        List<T> GetAll();
        void Add(T entity);
        void Delete(int id);
        T? GetById(int id);
        void Update(T entity);
        void Save();

        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task DeleteAsync(int id);
        Task<T?> GetByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task SaveAsync();
    }
}