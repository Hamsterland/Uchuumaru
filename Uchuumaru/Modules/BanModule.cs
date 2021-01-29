using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Uchuumaru.Services.Infractions.Bans;

namespace Uchuumaru.Modules
{
    [Name("Ban")]
    [Summary("7DS.")]
    [RequireUserPermission(GuildPermission.BanMembers)]
    public class BanModule : ModuleBase<SocketCommandContext>
    {
        private readonly IBanService _ban;

        public BanModule(IBanService ban)
        {
            _ban = ban;
        }

        private readonly Random _random = new();
        
        private readonly string[] _banMessages = 
        {
            "{0} was sent to hell.",
            "{0} hit the ground too hard.",
            "{0}'s VISA expired.",
            "{0} went on vacation.",
            "{0} burned to death.",
            "{0} drowned",
            "Assassinated {0}.",
        };

        private readonly string[] _unbanMessages =
        {
            "God's grace saved {0}.",
            "{0}'s sentence expired.",
            "{0} received a new VISA.",
        };
        
        [Command("ban")]
        [Summary("Bans a user.")]
        public async Task Ban(IGuildUser user, [Remainder] string reason = null)
        {
            var bans = await Context.Guild.GetBansAsync();

            if (bans.Any(x => x.User.Id == user.Id))
            {
                await ReplyAsync($"{user} is already banned.");
                return; 
            }

            await Context.Guild.AddBanAsync(user, reason: reason);
            await ReplyBanMessage(user);
        }

        [Command("hackban")]
        [Summary("Bans a user by Id.")]
        public async Task Hackban(ulong userId, [Remainder] string reason = null)
        {
            var user = await Context.Client.Rest.GetUserAsync(userId);

            if (user is null)
            {
                await ReplyAsync($"The specified user with Id \"{userId}\" does not exist.");
                return;
            }
            
            var bans = await Context.Guild.GetBansAsync();

            if (bans.Any(ban => ban.User.Id == userId))
            {
                await ReplyAsync($"{user} is already banned.");
                return; 
            }
            
            await Context.Guild.AddBanAsync(user, reason: reason);
            await ReplyBanMessage(user);
        }

        [Command("unban")]
        [Summary("Unbans a user.")]
        public async Task Unban(ulong userId)
        {
            var user = await Context.Client.Rest.GetUserAsync(userId);

            if (user is null)
            {
                await ReplyAsync($"The specified user with Id {userId} no longer exists.");
                return;
            }

            var bans = await Context.Guild.GetBansAsync();

            if (bans.All(x => x.User.Id != userId))
            {
                await ReplyAsync($"{user} is not banned in this guild.");
                return; 
            }
            
            await Context.Guild.RemoveBanAsync(userId);
            await _ban.Unban(Context.Guild.Id, user.Id, Context.User.Id);
            await ReplyUnbanMessage(user);
        }
        
        private async Task ReplyBanMessage(IUser user)
        {
            var message = _banMessages[_random.Next(0, _banMessages.Length - 1)];
            await ReplyAsync(string.Format(message, user));
        }
        
        private async Task ReplyUnbanMessage(IUser user)
        {
            var message = _unbanMessages[_random.Next(0, _unbanMessages.Length - 1)];
            await ReplyAsync(string.Format(message, user));
        }
    }
}