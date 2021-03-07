namespace Uchuumaru.Services.MyAnimeList
{
    public class VerificationResult
    {
        public bool IsSuccess { get; init; }
        public string ErrorReason { get; init; }

        public static VerificationResult FromSuccess()
        {
            return new() { IsSuccess = true };
        }

        public static VerificationResult FromError(string errorReason)
        {
            return new()
            {
                IsSuccess = false,
                ErrorReason = errorReason
            };
        }
    }
}