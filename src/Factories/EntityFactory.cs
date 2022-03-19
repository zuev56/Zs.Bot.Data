using System;
using Zs.Bot.Data.Models;

namespace Zs.Bot.Data.Factories
{
    public static class EntityFactory
    {
        public static Message NewMessage(string messageText = null, int chatId = default, int userId = default)
        {
            return new Message
            {
                Text = messageText,
                ChatId = chatId,
                UserId = userId,
                InsertDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };
        }

        public static Chat NewChat()
        {
            return new Chat
            {
                InsertDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };
        }

        public static User NewUser()
        {
            return new User
            {
                InsertDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };
        }
    }
}
