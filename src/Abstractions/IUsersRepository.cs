using System.Collections.Generic;
using System.Threading.Tasks;
using Zs.Bot.Data.Enums;
using Zs.Bot.Data.Models;

namespace Zs.Bot.Data.Abstractions
{
    public interface IUsersRepository : IItemsWithRawDataRepository<User, int>
    {
        Task<User> FindByRawDataIdAsync(long rawId);
        Task<List<User>> FindByRoleIdsAsync(IEnumerable<Role> userRoleIds);
    }
}
