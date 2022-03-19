using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zs.Bot.Data.Abstractions;
using Zs.Bot.Data.Models;

namespace Zs.Bot.Data.Repositories
{
    public abstract class MessagesRepositoryBase<TContext> : ItemsWithRawDataRepository<TContext, Message, int>, IMessagesRepository
        where TContext : DbContext
    {
        public MessagesRepositoryBase(
            IDbContextFactory<TContext> contextFactory,
            TimeSpan? criticalQueryExecutionTimeForLogging = null,
            ILogger<MessagesRepositoryBase<TContext>> logger = null)
            : base(contextFactory, criticalQueryExecutionTimeForLogging, logger)
        {
        }

        public abstract Task<Message> FindByRawDataIdsAsync(int rawMessageId, long rawChatId);

        public abstract Task<List<Message>> FindDailyMessages(int chatId);

        public abstract Task<Dictionary<int, int>> FindUserIdsAndMessagesCountSinceDate(int chatId, DateTime? startDate);

        public async Task<List<Message>> FindBotDialogMessagesInTimeRange(int chatId, int botUserId, string botName, DateTime fromDate, DateTime toDate)
        {
            return await FindAllAsync(
                m => m.ChatId == chatId
                  && m.InsertDate > fromDate
                  && m.InsertDate < toDate
                  && !m.IsDeleted
                  && (m.UserId == botUserId || (m.Text.Trim().IndexOf("/") == 0 && m.Text.Contains(botName)))).ConfigureAwait(false);
        }

        public async Task<List<Message>> FindAllTodaysMessagesWithTextAsync(string searchText)
        {
            return await FindAllAsync(m => m.InsertDate > DateTime.UtcNow.Date && m.Text.Contains(searchText)).ConfigureAwait(false);
        }
    }
}
