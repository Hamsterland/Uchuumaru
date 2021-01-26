﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Uchuumaru.Services.Users;

namespace Uchuumaru.Modules
{
    [Name("User")]
    [Summary("The FBI cannot compare.")]
    public class UserModule : ModuleBase<SocketCommandContext>
    {
        private readonly IUserService _user;

        public UserModule(IUserService user)
        {
            _user = user;
        }

        [Command("names")]
        [Summary("Shows the past 20 usernames and nicknames for a user.")]
        public async Task Names(IGuildUser user)
        {
            var summary = await _user.GetUserSummary(Context.Guild.Id, user.Id);

            List<string> usernames;
            List<string> nicknames;
            
            if (summary is null)
            {
                usernames = new List<string> { user.Username };
                nicknames = new List<string> { user.Nickname };
            }
            else
            {
                usernames = summary.Usernames;
                usernames.Add(user.Username);
                usernames.Reverse();

                nicknames = summary.Nicknames;
                
                if (user.Nickname is not null)
                {
                    nicknames.Add(user.Nickname);
                }   
                
                nicknames.Reverse();
            }

            var embed = new EmbedBuilder()
                .WithColor(Constants.DefaultColour)
                .WithAuthor(author => author
                    .WithName($"{user}'s Name History")
                    .WithIconUrl(user.GetAvatarUrl()))
                .AddField("Usernames", string.Join(", ", usernames))
                .AddField("Nicknames", string.Join(", ", nicknames))
                .Build();

            await ReplyAsync(embed: embed);
        }
    }
}