using Discord.WebSocket;
using Discord;
using NekosSharp;

namespace Bot.Commands
{
    public class SlashCommandsHandler
    {
        private DiscordSocketClient client;
        public static NekoClient NekoClient = new NekoClient("Magnet");
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

                case "neko":
                    await NekoCmd(cmd);
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

            SocketTextChannel TxtChannel = (SocketTextChannel) client.GetChannel(cmd.Channel.Id);
            SocketGuildUser CmdUser = TxtChannel.GetUser(cmd.User.Id);

            if (!CmdUser.GuildPermissions.Has(GuildPermission.ManageRoles))
            {
                await cmd.RespondAsync("You do not have permission to do that", ephemeral: true);
                return;
            }

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
        
        public async Task NekoCmd(SocketSlashCommand cmd)
        {
            Request req;
            EmbedBuilder embed;

            switch (cmd.Data.Options.First().Name)
            {
                case "neko":
                    req = await NekoClient.Image_v3.Neko();
                    embed = new EmbedBuilder()
                        .WithImageUrl(req.ImageUrl);

                    await cmd.RespondAsync(embed: embed.Build());
                    break;

                case "misc":
                    switch (cmd.Data.Options.First().Options.First().Name)
                    {
                        case "cat":
                            req = await NekoClient.Misc_v3.Cat();
                            embed = new EmbedBuilder()
                                .WithImageUrl(req.ImageUrl);

                            await cmd.RespondAsync(embed: embed.Build());
                            break;
                        case "8ball":
                            req = await NekoClient.Misc_v3.EightBall();
                            embed = new EmbedBuilder()
                                .WithImageUrl(req.ImageUrl);

                            await cmd.RespondAsync(embed: embed.Build());
                            break;

                    }
                    break;

                case "nsfw":
                    SocketTextChannel channel = (SocketTextChannel) client.GetChannel(cmd.Channel.Id);
                    if (!channel.IsNsfw)
                    {
                        await cmd.RespondAsync("This is not an NSFW channel, please try there first!", ephemeral:true);
                        return;
                    };

                    switch (cmd.Data.Options.First().Options.First().Name)
                    {
                        case "pussy":
                            req = await NekoClient.Nsfw_v3.Pussy();
                            embed = new EmbedBuilder()
                                .WithImageUrl(req.ImageUrl);

                            await cmd.RespondAsync(embed: embed.Build());
                            break;
                        case "cum":
                            req = await NekoClient.Nsfw_v3.Cum();
                            embed = new EmbedBuilder()
                                .WithImageUrl(req.ImageUrl);

                            await cmd.RespondAsync(embed: embed.Build());
                            break;
                        case "bdsm":
                            req = await NekoClient.Nsfw_v3.Bdsm();
                            embed = new EmbedBuilder()
                                .WithImageUrl(req.ImageUrl);

                            await cmd.RespondAsync(embed: embed.Build());
                            break;
                        case "gif-spank":
                            req = await NekoClient.Nsfw_v3.SpankGif();
                            embed = new EmbedBuilder()
                                .WithImageUrl(req.ImageUrl);

                            await cmd.RespondAsync(embed: embed.Build());
                            break;
                        case "ero-neko":
                            req = await NekoClient.Nsfw_v3.EroNeko();
                            embed = new EmbedBuilder()
                                .WithImageUrl(req.ImageUrl);

                            await cmd.RespondAsync(embed: embed.Build());
                            break;
                    }
                    break;
            }

        }
    }

}