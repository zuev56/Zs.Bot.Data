using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Zs.Bot.Data.Abstractions;
using Zs.Common.Exceptions;
using Zs.Common.Extensions;

namespace Zs.Bot.Data.Repositories
{
    /// <summary>
    /// Repository for items containing raw data
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TId">Primary key type</typeparam>
    /// <typeparam name="TContext">DB Context</typeparam>
    public abstract class ItemsWithRawDataRepository<TContext, TEntity, TId> : CommonRepository<TContext, TEntity, TId>, IItemsWithRawDataRepository<TEntity, TId>
        where TEntity : class, IDbEntityWithRawData<TEntity, TId>
        where TContext : DbContext
    {
        private readonly ILogger<ItemsWithRawDataRepository<TContext, TEntity, TId>> _logger;

        public ItemsWithRawDataRepository(
            IDbContextFactory<TContext> contextFactory,
            TimeSpan? criticalQueryExecutionTimeForLogging = null,
            ILogger<ItemsWithRawDataRepository<TContext, TEntity, TId>> logger = null)
            : base(contextFactory, criticalQueryExecutionTimeForLogging, logger)
        {
            _logger = logger;

            BeforeUpdateItem = MergeRawDataFields;
        }
                
        public async Task<TId> GetActualIdByRawDataHashAsync(TEntity item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            var dbItem = await FindAsync(i => i.RawDataHash == item.RawDataHash);

            return dbItem != default && !dbItem.Id.Equals(default(TId))
                ? dbItem.Id
                : throw new ItemNotFoundException(item);
        }

        /// <summary>If items RawData properties are different, copies RawDataHistory and compement it with current</summary>
        /// <param name="existingItem"></param>
        /// <param name="newItem"></param>
        private void MergeRawDataFields(TEntity newItem, TEntity existingItem)
        {
            if (newItem.RawData != existingItem.RawData)
            {
                newItem.RawDataHistory = existingItem.RawDataHistory;

                if (newItem.RawDataHistory == null && newItem.RawData != null)
                {
                    newItem.RawDataHistory = $"[{existingItem.RawData}]".NormalizeJsonString();
                }
                else if (newItem.RawDataHistory != null)
                {
                    var rawDataHistory = JsonSerializer.Deserialize<JsonElement>(newItem.RawDataHistory).EnumerateArray().ToList();
                    rawDataHistory.Add(JsonSerializer.Deserialize<JsonElement>(existingItem.RawData));
                    newItem.RawDataHistory = JsonSerializer.Serialize(rawDataHistory).NormalizeJsonString();
                }

                newItem.RawDataHash = newItem.RawData.GetMD5Hash();
            }
        }
    }
}
