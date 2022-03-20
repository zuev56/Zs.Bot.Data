using System;
using System.Collections.Generic;
using Zs.Common.Abstractions.Data;

namespace Zs.Bot.Data.Models;

public class ChatType : IDbEntity<ChatType, string>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Func<ChatType> GetItemForSave => () => this;
    public Func<ChatType, ChatType> GetItemForUpdate => (existingItem) => this;
    public ICollection<Chat> Chats { get; set; }
    public override string ToString() => Name;
}

