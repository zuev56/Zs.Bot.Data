using System;
using Zs.Bot.Data.Abstractions;

namespace Zs.Bot.Data.Models;

public class Message : IDbEntityWithRawData<Message, int>
{
    public int Id { get; set; }
    public int? ReplyToMessageId { get; set; }
    public string MessengerId { get; set; } = null!;
    public string MessageTypeId { get; set; } = null!;
    public int UserId { get; set; }
    public int ChatId { get; set; }
    public string? Text { get; set; }
    public string RawData { get; set; } = null!;
    public string RawDataHash { get; set; } = null!;
    public string? RawDataHistory { get; set; }
    public bool IsSucceed { get; set; }
    public int FailsCount { get; set; }
    public string? FailDescription { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime UpdateDate { get; set; }
    public DateTime InsertDate { get; set; }
    public MessageType MessageType { get; set; } = null!;
    public Message ReplyToMessage { get; set; } = null!;
    public MessengerInfo Messenger { get; set; } = null!;
    public User User { get; set; } = null!;
    public Chat Chat { get; set; } = null!;

    public Func<Message> GetItemForSave => () =>
        new Message
        {
            Id = this.Id,
            ReplyToMessageId = this.ReplyToMessageId,
            MessengerId = this.MessengerId,
            MessageTypeId = this.MessageTypeId,
            UserId = this.UserId,
            ChatId = this.ChatId,
            Text = this.Text?.Length > 100 ? this.Text.Substring(0, 100) : this.Text,
            RawData = this.RawData,
            RawDataHash = this.RawDataHash,
            RawDataHistory = this.RawDataHistory,
            IsSucceed = this.IsSucceed,
            FailsCount = this.FailsCount,
            FailDescription = this.FailDescription,
            IsDeleted = this.IsDeleted,
            UpdateDate = this.UpdateDate,
            InsertDate = this.InsertDate,
        };

    public Func<Message, Message> GetItemForUpdate => (existingItem) =>
        new Message
        {
            Id = existingItem.Id,
            ReplyToMessageId = this.ReplyToMessageId,
            MessengerId = existingItem.MessengerId,
            MessageTypeId = this.MessageTypeId,
            UserId = existingItem.UserId,
            ChatId = existingItem.ChatId,
            Text = this.Text,
            RawData = this.RawData,
            RawDataHash = this.RawDataHash,
            RawDataHistory = this.RawDataHistory,
            IsSucceed = existingItem.IsSucceed,
            FailsCount = existingItem.FailsCount,
            FailDescription = existingItem.FailDescription,
            IsDeleted = this.IsDeleted,
            UpdateDate = DateTime.UtcNow,
            InsertDate = existingItem.InsertDate,
        };

    public override bool Equals(object? obj)
    {
        return Equals(obj as Message);
    }

    public bool Equals(Message? other)
    {
        return other != null &&
               Id == other.Id &&
               ReplyToMessageId == other.ReplyToMessageId &&
               MessengerId == other.MessengerId &&
               MessageTypeId == other.MessageTypeId &&
               UserId == other.UserId &&
               ChatId == other.ChatId &&
               Text == other.Text &&
               RawData == other.RawData &&
               RawDataHash == other.RawDataHash &&
               RawDataHistory == other.RawDataHistory &&
               IsSucceed == other.IsSucceed &&
               FailsCount == other.FailsCount &&
               FailDescription == other.FailDescription &&
               IsDeleted == other.IsDeleted;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(Id);
        hash.Add(ReplyToMessageId);
        hash.Add(MessengerId);
        hash.Add(MessageTypeId);
        hash.Add(UserId);
        hash.Add(ChatId);
        hash.Add(Text);
        hash.Add(RawData);
        hash.Add(RawDataHash);
        hash.Add(RawDataHistory);
        hash.Add(IsSucceed);
        hash.Add(FailsCount);
        hash.Add(FailDescription);
        hash.Add(IsDeleted);
        return hash.ToHashCode();
    }

    public Message DeepCopy()
    {
        return new Message
        {
            Id = this.Id,
            ReplyToMessageId = this.ReplyToMessageId,
            MessengerId = this.MessengerId,
            MessageTypeId = this.MessageTypeId,
            UserId = this.UserId,
            ChatId = this.ChatId,
            Text = this.Text,
            RawData = this.RawData,
            RawDataHash = this.RawDataHash,
            RawDataHistory = this.RawDataHistory,
            IsSucceed = this.IsSucceed,
            FailsCount = this.FailsCount,
            FailDescription = this.FailDescription,
            IsDeleted = this.IsDeleted,
            UpdateDate = this.UpdateDate,
            InsertDate = this.InsertDate,
        };
    }
    public override string ToString() => Id.ToString();
}

