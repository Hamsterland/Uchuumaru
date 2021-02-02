using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord.Commands;

namespace Uchuumaru.TypeReaders
{
    public class DateTimeTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            var cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            var success = DateTime.TryParse(input, cultureInfo, DateTimeStyles.None, out var date);
            return Task.FromResult(success 
                ? TypeReaderResult.FromSuccess(date) 
                : TypeReaderResult.FromError(CommandError.ParseFailed, "Input could not be parsed as DateTime."));
        }
    }
}