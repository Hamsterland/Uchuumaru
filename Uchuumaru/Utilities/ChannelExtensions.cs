using Discord;

namespace Uchuumaru.Utilities
{
    public static class ChannelExtensions
    {
        public static string Mention(this IChannel channel) => $"<#{channel.Id}>";
        public static string Represent(this IChannel channel) => $"{channel.Mention()} ({channel.Id})";
    }
}