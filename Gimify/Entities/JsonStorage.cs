using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Gimify.Entities;

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

        public void Add(T entity)
        {
            _items = GetAll();

            if (entity.id == 0)
            {
                int nextId = _items.Any() ? _items.Max(e => e.id) + 1 : 1;
                entity.id = nextId;
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

        public T? GetById(int id)
        {
            return _items.FirstOrDefault(e => e.id == id);
        }

        public List<T> GetAll()
        {
            return _items; 
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize(_items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json); 
        }
    }
}
