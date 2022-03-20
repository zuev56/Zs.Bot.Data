using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zs.Bot.Data.Abstractions;
using Zs.Bot.Data.Enums;
using Zs.Bot.Data.Models;
using Zs.Common.Data.Repositories;

namespace Zs.Bot.Data.Repositories;

public abstract class UsersRepositoryBase<TContext> : ItemsWithRawDataRepository<TContext, User, int>, IUsersRepository
    where TContext : DbContext
{
    public UsersRepositoryBase(
        IDbContextFactory<TContext> contextFactory,
        TimeSpan? criticalQueryExecutionTimeForLogging = null,
        ILogger<UsersRepositoryBase<TContext>> logger = null)
        : base(contextFactory, criticalQueryExecutionTimeForLogging, logger)
    {
    }

    // Implementation depends on concrete database
    public abstract Task<User> FindByRawDataIdAsync(long rawId);

    public async Task<List<User>> FindByRoleIdsAsync(IEnumerable<Role> userRoles)
    {
        var userRoleIds = userRoles.Select(r => r.ToString().ToUpperInvariant());
        return await FindAllAsync(u => userRoleIds.Contains(u.UserRoleId)).ConfigureAwait(false);
    }
}
