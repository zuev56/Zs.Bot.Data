using System;
using System.Collections.Generic;
using Zs.Bot.Data.Abstractions;

namespace Zs.Bot.Data.Models;

public class ChatType : IDbEntity<ChatType, string>
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Func<ChatType> GetItemForSave => () => this;
    public Func<ChatType, ChatType> GetItemForUpdate => _ => this;
    public ICollection<Chat> Chats { get; set; } = null!;
    public override string ToString() => Name;
}