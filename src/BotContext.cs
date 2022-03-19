using Microsoft.EntityFrameworkCore;
using Zs.Bot.Data.Models;

namespace Zs.Bot.Data
{
    public abstract class BotContext<TContext> : DbContext
        where TContext : DbContext
    {
        public DbSet<ChatType> ChatTypes { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<MessageType> MessageTypes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessengerInfo> Messengers { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }

        protected BotContext()
        {
        }
        
        protected BotContext(DbContextOptions<TContext> options)
            : base(options)
        {
        }
    }
}
