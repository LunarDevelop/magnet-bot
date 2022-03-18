﻿using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using sancus;
using System;
using System.IO;

namespace Bot
{
    public class Program
    {
        public static DiscordSocketClient client = new DiscordSocketClient();
        public static SocketGuild test_guild = client.GetGuild(780211278614364160);

        public static Task Main(string[] args) => new Program().MainAsync();

        public async Task MainAsync()
        {
            var root = Directory.GetCurrentDirectory();
            var dotenv = Path.Combine(root, ".env");
            DotEnv.Load(dotenv);

            client.Log += Log;
            
            var token = Environment.GetEnvironmentVariable("magnet-token");

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            client.Ready += Client_Ready;

            await client.SetStatusAsync(UserStatus.DoNotDisturb);
            await client.SetGameAsync("TESTING SYSTEMS");

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        public async Task Client_Ready()
        {
            await test_guild.GetTextChannel(873389212857139223).SendMessageAsync(client.CurrentUser.Username + " is now online");
        }
    }
}