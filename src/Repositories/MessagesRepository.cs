using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zs.Bot.Data.Extensions;
using Zs.Bot.Data.Models;
using Zs.Bot.Data.Queries;
using Zs.Common.Models;

namespace Zs.Bot.Data.Repositories;

public class MessagesRepository<TContext> : CommonRepository<TContext, Message>, IMessagesRepository
    where TContext : DbContext
{
    public MessagesRepository(IDbContextFactory<TContext> contextFactory, IQueryFactory queryFactory)
        : base(contextFactory, queryFactory)
    {
    }

    public async Task<IReadOnlyList<Message>> FindByRawDatesAsync(int rawChatId, DateTimeRange dateTimeRange, CancellationToken cancellationToken)
    {
        var chatIdPath = QueryFactory.RawDataStructure.Message.ChatId;
        var datePath = QueryFactory.RawDataStructure.Message.Date;
        var startDate = dateTimeRange.Start.ToJsonDate();
        var endDate = dateTimeRange.End.ToJsonDate();

        var condition = RawData.Eq(chatIdPath, rawChatId)
            .And(RawData.Gte(datePath, startDate))
            .And(RawData.Lte(datePath, endDate));

        var messages = await FindByRawDataConditionAsync(condition, cancellationToken);

        return messages;
    }

    public async Task<Message?> FindByRawIdAsync(int rawMessageId, long rawChatId, CancellationToken cancellationToken)
    {
        var messageIdPath = QueryFactory.RawDataStructure.Message.Id;
        var chatIdPath = QueryFactory.RawDataStructure.Message.ChatId;
        var condition = RawData.Eq(chatIdPath, rawChatId)
            .And(RawData.Eq(messageIdPath, rawMessageId));

        var messages = await FindByRawDataConditionAsync(condition, cancellationToken);

        return messages.SingleOrDefault();
    }

    public async Task<IReadOnlyList<Message>> FindDialogWithBotAsync(int rawChatId, int rawUserId, string botName, DateTimeRange dateTimeRange, CancellationToken cancellationToken)
    {
        var chatIdPath = QueryFactory.RawDataStructure.Message.ChatId;
        var datePath = QueryFactory.RawDataStructure.Message.Date;
        var startDate = dateTimeRange.Start.ToJsonDate();
        var endDate = dateTimeRange.End.ToJsonDate();

        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<Message>> FindWithTextAsync(int rawChatId, string searchText, DateTimeRange dateTimeRange, CancellationToken cancellationToken)
    {
        var (_, textPath, chatIdPath, _, datePath) = QueryFactory.RawDataStructure.Message;
        var startDate = dateTimeRange.Start.ToJsonDate();
        var endDate = dateTimeRange.End.ToJsonDate();
        var condition = RawData.Eq(chatIdPath, rawChatId)
                .And(RawData.Gte(datePath, startDate))
                .And(RawData.Lte(datePath, endDate))
                .And(RawData.Contains(textPath, searchText));

        var messages = await FindByRawDataConditionAsync(condition, cancellationToken);

        return messages;
    }
}