using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zs.Bot.Data.Abstractions;
using Zs.Bot.Data.Models;

namespace Zs.Bot.Data.Repositories;

public sealed class CommandsRepository<TContext> : CommonRepository<TContext, Command, string>, ICommandsRepository
    where TContext : DbContext
{
    public CommandsRepository(
        IDbContextFactory<TContext> contextFactory,
        TimeSpan? criticalQueryExecutionTimeForLogging = null,
        ILogger<CommonRepository<TContext, Command, string>>? logger = null)
        : base(contextFactory, criticalQueryExecutionTimeForLogging, logger)
    {
    }

    public async Task<Command?> FindWhereIdLikeValueAsync(string value)
    {
        return await FindAsync(c => EF.Functions.Like(c.Id, value)).ConfigureAwait(false);
    }
}
