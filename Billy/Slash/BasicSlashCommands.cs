using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;

namespace Billy.Slash
{
    public class BasicSlashCommands : ApplicationCommandModule
    {
        private ulong YourDiscordId = 692805583446868008; // Replace 1234567890 with your Discord ID
        /*
        [SlashCommand("embed", "No description provided.")]
        public async Task MyFirstSlashCommand(InteractionContext ctx)
        {
            // Check if the user's ID matches your Discord ID
            if (ctx.User.Id != YourDiscordId)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("You do not have permission to use this command!"));
                return;
            }
            await ctx.DeferAsync();
           
            var footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = $"This command was executed by {ctx.User.Username}"
            };
            
            var author = new DiscordEmbedBuilder.EmbedAuthor
            {
                Name = ctx.User.Username,
                IconUrl = ctx.User.AvatarUrl,
            };

            var embed = new DiscordEmbedBuilder
            {
                Title = $"Test Embed",
                Footer = footer,
                Description = $"This command was executed by {ctx.User.Username}",
                Author = author
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
        }
        */
        /*
        [SlashCommand("parameters", "No description provided.")]
        public async Task Para(InteractionContext ctx, [Option("testoption", "Type in anything")] string testPara, [Option("testnum", "Type in anything")] long testNumber)
        {
            await ctx.DeferAsync();

            var embedMessage = new DiscordEmbedBuilder
            {
                Title = "Test Embed",
                Color = DiscordColor.Green,
                Description = $"{testPara} {testNumber}"
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }
        */
        /*
        [SlashCommand("dpara", "No description provided.")]
        public async Task DPara(InteractionContext ctx, [Option("user", "Mention a user")] DiscordUser user, [Option("file", "Upload a file here")] DiscordAttachment file)
        {
            await ctx.DeferAsync();

            var mem = (DiscordMember)user;

            var embedMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Title = "New Dc Embed",
                Description = $"{mem.Username} {file.FileName} {file.FileSize}"
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }
        */
        [SlashCommand("suggest", "No description provided.")]
        public async Task Modal(InteractionContext ctx)
        {
            var modal = new DiscordInteractionResponseBuilder()
                .WithTitle("Create a Suggestion")
                .WithCustomId("modal")
                .AddComponents(new TextInputComponent("Suggestion", "randomTextBox", "Type Something Here"));

            await ctx.CreateResponseAsync(InteractionResponseType.Modal, modal);
        }
    }
}
