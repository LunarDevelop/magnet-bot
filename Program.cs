using Discord;
using Discord.WebSocket;
using Bot.Commands;
using MySql.Data.MySqlClient;

namespace Bot
{
    public class Program
    {
        public static DiscordSocketClient client = new DiscordSocketClient();
        private SlashCommandsRegister SlashRegister = new SlashCommandsRegister(client);
        private SlashCommandsHandler SlashHandler = new SlashCommandsHandler(client);

        public DBConnection dBConnection;

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
            client.SlashCommandExecuted += SlashHandler.Handler;

            await client.SetStatusAsync(UserStatus.Online);
            await client.SetGameAsync(Environment.GetEnvironmentVariable("status-message"));

            dBConnection = new DBConnection();

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
            try 
            { 
                var TestServer = client.GetGuild(Convert.ToUInt64(Environment.GetEnvironmentVariable("test-guild")));

                // Removes all slash commands from my test server to ensure it all is current commands
                await TestServer
                    .DeleteApplicationCommandsAsync();

                //Alerts my test server that the bot is online
                //TODO this will be changed to Lunar-dev notification channel once live
                var LoadMsg = new EmbedBuilder()
                        .WithTitle($"{client.CurrentUser.Username} loaded successfully")
                        .WithColor(Color.DarkPurple)
                        .WithCurrentTimestamp();

                await TestServer.GetTextChannel(
                    Convert.ToUInt64(Environment.GetEnvironmentVariable("notification-channel")))
                    .SendMessageAsync(embed: LoadMsg.Build());

                //Register Commands
                await SlashRegister.SlashRegisterAsync(); 
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }

            try
            {

                dBConnection.OpenConnection();

                MySqlDataReader result = dBConnection.SelectQueryExecutor("*", "new_table", null);

                while (result.Read())
                {
                    Console.WriteLine(result.GetString(1));
                }
                dBConnection.CloseConnection();
            }
            catch (Exception ex) { Console.Error.WriteLine(ex.ToString()); }
        }
    }
}