namespace Uchuumaru.Services.MyAnimeList
{
    public class VerificationResult
    {
        public bool IsSuccess { get; init; }
        
        public VerificationError? Error { get; init; }
        public string ErrorReason { get; init; }
        
        public static VerificationResult FromSuccess()
        {
            return new() { IsSuccess = true };
        }

        public static VerificationResult FromError(VerificationError error, string errorReason)
        {
            return new()
            {
                IsSuccess = false,
                Error = error,
                ErrorReason = errorReason
            };
        }
    }
}