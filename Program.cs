﻿using Discord;
using Discord.WebSocket;
using Bot.Commands;

namespace Bot
{
    public class Program
    {
        public static DiscordSocketClient client = new DiscordSocketClient();
        private SlashCommandsRegister SlashRegister = new SlashCommandsRegister();

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
            //Register Commands
            await SlashRegister.SlashRegisterAsync(client);
        }
    }
}