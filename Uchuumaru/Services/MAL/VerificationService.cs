using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Uchuumaru.Data;
using Uchuumaru.Data.Models;

namespace Uchuumaru.Services.MAL
{
    /// <inheritdoc/>
    public class VerificationService : IVerificationService
    {
        /// <summary>
        /// The database context.
        /// </summary>
        private readonly UchuumaruContext _context;
        
        /// <summary>
        /// The profile parser.
        /// </summary>
        private readonly IProfileParser _parser;
        
        /// <summary>
        /// The Discord client.
        /// </summary>
        private readonly DiscordSocketClient _client;

        /// <summary>
        /// Constructs a new <see cref="VerificationService"/> with the given
        /// injected dependencies.
        /// </summary>
        public VerificationService(
            UchuumaruContext context, 
            IProfileParser parser, 
            DiscordSocketClient client)
        {
            _context = context;
            _parser = parser;
            _client = client;
        }

        /// <inheritdoc/>
        public async Task<Profile> GetProfile(ulong userId)
        {
            var malUser = await _context
                .MALUsers
                .FirstOrDefaultAsync(x => x.UserId == userId);
            
            if (malUser is null || !malUser.IsVerified)
                throw new NoVerifiedProfilException(_client.GetUser(userId));

            var username = await _parser.GetUsernameFromId(malUser.MAL);
            return await _parser.Parse(username);
        }
        
        /// <inheritdoc/>
        public async Task Start(ulong guildId, ulong userId)
        {
            var malUser = await _context
                .MALUsers
                .FirstOrDefaultAsync(x => x.UserId == userId);

            var user = _client.GetUser(userId);
            
            // If the user is already verified, we don't need to generate a new code.
            // The * is an indicator that we're returning an existing code.
            if (malUser is not null && malUser.IsVerified)
            {
                await user.SendMessageAsync($"Your Verification Code is: `{malUser.VerificationCode}`.");
                return;
            }

            if (malUser is null)
            {
                malUser = new MALUser { UserId = userId };
                _context.Add(malUser);
            }
            
            var code = GenerateVerificationCode(guildId, userId);
            
            malUser.VerificationCode = code;
            await _context.SaveChangesAsync();
            
            await user.SendMessageAsync($"Your Verification Code is `{code}`. Set your MAL account Location to this code. Then do +mal set [MAL username].");
        }

        /// <inheritdoc/>
        public async Task Confirm(ulong userId, string username)
        {
            var malUser = await _context
                .MALUsers
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (malUser.VerificationCode is null)
                throw new NoVerificationCodeException(_client.GetUser(userId));
            
            var html = await _parser.DownloadHtml(username);
            var document = await _parser.ParseDocumentAsync(html);
            var elements = _parser.GetClearfixElements(document);

            var location = _parser.GetLocation(new List<IElement>(elements));
            if (location != malUser.VerificationCode)
                throw new VerificationFailedException(_client.GetUser(userId));
            
            var imageUrl = _parser.GetImageUrl(document);
            var id = _parser.GetUserId(imageUrl);
            
            malUser.MAL = id;
            await _context.SaveChangesAsync();
        }
        
        private static string GenerateVerificationCode(ulong guildId, ulong userId)
        {
            var p1 = DateTime.Now.Ticks;
            var p2 = TakeDigits((long) (guildId + userId), 5);
            return $"{p1}-{p2}";
        }
        
        private static long TakeDigits(long number, int digits)
        {
            number = Math.Abs(number);

            if (number == 0)
                return number;

            var numberOfDigits = (int) Math.Floor(Math.Log10(number) + 1);

            if (numberOfDigits >= digits)
                return (int) Math.Truncate(number / Math.Pow(10, numberOfDigits - digits));
            
            return number;
        }
    }
}