using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace Bot.Commands
{
    public class SlashCommandsRegister
    {
        List<SlashCommandBuilder> CmdList = new List<SlashCommandBuilder>();


        public async Task SlashRegisterAsync(DiscordSocketClient client)
        {

            // Say Command
            SlashCommandBuilder SayCmd = new SlashCommandBuilder()
            .WithName("say")
            .WithDescription("Get the" + client.CurrentUser.Username + "to say something in this channel.");

            CmdList.Add(SayCmd);

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
                        .AddOption("role", ApplicationCommandOptionType.Role, "The role to remove", isRequired: true));

            CmdList.Add(RoleCmd);

            // Register Commands
            try
            {
                // Now that we have our builder, we can call the CreateApplicationCommandAsync method to make our slash command.
                foreach (SlashCommandBuilder cmd in CmdList)
                {
                    await client.GetGuild(
                        Convert.ToUInt64(Environment.GetEnvironmentVariable("test-guild")))
                        .CreateApplicationCommandAsync(cmd.Build());
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