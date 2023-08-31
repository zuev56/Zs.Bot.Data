using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zs.Bot.Data.Models;
using Zs.Common.Models;

namespace Zs.Bot.Data.Repositories;

public interface IMessagesRepository : IRepository<Message>
{
    Task<IReadOnlyList<Message>> FindByRawDatesAsync(int rawChatId, DateTimeRange dateTimeRange, CancellationToken cancellationToken = default);
    Task<Message?> FindByRawIdAsync(int rawMessageId, long rawChatId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Message>> FindDialogWithBotAsync(int rawChatId, int rawUserId, string botName, DateTimeRange dateTimeRange, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Message>> FindWithTextAsync(int rawChatId, string searchText, DateTimeRange dateTimeRange, CancellationToken cancellationToken = default);
}