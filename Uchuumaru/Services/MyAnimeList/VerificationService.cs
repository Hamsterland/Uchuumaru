using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Schema;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Uchuumaru.Data;
using Uchuumaru.Data.Models;
using Uchuumaru.MyAnimeList.Models;
using Uchuumaru.MyAnimeList.Parsers;

namespace Uchuumaru.Services.MyAnimeList
{
    public class VerificationService : IVerificationService
    {
        private readonly ProfileParser _profileParser;
        private readonly CommentsParser _commentsParser;
        private readonly IDbContextFactory<UchuumaruContext> _contextFactory;

        public VerificationService(
            ProfileParser profileParser,
            CommentsParser commentsParser,
            IDbContextFactory<UchuumaruContext> contextFactory)
        {
            _profileParser = profileParser;
            _commentsParser = commentsParser;
            _contextFactory = contextFactory;
        }

        private readonly Random _random = new();

        private const int _tokenLower = 100000;
        private const int _tokenUpper = 999999;
        private const int _maxRetries = 3;
        private const int _RetryWaitPeriod = 10000;

        private Emote _loading = Emote.Parse("<a:loading:818260297118384178>");
        
        public async Task<Profile> GetProfile(ulong userId)
        {
            await using (var context = _contextFactory.CreateDbContext())
            {
                var user = await context
                    .MALUsers
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (user is null || !user.IsVerified)
                    return null;

                await _commentsParser.Download(user.MyAnimeListId);
                var username = _commentsParser.GetUsername();
                var profile = await Profile.FromUsername(username, _profileParser);
                return profile;   
            }
        }

        public async Task Begin(IGuildUser author, string username, ITextChannel channel) 
        {
            await using (var _context = _contextFactory.CreateDbContext())
            {
                var user = await _context
                    .MALUsers
                    .FirstOrDefaultAsync(x => x.UserId == author.Id);

                if (user is null)
                {
                    user = new MALUser { UserId = author.Id };
                    _context.Add(user);
                }

                var token = GetToken();
                var avatar = author.GetAvatarUrl();
                
                var embed = new EmbedBuilder()
                    .WithColor(Constants.DefaultColour)
                    .WithAuthor(a => a
                        .WithName($"{author}")
                        .WithIconUrl(avatar))
                    .WithDescription($"{_loading} Please set your MyAnimeList account Location field to the Token below. The rest is magic!")
                    .AddField("Token", token, true)
                    .AddField("Edit Profile", "https://myanimelist.net/editprofile.php", true)
                    .Build();

                var message = await channel.SendMessageAsync(embed: embed);
                
                var str = token.ToString();
                var success = false;

                for (var i = 0; i < _maxRetries; i++)
                {
                    await _profileParser.Download(username);

                    if (_profileParser.GetLocation() == str)
                    {
                        success = true;
                        break;
                    }

                    await Task.Delay(_RetryWaitPeriod);
                }
                
                if (success)
                {
                    await message.ModifyAsync(msg =>
                    {
                        msg.Embed = new EmbedBuilder()
                            .WithColor(Color.Green)
                            .WithAuthor(a => a
                                .WithName($"{author}")
                                .WithIconUrl(avatar))
                            .WithDescription("Successfully linked your account.")
                            .Build();
                    });
                    
                    user.MyAnimeListId = _profileParser.GetUserId();
                    await _context.SaveChangesAsync();
                }
                else
                {
                    await message.ModifyAsync(msg =>
                    {
                        msg.Embed = new EmbedBuilder()
                            .WithColor(Color.Red)
                            .WithAuthor(a => a
                                .WithName($"{author}")
                                .WithIconUrl(avatar))
                            .WithDescription("Failed to link your account. Did you set your location correctly?")
                            .Build();
                    });
                }
            }
        }

        private int GetToken() 
            => _random.Next(_tokenLower, _tokenUpper);
    }
}