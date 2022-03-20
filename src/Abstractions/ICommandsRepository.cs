using System.Threading.Tasks;
using Zs.Bot.Data.Models;
using Zs.Common.Data.Abstractions;

namespace Zs.Bot.Data.Abstractions;

public interface ICommandsRepository : IRepository<Command, string>
{
    //Task<Chat> FindByRawDataIdAsync(long rawId);
    Task<Command> FindWhereIdLikeValueAsync(string value);
}
