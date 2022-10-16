﻿using System;
using System.Collections.Generic;
using Zs.Bot.Data.Abstractions;

namespace Zs.Bot.Data.Models;

public class MessageType : IDbEntity<MessageType, string>
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Func<MessageType> GetItemForSave => () => this;
    public Func<MessageType, MessageType> GetItemForUpdate => (existingItem) => this;
    public ICollection<Message> Messages { get; set; } = null!;
}

