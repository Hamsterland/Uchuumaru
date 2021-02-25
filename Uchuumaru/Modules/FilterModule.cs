using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Uchuumaru.Data.Models;
using Uchuumaru.Preconditions;
using Uchuumaru.Services.Filters;

namespace Uchuumaru.Modules
{
    [Name("Filter")]
    [Group("filter")]
    [Summary("Why doesn't alien life exist?")]
    [RequireModeratorOrDeveloper]
    public class FilterModule : ModuleBase<SocketCommandContext>
    {
        private readonly IFilterService _filter;

        public FilterModule(IFilterService filter)
        {
            _filter = filter;
        }

        [Command("enable")]
        [Summary("Enables the filter.")]
        public async Task Enable()
        {
            await _filter.ModifyFilter(Context.Guild.Id, true);
            await ReplyAsync("Enabled the filter.");
        }
        
        [Command("disable")]
        [Summary("Enables the filter.")]
        public async Task Disable()
        {
            await _filter.ModifyFilter(Context.Guild.Id, false);
            await ReplyAsync("Disabled the filter.");
        }
        
        [Command("status")]
        [Summary("Shows information about the filter.")]
        public async Task Stats()
        {
            var status = await _filter.GetFilterStatus(Context.Guild.Id);

            var embed = new EmbedBuilder()
                .WithColor(Constants.DefaultColour)
                .WithAuthor(author => author
                    .WithName($"{Context.Guild.Name} Filter Status")
                    .WithIconUrl(Context.Guild.IconUrl))
                .AddField("Enabled", status.Enabled)
                .AddField("Expressions", "Use the `filter list` command to see filtered expressions.")
                .Build();

            await ReplyAsync(embed: embed);
        }

        [Command("list")]
        [Summary("Lists all filtered expressions.")]
        public async Task List()
        {
            var status = await _filter.GetFilterStatus(Context.Guild.Id);
            var JSON = JsonSerializer.Serialize(status.Expressions, new JsonSerializerOptions { WriteIndented = true });
            var path = $"{Directory.GetCurrentDirectory()}/filters-{DateTime.UtcNow:MM.dd.yyyy}"; 

            await using (var sw = File.CreateText(path))
            {
                await sw.WriteAsync(JSON);
            }

            await Context.Channel.SendFileAsync(path, "Here is a list of all the filtered expressions.");
            File.Delete(path);
        }
        
        [Command("add")]
        [Summary("Adds an expression to the filter.")]
        public async Task Add([Remainder] string expression)
        {
            await _filter.AddFilter(Context.Guild.Id, expression);
            await ReplyAsync($"Added the expression \"{expression}\" to the filter.");
        }

        [Command("remove")]
        [Summary("Removes an expression from the filter.")]
        public async Task Remove([Remainder] string expression)
        {
            await _filter.RemoveFilter(Context.Guild.Id, expression);
            await ReplyAsync($"Removed the expression \"{expression}\" from the filter.");
        }

        [Group("channel")]
        [Summary("You have to write the violations somewhere.")]
        public class ChannelModule : ModuleBase<SocketCommandContext>
        {
            private readonly IFilterService _filter;

            public ChannelModule(IFilterService filter)
            {
                _filter = filter;
            }
            
            [Command("set")]
            [Summary("Sets the filter channel.")]
            public async Task Set(IGuildChannel channel)
            {
                await _filter.ModifyFilterChannel(ChannelModificationOptions.Set, Context.Guild.Id, channel.Id);
                await ReplyAsync($"Set the filter violation channel to <#{channel.Id}> ({channel.Id})");
            }
            
            [Command("remove")]
            [Summary("Removes the filter channel.")]
            public async Task Remove()
            {
                await _filter.ModifyFilterChannel(ChannelModificationOptions.Remove, Context.Guild.Id);
                await ReplyAsync("Removed the filter violation channel.");
            }
        }
    }
}