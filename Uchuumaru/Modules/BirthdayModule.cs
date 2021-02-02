using System;
using System.Threading.Tasks;
using Discord.Commands;
using Uchuumaru.Services.Birthdays;

namespace Uchuumaru.Modules
{
    [Name("Birthday")]
    [Group("birthday")]
    [Summary("Happy Birthday!")]
    public class BirthdayModule : ModuleBase<SocketCommandContext>
    {
        private readonly IBirthdayService _birthday;

        public BirthdayModule(IBirthdayService birthday)
        {
            _birthday = birthday;
        }

        [Command("set")]
        [Summary("Sets a birthday.")]
        public async Task Set(DateTime date)
        {
            var utc = date.ToUniversalTime();
            await _birthday.SetBirthday(Context.User.Id, utc);
        }

        [Command("remove")]
        [Summary("Removes your birthday.")]
        public async Task Remove()
        {
            // default is 01/01/0001 00:00:00
            await _birthday.SetBirthday(Context.User.Id, default);
        }
    }
}