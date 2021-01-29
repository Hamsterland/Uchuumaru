using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Interactivity;
using Interactivity.Pagination;
using MoreLinq;
using Uchuumaru.Data.Models;
using Uchuumaru.Services.Infractions;

namespace Uchuumaru.Modules
{
    [Name("Infractions")]
    [Group("infraction")]
    [Summary("Santa's little helper.")]
    [RequireUserPermission(ChannelPermission.ManageMessages)]
    public class InfractionModule : ModuleBase<SocketCommandContext>
    {
        private readonly IInfractionService _infraction;
        private readonly InteractivityService _interactivity;

        public InfractionModule(IInfractionService infraction, InteractivityService interactivity)
        {
            _infraction = infraction;
            _interactivity = interactivity;
        }
        
        [Command("claim")]
        [Summary("Claims an infraction.")]
        public async Task Claim(ulong messageId)
        {
            await _infraction.ClaimInfraction(Context.Guild.Id, messageId, Context.User.Id);
            await ReplyAsync($"Claimed this infraction for {Context.User} ({Context.User.Id})");
        }

        [Command("claim")]
        [Summary("Claims an infraction with a reason.")]
        public async Task Claim(ulong messageId, [Remainder] string reason)
        {
            await _infraction.ClaimInfraction(Context.Guild.Id, messageId, Context.User.Id, reason);
            await ReplyAsync($"Claimed this infraction for {Context.User} ({Context.User.Id}) with reason \"{reason}\".");
        }

        [Command("list", RunMode = RunMode.Async)]
        [Alias("")]
        [Summary("Lists all of a user's infractions.")]
        public async Task List(IUser user)
        {
            var summary = await _infraction.GetAllInfractions(Context.Guild.Id, user.Id);
            var infractions = summary?.ToList();

            if (infractions.Count == 0)
            {
                var embed = new EmbedBuilder()
                    .WithColor(Constants.DefaultColour)
                    .WithAuthor(author => author
                        .WithName($"{user}'s Infractions")
                        .WithIconUrl(user.GetAvatarUrl()))
                    .WithDescription("This user has no infractions.")
                    .Build();

                await ReplyAsync(embed: embed);
            }
            else
            {
                var batched = infractions.Batch(5);
                var pages = new List<PageBuilder>();
            
                foreach (var infractionGroup in batched)
                {
                    var curentPage = new PageBuilder()
                        .WithTitle($"{user}'s ({user.Id}) Infractions")
                        .WithColor(Constants.DefaultColour);

                    foreach (var infraction in infractionGroup)
                    {
                        curentPage
                            .WithColor(Constants.DefaultColour)
                            .AddField($"[{infraction.Id}] {infraction.Type}", infraction.Reason ?? "No reason provided.");
                    }
                
                    pages.Add(curentPage);
                }
            
                var paginator = new StaticPaginatorBuilder()
                    .WithUsers(Context.User)
                    .WithPages(pages)
                    .WithFooter(PaginatorFooter.PageNumber | PaginatorFooter.Users)
                    .WithDefaultEmotes()
                    .Build();
            
                await _interactivity.SendPaginatorAsync(paginator, Context.Channel);
            }
        }

        [Command("list", RunMode = RunMode.Async)]
        [Alias("")]
        [Summary("Lists all of a user's infractions.")]
        public async Task List(ulong userId)
        {
            var user = await Context.Client.Rest.GetUserAsync(userId);

            if (user is null)
            {
                await ReplyAsync($"User \"{userId}\" no longer exists.");
                return;
            }

            await List(user);
        }
        
        [Group("channel")]
        [Summary("Where will the infractions log?")]
        public class ChannelModule : ModuleBase<SocketCommandContext>
        {
            private readonly IInfractionService _infraction;

            public ChannelModule(IInfractionService infraction)
            {
                _infraction = infraction;
            }

            [Command("set")]
            [Summary("Sets the filter channel.")]
            public async Task Set(IGuildChannel channel)
            {
                await _infraction.ModifyInfractionChannel(ChannelModificationOptions.Set, Context.Guild.Id, channel.Id);
                await ReplyAsync($"Set the infraction channel to <#{channel.Id}> ({channel.Id})");
            }
            
            [Command("remove")]
            [Summary("Removes the filter channel.")]
            public async Task Remove()
            {
                await _infraction.ModifyInfractionChannel(ChannelModificationOptions.Remove, Context.Guild.Id);
                await ReplyAsync("Removed the infraction channel.");
            }
        }
    }
}