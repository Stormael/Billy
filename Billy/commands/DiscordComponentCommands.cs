using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace Billy.commands
{
    public class DiscordComponentCommands : BaseCommandModule
    {
        [Command("dropdown")]
        public async Task Dropdown(CommandContext ctx)
        {
            ulong requiredUserId = 692805583446868008;
            if (ctx.User.Id != requiredUserId)
            {
                await ctx.Channel.SendMessageAsync("You do not have permission to use this command!");
                return;
            }

            List<DiscordSelectComponentOption> optionList = new List<DiscordSelectComponentOption>();
            optionList.Add(new DiscordSelectComponentOption("Option 1", "option1"));
            optionList.Add(new DiscordSelectComponentOption("Option 2", "option2"));
            optionList.Add(new DiscordSelectComponentOption("Option 3", "option3"));

            var options = optionList.AsEnumerable();

            var dropDown = new DiscordSelectComponent("dropdown", "Select...", options);

            var dropdownMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Dropdown Test")
                    .WithColor(DiscordColor.Black))
                .AddComponents(dropDown);
            await ctx.Channel.SendMessageAsync(dropdownMessage);
        }
        [Command("portal")]
        public async Task CList(CommandContext ctx)
        {
            ulong requiredUserId = 692805583446868008;
            if (ctx.User.Id != requiredUserId)
            {
                await ctx.Channel.SendMessageAsync("You do not have permission to use this command!");
                return;
            }

            var channelComponent = new DiscordChannelSelectComponent("channelDropdownList", "Select...");

            var dropdownMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Select a channel")
                    .WithColor(DiscordColor.Black))
                .AddComponents(channelComponent);
            await ctx.Channel.SendMessageAsync(dropdownMessage);
        }
        [Command("uInfo")]
        public async Task MList(CommandContext ctx)
        {
            ulong requiredUserId = 692805583446868008;
            if (ctx.User.Id != requiredUserId)
            {
                await ctx.Channel.SendMessageAsync("You do not have permission to use this command!");
                return;
            }

            var mentionComponent = new DiscordMentionableSelectComponent("mentionList", "Select...");

            var dropdownMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithTitle("Mention-List Test")
                    .WithColor(DiscordColor.Black))
                .AddComponents(mentionComponent);
            await ctx.Channel.SendMessageAsync(dropdownMessage);
        }
    }
}
