using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Hangfire;
using Interactivity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Uchuumaru.Data;
using Uchuumaru.Services;
using Uchuumaru.Services.Guilds;
using Uchuumaru.Services.Infractions;
using Uchuumaru.Services.Users;

namespace Uchuumaru
{
    public class Program
    {
        public static async Task Main(string[] args) => await Host.CreateDefaultBuilder()
            .UseSerilog((_, configuration) =>
            {
                configuration
                    .Enrich.FromLogContext()
                    .MinimumLevel.Information()
                    .WriteTo.Console(theme: AnsiConsoleTheme.Literate);
            })
            .ConfigureAppConfiguration((_, builder) =>
            {
                builder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false, true);
            })
            .ConfigureServices((context, collection) =>
            {
                var client = new DiscordSocketClient(new DiscordSocketConfig
                {
                    AlwaysDownloadUsers = true,
                    MessageCacheSize = 10000,
                    LogLevel = LogSeverity.Verbose
                });

                var commands = new CommandService(new CommandServiceConfig
                {
                    DefaultRunMode = RunMode.Sync,
                    LogLevel = LogSeverity.Verbose,
                    ThrowOnError = true
                });

                var interactivity = new InteractivityService(client);

                collection
                    .AddMediatR(typeof(Program).Assembly)
                    .AddSingleton(client)
                    .AddSingleton(interactivity)
                    .AddSingleton(provider =>
                    {
                        commands.AddModulesAsync(Assembly.GetEntryAssembly(), provider);
                        return commands;
                    })
                    .AddDbContext<UchuumaruContext>(options =>
                    {
                        options.UseNpgsql(context.Configuration["Postgres:Connection"]);
                    })
                    .AddGuild()
                    .AddFilter()
                    .AddInfractions()
                    .AddUsers()
                    .AddHostedService<Startup>()
                    .AddHostedService<DiscordListener>()
                    .AddHostedService<AutoRestartService>()
                    .AddSingleton(new InteractivityService(client, TimeSpan.FromSeconds(30)));
            })
            .RunConsoleAsync();
    }
}