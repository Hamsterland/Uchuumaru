using System;

namespace Uchuumaru.MyAnimeList.Parsers
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class DocumentPropertyAttribute : Attribute
    {
        
    }

    [AttributeUsage(AttributeTargets.Method)]
    internal class ClearfixPropertyAttribute : Attribute
    {
    }
}