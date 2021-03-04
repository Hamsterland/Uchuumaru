using System;
using Discord;

namespace Uchuumaru.Services.MAL
{
    [Obsolete("Inefficient, terrible code. Riddled with bugs. Replaced by Uchuumaru.MyAnimeList.")]
    public class NoVerifiedProfilException : Exception
    {
        public NoVerifiedProfilException(IUser user)
            : base($"{user} does not have a profile set.")
        {
        }
    }
    
    [Obsolete("Inefficient, terrible code. Riddled with bugs. Replaced by Uchuumaru.MyAnimeList.")]
    public class NoVerificationCodeException : Exception
    {
        public NoVerificationCodeException(IUser user)
            : base($"{user}, you do not have a Verification Code.")
        {
        }
    }
    
    [Obsolete("Inefficient, terrible code. Riddled with bugs. Replaced by Uchuumaru.MyAnimeList.")]
    public class VerificationFailedException : Exception
    {
        public VerificationFailedException(IUser user)
            : base($"{user}, verification process failed. This is likely because your MAL account location is not set to your Verification Code.")
        {
        }
    }
}