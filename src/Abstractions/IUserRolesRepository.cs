﻿using Zs.Bot.Data.Models;

namespace Zs.Bot.Data.Abstractions
{
    public interface IUserRolesRepository : IRepository<UserRole, string>
    {
        //Task<Chat> FindByRawDataIdAsync(long rawId);
    }
}