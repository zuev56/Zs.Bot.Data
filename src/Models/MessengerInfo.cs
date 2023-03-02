using System;
using System.Collections.Generic;
using Zs.Bot.Data.Abstractions;

namespace Zs.Bot.Data.Models;

public sealed class MessengerInfo : IDbEntity<MessengerInfo, string>
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Func<MessengerInfo> GetItemForSave => () => this;
    public Func<MessengerInfo, MessengerInfo> GetItemForUpdate => _ => this;
    public ICollection<Message> Messages { get; set; } = null!;

}