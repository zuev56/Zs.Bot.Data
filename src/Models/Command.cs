using System;
using Zs.Bot.Data.Abstractions;

namespace Zs.Bot.Data.Models;

public class Command : IDbEntity<Command, string>
{
    public string Id { get; set; } = null!;
    public string Script { get; set; } = null!;
    public string? DefaultArgs { get; set; }
    public string? Description { get; set; }
    public string Group { get; set; } = null!;
    public Func<Command> GetItemForSave => () => this;
    public Func<Command, Command> GetItemForUpdate => _ => this;
}