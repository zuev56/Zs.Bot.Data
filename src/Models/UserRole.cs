using System;
using System.Collections.Generic;
using Zs.Bot.Data.Abstractions;

namespace Zs.Bot.Data.Models;

public class UserRole : IDbEntity<UserRole, string>
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Permissions { get; set; } = null!;
    public Func<UserRole> GetItemForSave => () => this;
    public Func<UserRole, UserRole> GetItemForUpdate => (existingItem) => this;
    public ICollection<User> Users { get; set; } = null!;
}

