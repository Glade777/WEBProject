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
    }
}
