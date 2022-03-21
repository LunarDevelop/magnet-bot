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

                case "role":
                    await RoleCmd(cmd);
                    break;

                default:
                    await cmd.RespondAsync($"Cannot find Slash Command with the name, {cmd.Data.Name} on the latest version of me.",ephemeral:true);
                    break;
            }
        }

        public async Task RoleCmd(SocketSlashCommand cmd)
        {
            switch (cmd.Data.Options.First().Name)
            {
                case "add":
                    var AddUsr = (SocketGuildUser) cmd.Data.Options.First().Options.First().Value;
                    var AddRole = (SocketRole)cmd.Data.Options.First().Options.Last().Value;

                    await AddUsr.AddRoleAsync(AddRole);
                    await cmd.RespondAsync($"{AddRole.Mention} has been added to {AddUsr.Mention}");
                    break;
                
                case "remove":
                    var RemoveUsr = (SocketGuildUser)cmd.Data.Options.First().Options.First().Value;
                    var RemoveRole = (SocketRole)cmd.Data.Options.First().Options.Last().Value;

                    await RemoveUsr.RemoveRoleAsync(RemoveRole);
                    await cmd.RespondAsync($"{RemoveRole.Mention} has been remove from {RemoveUsr.Mention}");
                    break;
            }
        }
    }
}