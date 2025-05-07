using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Gimify.Entities;
using System.Threading.Tasks;

namespace Gimify.DAL
{
    public class JsonStorage<T> : IDataStorage<T> where T : BaseEntity
    {
        private readonly string _filePath;
        private List<T> _items;

        public JsonStorage(string filePath)
        {
            _filePath = filePath;
            _items = LoadFromFile();
        }

        private List<T> LoadFromFile()
        {
            if (!File.Exists(_filePath))
                return new List<T>();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }

        private async Task<List<T>> LoadFromFileAsync()
        {
            if (!File.Exists(_filePath))
                return new List<T>();

            await using var fs = new FileStream(_filePath, FileMode.Open);
            return await JsonSerializer.DeserializeAsync<List<T>>(fs) ?? new List<T>();
        }

        public List<T> GetAll() => _items;

        public void Add(T entity)
        {
            if (entity.id == 0)
            {
                entity.id = _items.Any() ? _items.Max(e => e.id) + 1 : 1;
            }
            _items.Add(entity);
            Save();
        }

        public void Update(T entity)
        {
            var index = _items.FindIndex(e => e.id == entity.id);
            if (index >= 0)
            {
                _items[index] = entity;
                Save();
            }
        }

        public void Delete(int id)
        {
            var item = _items.FirstOrDefault(e => e.id == id);
            if (item != null)
            {
                _items.Remove(item);
                Save();
            }
        }

        public T? GetById(int id) => _items.FirstOrDefault(e => e.id == id);

        public void Save()
        {
            var json = JsonSerializer.Serialize(_items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public async Task<List<T>> GetAllAsync()
        {
            _items = await LoadFromFileAsync();
            return _items;
        }

        public async Task AddAsync(T entity)
        {
            if (entity.id == 0)
            {
                var items = await GetAllAsync();
                entity.id = items.Any() ? items.Max(e => e.id) + 1 : 1;
            }
            _items.Add(entity);
            await SaveAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            var items = await GetAllAsync();
            var index = items.FindIndex(e => e.id == entity.id);
            if (index >= 0)
            {
                items[index] = entity;
                _items = items;
                await SaveAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var items = await GetAllAsync();
            var item = items.FirstOrDefault(e => e.id == id);
            if (item != null)
            {
                items.Remove(item);
                _items = items;
                await SaveAsync();
            }
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var items = await GetAllAsync();
            return items.FirstOrDefault(e => e.id == id);
        }

        public async Task SaveAsync()
        {
            await using var fs = new FileStream(_filePath, FileMode.Create);
            await JsonSerializer.SerializeAsync(fs, _items, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
