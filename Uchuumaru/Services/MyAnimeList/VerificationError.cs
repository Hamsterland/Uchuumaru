namespace Uchuumaru.Services.MyAnimeList
{
    /// <summary>
    /// The type of errors encountered for verification.
    /// </summary>
    public enum VerificationError
    {
        /// <summary>
        /// The account is too young.
        /// </summary>
        AccountAge,
        
        /// <summary>
        /// The account is too inactive.
        /// </summary>
        AccountActivity,
        
        /// <summary>
        /// The location field was not correct.
        /// </summary>
        InvalidLocation,
        
        /// <summary>
        /// The lists are private.
        /// </summary>
        PrivateLists,
        
        /// <summary>
        /// Web-based error.
        /// </summary>
        Http
    }
}