using Discord;
using Discord.Net;
using Discord.WebSocket;

using Bot;
using Newtonsoft.Json;

namespace Bot.lib.commands
{
    public class SlashCommandsRegister
    {
        List<SlashCommandBuilder> CmdList = new List<SlashCommandBuilder>();

        
        SlashCommandBuilder SayCmd = new SlashCommandBuilder()
            .WithName("Say")
            .WithDescription("Get the" + Program.client.CurrentUser.Username +"to say something in this channel.");
            
        public async Task SlashRegisterAsync()
        {
            CmdList.Add(SayCmd);

            // Register Commands
            try
            {
                // Now that we have our builder, we can call the CreateApplicationCommandAsync method to make our slash command.
                foreach (SlashCommandBuilder cmd in CmdList)
                {
                    await Program.test_guild.CreateApplicationCommandAsync(cmd.Build());
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