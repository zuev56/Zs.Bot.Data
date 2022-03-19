namespace Zs.Bot.Data.Enums
{
    public enum MessageAction : short
    {
        Undefined = -1,
        Received = 0,
        Sending,
        Sent,
        Edited,
        Deleted
    }
}
