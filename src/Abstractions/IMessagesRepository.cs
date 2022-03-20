using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zs.Bot.Data.Models;
using Zs.Common.Data.Abstractions;

namespace Zs.Bot.Data.Abstractions;

public interface IMessagesRepository : IItemsWithRawDataRepository<Message, int>
{
    Task<Message> FindByRawDataIdsAsync(int rawMessageId, long rawChatId);

    Task<List<Message>> FindDailyMessages(int chatId);
    Task<List<Message>> FindBotDialogMessagesInTimeRange(int chatId, int botUserId, string botName, DateTime fromDate, DateTime toDate);
    Task<Dictionary<int, int>> FindUserIdsAndMessagesCountSinceDate(int chatId, DateTime? startDate);
    Task<List<Message>> FindAllTodaysMessagesWithTextAsync(string searchText);
}
