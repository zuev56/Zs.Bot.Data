using Zs.Bot.Data.Models;
using Zs.Common.Data.Abstractions;

namespace Zs.Bot.Data.Abstractions;

public interface IUserRolesRepository : IRepository<UserRole, string>
{
    //Task<Chat> FindByRawDataIdAsync(long rawId);
}
