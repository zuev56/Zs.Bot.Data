using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zs.Bot.Data.Abstractions;
using Zs.Bot.Data.Models;

namespace Zs.Bot.Data.Repositories;

public abstract class ChatsRepositoryBase<TContext> : ItemsWithRawDataRepository<TContext, Chat, int>, IChatsRepository
    where TContext : DbContext
{
    protected ChatsRepositoryBase(
        IDbContextFactory<TContext> contextFactory,
        TimeSpan? criticalQueryExecutionTimeForLogging = null,
        ILogger<ChatsRepositoryBase<TContext>>? logger = null)
        : base(contextFactory, criticalQueryExecutionTimeForLogging, logger)
    {
    }

    public abstract Task<Chat?> FindByRawDataIdAsync(long rawId);
}
