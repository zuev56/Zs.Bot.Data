using System;
using System.Collections.Generic;
using Zs.Bot.Data.Abstractions;

namespace Zs.Bot.Data.Models
{
    public class MessengerInfo : IDbEntity<MessengerInfo, string>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Func<MessengerInfo> GetItemForSave => () => this;
        public Func<MessengerInfo, MessengerInfo> GetItemForUpdate => (existingItem) => this;
        public ICollection<Message> Messages { get; set; }

    }

}
