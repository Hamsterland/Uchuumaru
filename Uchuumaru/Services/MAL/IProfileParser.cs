using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace Uchuumaru.Services.MAL
{
    /// <summary>
    /// Describes a service that scrapes a MAL profile.
    /// </summary>
    public interface IProfileParser
    {
        /// <summary>
        /// Parses a profile.
        /// </summary>
        /// <param name="username">The username of the profile to parse.</param>
        /// <returns>
        /// The <see cref="Profile"/>.
        /// </returns>
        Task<Profile> Parse(string username);
        
        /// <summary>
        /// Parses MAL profile HTML to an <see cref="IHtmlDocument"/>.
        /// </summary>
        /// <param name="html">The MAL profile HTML.</param>
        /// <returns>
        /// The MAL profile HTML in an <see cref="IHtmlDocument"/>.
        /// </returns>
        Task<IHtmlDocument> ParseDocumentAsync(string html);
        
        /// <summary>
        /// Downloads the MAL profile HTML for a user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// A HTML <see cref="string"/> of the user's MAL profile.
        /// </returns>
        Task<string> DownloadHtml(string username);
        
        /// <summary>
        /// Checks if an element in specified from a list of elements.
        /// </summary>
        /// <param name="elements">The list of elements to check from.</param>
        /// <param name="argument">The name of the element to see if it exists.</param>
        /// <returns>
        /// A <see cref="bool"/> indicating whether the element is specified.
        /// </returns>
        bool IsElementSpecified(IEnumerable<IElement> elements, string argument);
        
        /// <summary>
        /// Gets the username of an account from their MAL ID.
        /// </summary>
        /// <param name="id">The MAL ID.</param>
        /// <returns>
        /// The username belonging to the specified MAL ID.
        /// </returns>
        Task<string> GetUsernameFromId(int id);
        
        /// <summary>
        /// Gets the image URL.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>
        /// The image URL.
        /// </returns>
        string GetImageUrl(IDocument document);
        
        /// <summary>
        /// Gets the user ID from an image URL.
        /// </summary>
        /// <param name="imageUrl">The image URL.</param>
        /// <returns>
        /// The user ID.
        /// </returns>
        int GetUserId(string imageUrl);
        
        /// <summary>
        /// Gets the MAL Supporter status.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>
        /// The MAL Supporter status,
        /// </returns>
        bool GetSupporter(IDocument document);
        
        /// <summary>
        /// Gets the anime mean score.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>
        /// The anime mean score.
        /// </returns>
        double GetScore(IDocument document);
        
        /// <summary>
        /// Gets the clearfix elements.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> of clearfix elements.
        /// </returns>
        IEnumerable<IElement> GetClearfixElements(IDocument document);
        
        /// <summary>
        /// Gets the last online time.
        /// </summary>
        /// <param name="elements">The clearfix elements.</param>
        /// <returns>
        /// The last online tine.
        /// </returns>
        string GetLastOnline(IEnumerable<IElement> elements);
        
        
        /// <summary>
        /// Gets the gender.
        /// </summary>
        /// <param name="elements">The clearfix elements.</param>
        /// <returns>
        /// The gender.
        /// </returns>
        Gender GetGender(List<IElement> elements);
        
        /// <summary>
        /// Gets the birthday.
        /// </summary>
        /// <param name="elements">The clearfix elements.</param>
        /// <returns>
        /// The birthday. 
        /// </returns>
        string GetBirthday(List<IElement> elements);
        
        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <param name="elements">The clearfix elements.</param>
        /// <returns>
        /// The location.
        /// </returns>
        string GetLocation(List<IElement> elements);
        
        /// <summary>
        /// Gets the date joined.
        /// </summary>
        /// <param name="elements">The clearfix elements.</param>
        /// <returns>
        /// The date joined.
        /// </returns>
        DateTime GetDateJoined(List<IElement> elements);
    }
}