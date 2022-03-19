using System.Threading.Tasks;

namespace Zs.Bot.Data.Abstractions
{
    public interface IItemsWithRawDataRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IDbEntityWithRawData<TEntity, TKey>
    {
        Task<TKey> GetActualIdByRawDataHashAsync(TEntity item);
    }
}
