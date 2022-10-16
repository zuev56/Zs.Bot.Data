using System.Threading.Tasks;
using Zs.Bot.Data.Models;

namespace Zs.Bot.Data.Abstractions;

public interface IChatsRepository : IItemsWithRawDataRepository<Chat, int>
{
    Task<Chat?> FindByRawDataIdAsync(long rawId);
}
