﻿using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Interactivity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Uchuumaru.Data;
using Uchuumaru.MyAnimeList.Parsers;
using Uchuumaru.Services;
using Uchuumaru.Services.Birthdays;
using Uchuumaru.Services.Guilds;
using Uchuumaru.Services.Infractions;
using Uchuumaru.Services.Infractions.Reports;
using Uchuumaru.Services.MAL;
using Uchuumaru.Services.MyAnimeList;
using Uchuumaru.Services.Users;
using Uchuumaru.TypeReaders;
using IVerificationService = Uchuumaru.Services.MyAnimeList.IVerificationService;
using ProfileParser = Uchuumaru.MyAnimeList.Parsers.ProfileParser;
using VerificationService = Uchuumaru.Services.MyAnimeList.VerificationService;

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
                    .WriteTo.Console(theme: SystemConsoleTheme.Literate);
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
                    LogLevel = LogSeverity.Info
                });

                var commands = new CommandService(new CommandServiceConfig
                {
                    DefaultRunMode = RunMode.Sync,
                    LogLevel = LogSeverity.Info,
                    ThrowOnError = true
                });

                var interactivity = new InteractivityService(client, TimeSpan.FromSeconds(60));

                collection
                    .AddMediatR(typeof(Program).Assembly)
                    .AddSingleton(client)
                    .AddSingleton(interactivity)
                    .AddSingleton(provider =>
                    {
                        commands.AddTypeReader(typeof(DateTime), new DateTimeTypeReader());
                        commands.AddModulesAsync(Assembly.GetEntryAssembly(), provider);
                        return commands;
                    })
                    .AddDbContext<UchuumaruContext>(options =>
                    {
                        options.UseNpgsql(context.Configuration["Postgres:Connection"]);
                    })
                    .AddGuilds()
                    .AddFilters()
                    .AddInfractions()
                    .AddUsers()
                    .AddBirthdays()
                    .AddReports()
                    .AddMyAnimeList()
                    .AddHostedService<StartupHostedService>()
                    .AddHostedService<DiscordHostedService>()
                    .AddHostedService<BirthdayHostedService>();
            }).RunConsoleAsync();
    }
}