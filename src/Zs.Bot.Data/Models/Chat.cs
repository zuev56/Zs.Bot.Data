using System;
using System.Collections.Generic;
using Zs.Bot.Data.Abstractions;

namespace Zs.Bot.Data.Models
{
    public class Chat : IDbEntityWithRawData<Chat, int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ChatTypeId { get; set; }
        public string RawData { get; set; }
        public string RawDataHash { get; set; }
        public string RawDataHistory { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime InsertDate { get; set; }
        public Func<Chat> GetItemForSave => () => this;
        public Func<Chat, Chat> GetItemForUpdate => (existingItem) =>
            new Chat
            {
                Id             = existingItem.Id,
                Name           = this.Name,
                Description    = this.Description,
                ChatTypeId     = this.ChatTypeId,
                RawData        = this.RawData,
                RawDataHash    = this.RawDataHash,
                RawDataHistory = this.RawDataHistory,
                UpdateDate     = DateTime.UtcNow,
                InsertDate     = existingItem.InsertDate
            };


        public ChatType ChatType { get; set; }
        public ICollection<Message> Messages { get; set; }
        
        public override bool Equals(object obj)
        {
            return Equals(obj as Chat);
        }

        public bool Equals(Chat other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   Description == other.Description &&
                   ChatTypeId == other.ChatTypeId &&
                   RawData == other.RawData &&
                   RawDataHash == other.RawDataHash &&
                   RawDataHistory == other.RawDataHistory;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Name);
            hash.Add(Description);
            hash.Add(ChatTypeId);
            hash.Add(RawData);
            hash.Add(RawDataHash);
            hash.Add(RawDataHistory);
            return hash.ToHashCode();
        }

        public override string ToString() => $"{Name} ({Id} | {ChatType})";
    }

}
