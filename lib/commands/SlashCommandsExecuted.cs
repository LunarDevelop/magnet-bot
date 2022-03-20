using Discord.WebSocket;

namespace Bot.Commands
{
    public class SlashCommandsHandler
    {
        private DiscordSocketClient client;
        public SlashCommandsHandler(DiscordSocketClient client)
        {
            this.client = client;
        }

        public async Task Handler(SocketSlashCommand cmd)
        {
            switch (cmd.Data.Name)
            {
                case "say":
                    await cmd.RespondAsync(cmd.Data.Options.First().Value.ToString());
                    break;
                    
                default:
                    await cmd.RespondAsync($"Cannot find Slash Command with the name, {cmd.Data.Name}");
                    break;
            }
        }
    }
}