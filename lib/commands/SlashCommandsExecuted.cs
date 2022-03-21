using Discord.WebSocket;
using Discord;

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
            SocketRole Role;
            SocketGuildUser Usr;

            switch (cmd.Data.Options.First().Name)
            {
                case "add":
                    Usr = (SocketGuildUser) cmd.Data.Options.First().Options.First().Value;
                    Role = (SocketRole)cmd.Data.Options.First().Options.Last().Value;

                    await Usr.AddRoleAsync(Role);
                    await cmd.RespondAsync($"{Role.Mention} has been added to {Usr.Mention}");
                    break;
                
                case "remove":
                    Usr = (SocketGuildUser)cmd.Data.Options.First().Options.First().Value;
                    Role = (SocketRole)cmd.Data.Options.First().Options.Last().Value;

                    await Usr.RemoveRoleAsync(Role);
                    await cmd.RespondAsync($"{Role.Mention} has been remove from {Usr.Mention}");
                    break;
                
                case "info":
                    Role = (SocketRole) cmd.Data.Options.First().Options.First().Value;
                    var Embed = new EmbedBuilder()
                        .WithTitle($"{Role.Name} Information")
                        .AddField(new EmbedFieldBuilder()
                            .WithName("User Count")
                            .WithValue($"Count: {Role.Members.Count()}")
                            .WithIsInline(true)
                            )
                        .AddField(new EmbedFieldBuilder()
                            .WithName("ID")
                            .WithValue(Role.Id)
                            .WithIsInline(true));
                    await cmd.RespondAsync(embed: Embed.Build());
                    break;

            }
        }
    }
}