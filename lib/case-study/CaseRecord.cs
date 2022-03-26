using Discord;
using Discord.WebSocket;

namespace magnet_bot.case_study
{
    public class CaseRecord
    {
        private SocketGuildUser user;
        private SocketGuild guild;
        private ulong number;
        private CaseType caseType;
        private string reason = "No Reason Given";

        private SocketGuildUser User { get { return user; } }
        private SocketGuild Guild { get { return guild; } }
        private ulong CaseNumber { get { return number; }  }
        private CaseType CaseType { get { return caseType; } }
        private string? Reason { get { return reason; } }

        public CaseRecord(ulong number)
        {
            this.number = number;
        }

        public CaseRecord WithUser(SocketGuildUser user) { this.user = user; return this; }
        public CaseRecord WithGuild(SocketGuild guild) { this.guild = guild; return this; }
        public CaseRecord WithCaseType(CaseType caseType) { this.caseType = caseType; return this; }
        public CaseRecord WithReason(string reason) { this.reason = reason; return this; }

        private EmbedBuilder SendErrorCase()
        {
            EmbedBuilder embed = new EmbedBuilder()
                .WithColor(Color.DarkTeal)
                .WithTitle($"{Emoji.Parse(":interrobang:")} Error Error")
                .WithDescription("For some reason, the event was not on my list. This error will be reported to my developer")
                .WithCurrentTimestamp();
            return embed;
        }
        private EmbedBuilder SendWarnCase()
        {
            EmbedBuilder embed = new EmbedBuilder()
                .WithColor(Color.Gold)
                .WithTitle($"{new Emoji("⚠️")} Warning\t|\tCase {this.CaseNumber} ")
                .WithDescription($"<:user:912027882799374356>\t{User.Username}#{User.Discriminator} \n <:idemoji:912034927443320862>\t{User.Id}")
                .AddField(new EmbedFieldBuilder()
                    .WithName("Reason")
                    .WithValue(Reason)
                    .WithIsInline(true))
                .WithCurrentTimestamp();
            return embed;
        }
        private EmbedBuilder SendWarn2Case()
        {
            EmbedBuilder embed = new EmbedBuilder()
                .WithColor(Color.Orange)
                .WithTitle($"{new Emoji("⚠️")} Second Warning\t|\tCase {this.CaseNumber} ")
                .WithDescription($"<:user:912027882799374356>\t{User.Username}#{User.Discriminator} \n <:idemoji:912034927443320862>\t{User.Id}")
                .AddField(new EmbedFieldBuilder()
                    .WithName("Reason")
                    .WithValue(Reason)
                    .WithIsInline(true))
                .WithCurrentTimestamp();
            return embed;
        }
        private EmbedBuilder SendTimeOutCase()
        {
            EmbedBuilder embed = new EmbedBuilder()
                .WithColor(Color.DarkOrange)
                .WithTitle($"{new Emoji("⏰")} Timed Out\t|\tCase {this.CaseNumber} ")
                .WithDescription($"<:user:912027882799374356>\t{User.Username}#{User.Discriminator} \n <:idemoji:912034927443320862>\t{User.Id}")
                .AddField(new EmbedFieldBuilder()
                    .WithName("Reason")
                    .WithValue(Reason)
                    .WithIsInline(true))
                .WithCurrentTimestamp();
            return embed;
        }
        private EmbedBuilder SendKickCase()
        {
            EmbedBuilder embed = new EmbedBuilder()
                .WithColor(Color.Red)
                .WithTitle($"{new Emoji("👢")} Kick\t|\tCase {this.CaseNumber} ")
                .WithDescription($"<:user:912027882799374356>\t{User.Username}#{User.Discriminator} \n <:idemoji:912034927443320862>\t{User.Id}")
                .AddField(new EmbedFieldBuilder()
                    .WithName("Reason")
                    .WithValue(Reason)
                    .WithIsInline(true))
                .WithCurrentTimestamp();
            return embed;
        }
        private EmbedBuilder SendBanCase()
        {
            EmbedBuilder embed = new EmbedBuilder()
                .WithColor(Color.DarkRed)
                .WithTitle($"{new Emoji("⛔")} Ban\t|\tCase {this.CaseNumber} ")
                .WithDescription($"<:user:912027882799374356>\t{User.Username}#{User.Discriminator} \n <:idemoji:912034927443320862>\t{User.Id}")
                .AddField(new EmbedFieldBuilder()
                    .WithName("Reason")
                    .WithValue(Reason)
                    .WithIsInline(true))
                .WithCurrentTimestamp();
            return embed;
        }
        private EmbedBuilder SendAppealCase()
        {
            EmbedBuilder embed = new EmbedBuilder()
                .WithColor(Color.LightGrey)
                .WithTitle($"{new Emoji("")} Appeal Request\t|\tCase {this.CaseNumber} ")
                .WithDescription($"<:user:912027882799374356>\t{User.Username}#{User.Discriminator} \n <:idemoji:912034927443320862>\t{User.Id}")
                .AddField(new EmbedFieldBuilder()
                    .WithName("Reason")
                    .WithValue(Reason)
                    .WithIsInline(true))
                .WithCurrentTimestamp();
            return embed;
        }
        private EmbedBuilder SendAcceptedAppealCase()
        {
            EmbedBuilder embed = new EmbedBuilder()
                .WithColor(Color.Green)
                .WithTitle($"{Emoji.Parse(":green_cicle:")} Accepted Appeal\t|\tCase {this.CaseNumber} ")
                .WithDescription($"<:user:912027882799374356>\t{User.Username}#{User.Discriminator} \n <:idemoji:912034927443320862>\t{User.Id}")
                .AddField(new EmbedFieldBuilder()
                    .WithName("Reason")
                    .WithValue(Reason)
                    .WithIsInline(true))
                .WithCurrentTimestamp();
            return embed;
        }
        private EmbedBuilder SendDeniedAppealCase()
        {
            EmbedBuilder embed = new EmbedBuilder()
                .WithColor(Color.Blue)
                .WithTitle($"{Emoji.Parse(":red_cicle:")} Ban\t|\tCase {this.CaseNumber} ")
                .WithDescription($"<:user:912027882799374356>\t{User.Username}#{User.Discriminator} \n <:idemoji:912034927443320862>\t{User.Id}")
                .AddField(new EmbedFieldBuilder()
                    .WithName("Reason")
                    .WithValue(Reason)
                    .WithIsInline(true))
                .WithCurrentTimestamp();
            return embed;
        }

        public EmbedBuilder Build()
        {
            switch (this.CaseType)
            {
                case CaseType.Warning:
                    return SendWarnCase();

                case CaseType.SecondWarning:
                    return SendWarn2Case();

                case CaseType.Timeout:
                    return SendTimeOutCase();

                case CaseType.Kick:
                    return SendKickCase();

                case CaseType.Ban:
                    return SendBanCase();

                case CaseType.Appeal:
                    return SendAppealCase();

                case CaseType.AcceptedAppeal:
                    return SendAcceptedAppealCase();

                case CaseType.DeniedAppeal:
                    return SendDeniedAppealCase();

                default:
                    return SendErrorCase();
            }
        }
    }
    public enum CaseType
    {
        Warning,
        SecondWarning,
        Timeout,
        Kick,
        Ban,
        Appeal,
        AcceptedAppeal,
        DeniedAppeal,
    }
}
