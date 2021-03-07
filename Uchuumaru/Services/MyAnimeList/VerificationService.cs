using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data;
using Uchuumaru.Data.Models;
using Uchuumaru.MyAnimeList.Models;
using Uchuumaru.MyAnimeList.Parsers;

namespace Uchuumaru.Services.MyAnimeList
{
    public class VerificationService
    {
        private readonly UchuumaruContext _context;
        private readonly DiscordSocketClient _client;
        private readonly ProfileParser _profileParser;
        private readonly CommentsParser _commentsParser;

        public VerificationService(
            UchuumaruContext context,
            DiscordSocketClient client,
            ProfileParser profileParser,
            CommentsParser commentsParser)
        {
            _context = context;
            _client = client;
            _profileParser = profileParser;
            _commentsParser = commentsParser;
        }

        private readonly Random _random = new();

        private const int _tokenLower = 100000;
        private const int _tokenUpper = 999999;
        private readonly ConcurrentDictionary<ulong, string> _usernames = new();

        public async Task<Profile> GetProfile(ulong userId)
        {
            var user = await _context
                .MALUsers
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (user is null || !user.IsVerified)
                return null;

            await _commentsParser.Download(user.MyAnimeListId);
            var username = _commentsParser.GetUsername();
            var profile = await Profile.FromUsername(username, _profileParser);
            return profile;
        }

        public async Task Begin(ulong userId, string username, int token)
        {
            _usernames.AddOrUpdate(userId, username, (_, _) => username);
            
            var user = await _context
                .MALUsers
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (user is null)
            {
                user = new MALUser { UserId = userId };
                _context.Add(user);
            }

            user.Token = token;
            await _context.SaveChangesAsync();
        }

        public async Task Confirm(ulong userId)
        {
            var success = _usernames.TryGetValue(userId, out var username);

            if (!success)
                return;
            
            var user = await _context
                .MALUsers
                .FirstOrDefaultAsync(x => x.UserId == userId);

            await _profileParser.Download(username);

            var location = _profileParser.GetLocation();
            
            
        }
        
        public int GetToken()
            => _random.Next(_tokenLower, _tokenUpper);
    }
}