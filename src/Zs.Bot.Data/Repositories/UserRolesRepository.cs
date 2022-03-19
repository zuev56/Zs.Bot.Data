using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using Zs.Bot.Data.Abstractions;
using Zs.Bot.Data.Models;

namespace Zs.Bot.Data.Repositories
{
    public sealed class UserRolesRepository<TContext> : CommonRepository<TContext, UserRole, string>, IUserRolesRepository
        where TContext : DbContext
    {
        public UserRolesRepository(
            IDbContextFactory<TContext> contextFactory,
            TimeSpan? criticalQueryExecutionTimeForLogging = null,
            ILogger<CommonRepository<TContext, UserRole, string>> logger = null)
            : base(contextFactory, criticalQueryExecutionTimeForLogging, logger)
        {
        }
    }
}
