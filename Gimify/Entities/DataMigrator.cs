using Gimify.DAL.Repositories;
using Gimify.DAL;
using Gimify.Entities;


public class DataMigrator
{
    public static async Task MigrateToRedis(
        IDataStorage<User> userJsonStorage,
        IDataStorage<Posts> postJsonStorage,
        IDataStorage<User> userRedisStorage,
        IDataStorage<Posts> postRedisStorage)
    {
       
        foreach (var user in userJsonStorage.GetAll())
            await userRedisStorage.AddAsync(user);

        foreach (var post in postJsonStorage.GetAll())
            await postRedisStorage.AddAsync(post);
    }
}