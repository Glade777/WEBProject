using Gimify.Entities;
using StackExchange.Redis;
using System.Text.Json;

namespace Gimify.DAL
{
    public class RedisRepository<T> : IDataStorage<T> where T : BaseEntity
    {
        private readonly IDatabase _db;
        private readonly string _prefix;

        public RedisRepository(IConnectionMultiplexer redis, string prefix)
        {
            _db = redis.GetDatabase();
            _prefix = prefix;
        }

        public List<T> GetAll()
        {
            var server = _db.Multiplexer.GetServer(_db.Multiplexer.GetEndPoints().First());
            var keys = server.Keys(pattern: $"{_prefix}:*");

            return keys.Select(key =>
            {
                var value = _db.StringGet(key);
                return JsonSerializer.Deserialize<T>(value);
            }).Where(item => item != null).ToList();
        }

        public void Add(T entity)
        {
            var key = $"{_prefix}:{entity.id}";
            var serialized = JsonSerializer.Serialize(entity);
            _db.StringSet(key, serialized);
        }

        public T? GetById(int id)
        {
            var key = $"{_prefix}:{id}";
            var value = _db.StringGet(key);
            return value.IsNull ? null : JsonSerializer.Deserialize<T>(value);
        }

        public void Update(T entity) => Add(entity);

        public void Delete(int id) => _db.KeyDelete($"{_prefix}:{id}");

        public void Save() { } // Redis 

  
        public async Task<List<T>> GetAllAsync()
        {
            var server = _db.Multiplexer.GetServer(_db.Multiplexer.GetEndPoints().First());
            var keys = server.Keys(pattern: $"{_prefix}:*");

            var tasks = keys.Select(key => _db.StringGetAsync(key));
            var values = await Task.WhenAll(tasks);

            return values
                .Where(v => !v.IsNull)
                .Select(v => JsonSerializer.Deserialize<T>(v))
                .ToList();
        }

        public async Task AddAsync(T entity)
        {
            var key = $"{_prefix}:{entity.id}";
            var serialized = JsonSerializer.Serialize(entity);
            await _db.StringSetAsync(key, serialized);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var key = $"{_prefix}:{id}";
            var value = await _db.StringGetAsync(key);
            return value.IsNull ? null : JsonSerializer.Deserialize<T>(value);
        }

        public async Task UpdateAsync(T entity) => await AddAsync(entity);

        public async Task DeleteAsync(int id) => await _db.KeyDeleteAsync($"{_prefix}:{id}");

        public Task SaveAsync() => Task.CompletedTask;
    }
}