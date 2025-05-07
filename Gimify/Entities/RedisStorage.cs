using Gimify.Entities;
using StackExchange.Redis;
using System.Text.Json;

public class RedisStorage<T> : IDataStorage<T> where T : BaseEntity
{
    private readonly IDatabase _db;
    private readonly string _prefix;

    public RedisStorage(IConnectionMultiplexer redis, string prefix)
    {
        _db = redis.GetDatabase();
        _prefix = prefix;
    }


    public List<T> GetAll()
    {
        var server = _db.Multiplexer.GetServer(_db.Multiplexer.GetEndPoints().First());
        return server.Keys(pattern: $"{_prefix}:*")
            .Select(key => JsonSerializer.Deserialize<T>(_db.StringGet(key)))
            .Where(x => x != null)
            .ToList();
    }

    public void Add(T entity)
    {
        _db.StringSet($"{_prefix}:{entity.id}", JsonSerializer.Serialize(entity));
    }

    public T? GetById(int id)
    {
        var value = _db.StringGet($"{_prefix}:{id}");
        return value.IsNull ? null : JsonSerializer.Deserialize<T>(value);
    }

    public void Update(T entity) => Add(entity);

    public void Delete(int id) => _db.KeyDelete($"{_prefix}:{id}");

    public void Save() { }


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
        await _db.StringSetAsync($"{_prefix}:{entity.id}", JsonSerializer.Serialize(entity));
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        var value = await _db.StringGetAsync($"{_prefix}:{id}");
        return value.IsNull ? null : JsonSerializer.Deserialize<T>(value);
    }

    public async Task UpdateAsync(T entity) => await AddAsync(entity);

    public async Task DeleteAsync(int id) => await _db.KeyDeleteAsync($"{_prefix}:{id}");

    public Task SaveAsync() => Task.CompletedTask;
}