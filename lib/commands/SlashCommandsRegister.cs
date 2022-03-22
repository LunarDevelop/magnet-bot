using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace Bot.Commands
{
    public class SlashCommandsRegister
    {
        List<SlashCommandBuilder> TestCmdList = new List<SlashCommandBuilder>();
        List<SlashCommandBuilder> GlobalCmdList = new List<SlashCommandBuilder>();
        List<SlashCommandBuilder> LunarCmdList = new List<SlashCommandBuilder>();
        List<SlashCommandBuilder> WaifuCmdList = new List<SlashCommandBuilder>();

        DiscordSocketClient client;

        public async Task SlashRegisterAsync(DiscordSocketClient client)
        {
            this.client = client;

            // Say Command
            SlashCommandBuilder SayCmd = new SlashCommandBuilder()
            .WithName("say")
            .WithDescription("Get " + client.CurrentUser.Username + " to say something in this channel.")
            .AddOption(new SlashCommandOptionBuilder()
                .WithName("message")
                .WithDescription($"What do you want {client.CurrentUser.Username} to say")
                .WithType(ApplicationCommandOptionType.String)
                .WithRequired(true));

            GlobalCmdList.Add(SayCmd);

            // Role Commands
            SlashCommandBuilder RoleCmd = new SlashCommandBuilder()
                .WithName("role")
                .WithDescription("Commands to help with some role assignments")
                .AddOption(new SlashCommandOptionBuilder()
                        .WithName("add")
                        .WithDescription("Add a role to a user")
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .AddOption("user", ApplicationCommandOptionType.User, "The user to add a role to", isRequired: true)
                        .AddOption("role", ApplicationCommandOptionType.Role, "The role to add to the user", isRequired: true))
                .AddOption(new SlashCommandOptionBuilder()
                        .WithName("remove")
                        .WithDescription("Remove a role from a user")
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .AddOption("user", ApplicationCommandOptionType.User, "The user you want to remove a role from", isRequired: true)
                        .AddOption("role", ApplicationCommandOptionType.Role, "The role to remove", isRequired: true))
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("info")
                    .WithDescription("Show some information about a role")
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .AddOption("role", ApplicationCommandOptionType.Role, "The role you would like information about", isRequired:true));

            GlobalCmdList.Add(RoleCmd);

            await RegisterCmds();
        }

        private async Task RegisterCmds() {
            try
            {
                // Build the global commands
                foreach (SlashCommandBuilder cmd in GlobalCmdList)
                {
                    await client.CreateGlobalApplicationCommandAsync(cmd.Build());
                }

                // Build the test server commands
                var TestServer = client.GetGuild(Convert.ToUInt64(Environment.GetEnvironmentVariable("test-server")));
                foreach (SlashCommandBuilder cmd in TestCmdList)
                {
                    await TestServer.CreateApplicationCommandAsync(cmd.Build());
                }

                // Build the Lunar server commands
                var LunarServer = client.GetGuild(Convert.ToUInt64(Environment.GetEnvironmentVariable("lunar-server")));
                foreach (SlashCommandBuilder cmd in TestCmdList)
                {
                    await LunarServer.CreateApplicationCommandAsync(cmd.Build());
                }

                // Build the Waifu server commands
                var WaifuServer = client.GetGuild(Convert.ToUInt64(Environment.GetEnvironmentVariable("waifu-server")));
                foreach (SlashCommandBuilder cmd in TestCmdList)
                {
                    await WaifuServer.CreateApplicationCommandAsync(cmd.Build());
                }

            }
            catch (HttpException exception)
            {
                // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

                // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
                Console.WriteLine(json);
            }
        }
    }
}