using System;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data;
using Uchuumaru.Data.Models;
using Uchuumaru.MyAnimeList.Models;
using Uchuumaru.MyAnimeList.Parsers;
using static Uchuumaru.MyAnimeList.Constants;

namespace Uchuumaru.Services.MyAnimeList
{
    public class VerificationService : IVerificationService
    {
        private readonly ProfileParser _profileParser;
        private readonly CommentsParser _commentsParser;
        private readonly IDbContextFactory<UchuumaruContext> _contextFactory;
        private readonly IActivityService _activity;

        public VerificationService(
            ProfileParser profileParser,
            CommentsParser commentsParser,
            IDbContextFactory<UchuumaruContext> contextFactory,
            IActivityService activity)
        {
            _profileParser = profileParser;
            _commentsParser = commentsParser;
            _activity = activity;
            _contextFactory = contextFactory;
        }

        private readonly Random _random = new();
        private readonly HttpClient _client = new();
        
        private const int _tokenLower = 100000;
        private const int _tokenUpper = 999999;
        public const int MaxRetries = 6;
        public const int RetryWaitPeriod = 10000;
        private readonly TimeSpan _minimumAccountAge = TimeSpan.FromDays(30);

        
        public async Task<bool> AccountExists(string username)
        {
            var url = MyAnimeListRootUrl + ProfileUrl + username;
            var response = await _client.GetAsync(url);
            return response.IsSuccessStatusCode;
        }
        
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
                return await Profile.FromUsername(username, _profileParser);
            }
        }

        public async Task<VerificationResult> Authenticate(string username)
        {
            await _profileParser.Refresh(username);
            var dateJoined = _profileParser.GetDateJoined();
                
            if (DateTime.UtcNow.Ticks - dateJoined.Ticks < _minimumAccountAge.Ticks)
                return VerificationResult.FromError(VerificationError.AccountAge, 
                    $"Failed to verify. Your account must be at least {_minimumAccountAge} days old.");
                
            var anime = await _activity.CheckActivity(username, ListOptions.Anime);
            var manga = await _activity.CheckActivity(username, ListOptions.Manga);

            if (!anime.HasValue || !manga.HasValue)
                return VerificationResult.FromError(VerificationError.PrivateLists, 
                    "Failed to verify. One or more of your lists are private. Please make them accessible.");
            
            if (anime is not > 0.5 and < 14 || manga is not > 0.5 and < 14)
                return VerificationResult.FromError(VerificationError.AccountActivity,
                    "Failed to verify. Your account is too inactive. " +
                    "You must regularly update your list to show activity. Spamming entries will not help.");

            return VerificationResult.FromSuccess();
        }
        
        public async Task<VerificationResult> Verify(IGuildUser author, string username, int token) 
        {
            await using (var context = _contextFactory.CreateDbContext())
            {
                var user = await context
                    .MALUsers
                    .FirstOrDefaultAsync(x => x.UserId == author.Id);

                if (user is null)
                {
                    user = new MALUser { UserId = author.Id };
                    context.Add(user);
                }
                
                var str = token.ToString();
                var success = false;

                for (var i = 0; i < MaxRetries; i++)
                {
                    await _profileParser.Refresh(username);

                    if (_profileParser.GetLocation() == str)
                    {
                        success = true;
                        break;
                    }

                    await Task.Delay(RetryWaitPeriod);
                }

                if (success)
                {
                    user.MyAnimeListId = _profileParser.GetUserId();
                    await context.SaveChangesAsync();
                    return VerificationResult.FromSuccess();
                }
                
                return VerificationResult.FromError(VerificationError.InvalidLocation, 
                    "Failed to verify. Did you set your Location correctly? " +
                    $"Remember that you must do this within {RetryWaitPeriod * MaxRetries / 1000} seconds.");
            }
        }
        
        public int GetToken() 
            => _random.Next(_tokenLower, _tokenUpper);
    }
}