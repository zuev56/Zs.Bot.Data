using System;
using System.Collections.Generic;
using Zs.Bot.Data.Abstractions;
using Zs.Common.Extensions;

namespace Zs.Bot.Data.Models;

public class User : IDbEntityWithRawData<User, int>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? FullName { get; set; }
    public string UserRoleId { get; set; } = null!;
    public bool IsBot { get; set; }
    public string RawData { get; set; } = null!;
    public string RawDataHash { get; set; } = null!;
    public string? RawDataHistory { get; set; }
    public DateTime UpdateDate { get; set; }
    public DateTime InsertDate { get; set; }
    public ICollection<Message> Messages { get; set; } = null!;
    public Enums.Role UserRole => GetUserRole();

    public Func<User> GetItemForSave => () => this;
    public Func<User, User> GetItemForUpdate => (existingItem) =>
    {
        return new User
        {
            Id = existingItem.Id,
            Name = this.Name,
            FullName = this.FullName,
            UserRoleId = existingItem.UserRoleId,
            IsBot = existingItem.IsBot,
            RawData = this.RawData,
            RawDataHash = this.RawDataHash,
            RawDataHistory = this.RawDataHistory,
            UpdateDate = DateTime.UtcNow,
            InsertDate = existingItem.InsertDate
        };
    };

    private Enums.Role GetUserRole()
    {
        var role = UserRoleId.ToLowerInvariant().FirstCharToUpper();
        if (Enum.IsDefined(typeof(Enums.Role), role))
            return Enum.Parse<Enums.Role>(role);

        return Enums.Role.User;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as User);
    }

    public bool Equals(User? other)
    {
        return other != null &&
               Id == other.Id &&
               Name == other.Name &&
               FullName == other.FullName &&
               UserRoleId == other.UserRoleId &&
               IsBot == other.IsBot &&
               RawData == other.RawData &&
               RawDataHash == other.RawDataHash &&
               RawDataHistory == other.RawDataHistory;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(Id);
        hash.Add(Name);
        hash.Add(FullName);
        hash.Add(UserRoleId);
        hash.Add(IsBot);
        hash.Add(RawData);
        hash.Add(RawDataHash);
        hash.Add(RawDataHistory);
        return hash.ToHashCode();
    }

    public override string ToString() => $"{Name ?? FullName} ({Id})";
}
