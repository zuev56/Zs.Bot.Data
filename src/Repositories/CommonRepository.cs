using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Zs.Common.Abstractions.Data;

namespace Zs.Bot.Data.Repositories;

/// <summary>
/// Universal repository
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
/// <typeparam name="TId">Primary key type</typeparam>
/// <typeparam name="TContext">DB Context</typeparam>
public abstract class CommonRepository<TContext, TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : class, IDbEntity<TEntity, TId>
    where TContext : DbContext
{
    private readonly TimeSpan _criticalQueryExecutionTime;
    private readonly ILogger<CommonRepository<TContext, TEntity, TId>> _logger;

    protected IDbContextFactory<TContext> ContextFactory { get; }

    // TODO: Log query time
    // TODO: Return IOperationResult on save or delete

    // TODO: Make specific delegate type
    /// <summary> Calls before items update. 
    /// First argument - saving item, second argument - existing item from database </summary>
    protected Action<TEntity, TEntity> BeforeUpdateItem { get; set; }


    public CommonRepository(
        IDbContextFactory<TContext> contextFactory,
        TimeSpan? criticalQueryExecutionTimeForLogging = null,
        ILogger<CommonRepository<TContext, TEntity, TId>> logger = null)
    {
        ContextFactory = contextFactory ?? throw new NullReferenceException(nameof(contextFactory));
        _criticalQueryExecutionTime = criticalQueryExecutionTimeForLogging ?? TimeSpan.FromSeconds(1);
        _logger = logger;
    }

    protected async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default)
    {
        await using (var context = await ContextFactory.CreateDbContextAsync().ConfigureAwait(false))
        {
            return await context.Set<TEntity>().CountAsync(predicate, cancellationToken).ConfigureAwait(false);
        }
    }

    public virtual async Task<TEntity> FindByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var sw = new Stopwatch();
        sw.Start();
        string resultQuery = null;

        try
        {
            await using (var context = await ContextFactory.CreateDbContextAsync().ConfigureAwait(false))
            {
                var query = context.Set<TEntity>();

                resultQuery = query.ToQueryString();

                return await query.AsNoTracking().FirstOrDefaultAsync(i => i.Id.Equals(id), cancellationToken).ConfigureAwait(false);
            }
        }
        finally
        {
            sw.Stop();
            LogFind("Repository.FindByIdAsync [Elapsed: {Elapsed}].\n\tSQL: {SQL}", sw.Elapsed, resultQuery);
        }
    }

    protected virtual async Task<TEntity> FindBySqlAsync(string sql, CancellationToken cancellationToken = default)
    {
        var sw = new Stopwatch();
        sw.Start();
        string resultQuery = null;

        try
        {
            await using (var context = await ContextFactory.CreateDbContextAsync().ConfigureAwait(false))
            {
                var query = context.Set<TEntity>().FromSqlRaw(sql);

                resultQuery = query.ToQueryString();

                return await query.AsNoTracking().FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        finally
        {
            sw.Stop();
            LogFind("Repository.FindBySqlAsync [Elapsed: {Elapsed}].\n\tSQL: {SQL}", sw.Elapsed, resultQuery);
        }
    }

    /// <summary>
    /// Asynchronously returns the first element of a sequence that satisfies a specified
    /// condition or a default value if no such element is found
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="orderBy"> Sorting rules before executing predicate</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual async Task<TEntity> FindAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        CancellationToken cancellationToken = default)
    {
        var sw = new Stopwatch();
        sw.Start();
        string resultQuery = null;

        try
        {
            await using (var context = await ContextFactory.CreateDbContextAsync().ConfigureAwait(false))
            {
                IQueryable<TEntity> query = context.Set<TEntity>();

                if (predicate != null)
                    query = query.Where(predicate);

                if (orderBy != null)
                    query = orderBy(query);

                resultQuery = query.ToQueryString();

                return await query.AsNoTracking().FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        finally
        {
            sw.Stop();
            LogFind("Repository.FindAsync [Elapsed: {Elapsed}].\n\tSQL: {SQL}", sw.Elapsed, resultQuery);
        }
    }

    /// <summary>Asynchronously returns the list of elements</summary>
    public async Task<List<TEntity>> FindAllAsync(int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        return await FindAllAsync(null, null, skip, take, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously returns the list of elements of a sequence that satisfies a specified condition
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="orderBy"> Sorting rules before executing predicate</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual async Task<List<TEntity>> FindAllAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default)
    {
        var sw = new Stopwatch();
        sw.Start();
        string resultQuery = null;

        try
        {
            await using (var context = await ContextFactory.CreateDbContextAsync().ConfigureAwait(false))
            {
                IQueryable<TEntity> query = context.Set<TEntity>();

                if (predicate != null)
                    query = query.Where(predicate);

                if (orderBy != null)
                    query = orderBy(query);

                if (skip != null)
                    query = query.Skip((int)skip);

                if (take != null)
                    query = query.Take((int)take);

                resultQuery = query.ToQueryString();

                return await query.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        finally
        {
            sw.Stop();
            LogFind("Repository.FindAllAsync [Elapsed: {Elapsed}].\n\tSQL: {SQL}", sw.Elapsed, resultQuery);
        }
    }

    public async Task<List<TEntity>> FindAllAsync(TId[] ids, CancellationToken cancellationToken = default)
    {
        return await FindAllAsync(i => ids.Contains(i.Id)).ConfigureAwait(false);
    }

    protected async Task<List<TEntity>> FindAllBySqlAsync(string sql, CancellationToken cancellationToken = default)
    {
        var sw = new Stopwatch();
        sw.Start();
        string resultQuery = null;

        try
        {
            await using (var context = await ContextFactory.CreateDbContextAsync().ConfigureAwait(false))
            {
                var query = context.Set<TEntity>().FromSqlRaw(sql);

                resultQuery = query.ToQueryString();

                return await query.AsNoTracking().ToListAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        finally
        {
            sw.Stop();
            LogFind("Repository.FindAllBySqlAsync [Elapsed: {Elapsed}].\n\tSQL: {SQL}", sw.Elapsed, resultQuery);
        }
    }

    public virtual async Task<bool> SaveAsync(TEntity item, CancellationToken cancellationToken = default)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        var sw = new Stopwatch();
        sw.Start();
        string resultChanges = null;
        string detailedResultChanges = null;

        try
        {
            await using (var context = await ContextFactory.CreateDbContextAsync().ConfigureAwait(false))
            {
                var itemToSave = await AddItemToContextForSave(context, item, cancellationToken).ConfigureAwait(false);

                if (context.ChangeTracker.HasChanges())
                {
                    resultChanges = Environment.NewLine + context.ChangeTracker.ToDebugString(ChangeTrackerDebugStringOptions.ShortDefault);
                    detailedResultChanges = Environment.NewLine + context.ChangeTracker.ToDebugString(ChangeTrackerDebugStringOptions.LongDefault);

                    int changes = await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    item.Id = itemToSave.Id;
                    return changes == 1;
                }
            }

            return false;
        }
        finally
        {
            sw.Stop();
            LogSave("Repository.SaveAsync [Elapsed: {Elapsed}].\n\tChanges: {changes}", sw.Elapsed, resultChanges, detailedResultChanges);
        }
    }

    public virtual async Task<bool> SaveRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default)
    {
        if (items is null)
            throw new ArgumentNullException(nameof(items));

        var sw = new Stopwatch();
        sw.Start();
        string resultChanges = null;
        string detailedResultChanges = null;

        try
        {
            await using (var context = await ContextFactory.CreateDbContextAsync().ConfigureAwait(false))
            {
                var itemsToSave = new List<TEntity>();
                foreach (var item in items)
                {
                    itemsToSave.Add(await AddItemToContextForSave(context, item, cancellationToken).ConfigureAwait(false));
                }

                if (context.ChangeTracker.HasChanges())
                {
                    resultChanges = context.ChangeTracker.ToDebugString(ChangeTrackerDebugStringOptions.ShortDefault);
                    detailedResultChanges = context.ChangeTracker.ToDebugString(ChangeTrackerDebugStringOptions.LongDefault);

                    int changes = await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                    var itemsList = items.ToList();
                    for (int i = 0; i < itemsToSave.Count; i++)
                    {
                        itemsList[i].Id = itemsToSave[i].Id;
                    }

                    return changes == itemsList.Count;
                }
            }

            return false;
        }
        finally
        {
            sw.Stop();
            LogSave("Repository.SaveRangeAsync [Elapsed: {Elapsed}].\n\tChanges: {changes}", sw.Elapsed, resultChanges, detailedResultChanges);
        }
    }

    private async Task<TEntity> AddItemToContextForSave(TContext context, TEntity item, CancellationToken cancellationToken)
    {
        var itemToSave = item.GetItemForSave?.Invoke();

        if (itemToSave is null)
            throw new InvalidOperationException("Item for save can not be null!");

        var existingItem = !itemToSave.Id.Equals(default(TId))
            ? await context.Set<TEntity>().FirstOrDefaultAsync(i => i.Id.Equals(itemToSave.Id), cancellationToken).ConfigureAwait(false)
            : null;

        if (existingItem != null)
        {
            // Make changes directly in the itemToSave
            itemToSave = itemToSave.GetItemForUpdate(existingItem);

            // Make changes in child repository
            BeforeUpdateItem?.Invoke(itemToSave, existingItem);

            if (!itemToSave.Equals(existingItem))
            {
                context.Entry(existingItem).State = EntityState.Detached;
                context.Entry(itemToSave).State = EntityState.Modified;
                context.Set<TEntity>().Update(itemToSave);
            }
        }
        else
        {
            context.Set<TEntity>().Add(itemToSave);
        }

        return itemToSave;
    }

    public virtual async Task<bool> DeleteAsync(TEntity item, CancellationToken cancellationToken = default)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        var sw = new Stopwatch();
        sw.Start();
        string resultChanges = null;

        try
        {
            await using (var context = await ContextFactory.CreateDbContextAsync().ConfigureAwait(false))
            {
                var existingItem = await context.Set<TEntity>().FirstOrDefaultAsync(i => i.Id.Equals(item.Id), cancellationToken).ConfigureAwait(false);
                if (existingItem != null)
                {
                    context.Set<TEntity>().Remove(existingItem);
                    resultChanges = context.ChangeTracker.ToDebugString(ChangeTrackerDebugStringOptions.LongDefault);

                    return await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false) == 1;
                }
            }


            return false;
        }
        finally
        {
            sw.Stop();
            LogDelete("Repository.DeleteAsync [Elapsed: {Elapsed}].\n\tChanges: {changes}", sw.Elapsed, resultChanges);
        }
    }

    public virtual async Task<bool> DeleteRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default)
    {
        if (items is null)
            throw new ArgumentNullException(nameof(items));

        var sw = new Stopwatch();
        sw.Start();
        string resultChanges = null;

        try
        {
            await using (var context = await ContextFactory.CreateDbContextAsync().ConfigureAwait(false))
            {
                var ids = items.Select(i => i.Id).ToList();
                var existingItems = await context.Set<TEntity>().Where(i => ids.Contains(i.Id)).ToListAsync(cancellationToken);
                if (existingItems?.Any() == true && existingItems.Count == items.Count())
                {
                    context.Set<TEntity>().RemoveRange(existingItems);
                    resultChanges = context.ChangeTracker.ToDebugString(ChangeTrackerDebugStringOptions.LongDefault);

                    return await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false) == existingItems.Count;
                }
            }

            return false;
        }
        finally
        {
            sw.Stop();
            LogDelete("Repository.DeleteRangeAsync [Elapsed: {Elapsed}].\n\tChanges: {changes}", sw.Elapsed, resultChanges);
        }
    }

    protected void LogFind(string message, TimeSpan elapsed, string sql)
    {
        if (elapsed > _criticalQueryExecutionTime)
            _logger?.LogWarning(message, elapsed, sql);
        else
            _logger?.LogDebug(message, elapsed, sql);
    }

    private void LogSave(string message, TimeSpan elapsed, string changes, string detailedChanges)
    {
        if (elapsed > _criticalQueryExecutionTime)
            _logger?.LogWarning(message, elapsed, detailedChanges);
        else if (_logger?.IsEnabled(LogLevel.Trace) != true)
            _logger?.LogDebug(message, elapsed, changes);
        else
            _logger?.LogTrace(message, elapsed, detailedChanges);
    }

    private void LogDelete(string message, TimeSpan elapsed, string changes)
    {
        LogFind(message, elapsed, changes);
    }

}
