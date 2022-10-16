using Microsoft.EntityFrameworkCore;
using Zs.Bot.Data.Models;

namespace Zs.Bot.Data;

public abstract class BotContext<TContext> : DbContext
    where TContext : DbContext
{
    public DbSet<ChatType> ChatTypes { get; set; } = null!;
    public DbSet<Chat> Chats { get; set; } = null!;
    public DbSet<Command> Commands { get; set; } = null!;
    public DbSet<MessageType> MessageTypes { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<MessengerInfo> Messengers { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected BotContext()
    {
    }

    protected BotContext(DbContextOptions<TContext> options)
        : base(options)
    {
    }
}
