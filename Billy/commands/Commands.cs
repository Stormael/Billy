using Billy.cards;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Billy.commands
{
    public class Commands : BaseCommandModule
    {
        [Command("ping")]
        public async Task Ping(CommandContext ctx)
        {
            ulong requiredUserId = 692805583446868008;
            if (ctx.User.Id != requiredUserId)
            {
                await ctx.Channel.SendMessageAsync("You do not have permission to use this command!");
                return;
            }

            await ctx.Channel.SendMessageAsync($"pong {ctx.User.Mention}");
        }
        [Command("add")]
        public async Task Add(CommandContext ctx, int n1 = 0, int n2 = 0)
        {
            // Check if the user invoking the command has the required user ID
            ulong requiredUserId = 692805583446868008; 
            if (ctx.User.Id != requiredUserId)
            {
                await ctx.Channel.SendMessageAsync("You do not have permission to use this command!");
                return;
            }

            // Check if parameters are provided
            if (n1 == 0 || n2 == 0)
            {
                await ctx.Channel.SendMessageAsync("Please enter 2 numbers!");
                return;
            }

            // Perform addition
            int result = n1 + n2;
            await ctx.Channel.SendMessageAsync(result.ToString());
        }

        [Command("subtract")]
        public async Task Subtract(CommandContext ctx, int n1 = 0, int n2 = 0)
        {
            ulong requiredUserId = 692805583446868008;
            if (ctx.User.Id != requiredUserId)
            {
                await ctx.Channel.SendMessageAsync("You do not have permission to use this command!");
                return;
            }

            if (n1 == 0 || n2 == 0)
            {
                await ctx.Channel.SendMessageAsync("Error: Please Enter 2 Numbers!");
                return;
            }

            int result = n1 - n2;
            await ctx.Channel.SendMessageAsync(result.ToString());
        }

        [Command("embed")]
        public async Task Embed(CommandContext ctx)
        {
            ulong requiredUserId = 692805583446868008;
            if (ctx.User.Id != requiredUserId)
            {
                await ctx.Channel.SendMessageAsync("You do not have permission to use this command!");
                return;
            }

            var message = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithTitle($"This command was executed by {ctx.User.Username}")
                .WithDescription($"This command was executed by {ctx.User.Mention}")
                .WithFooter($"This command was executed by {ctx.User.Username}")
                );
            await ctx.Channel.SendMessageAsync(message);
        }
        [Command("embed2")]
        public async Task Embed2(CommandContext ctx)
        {
            ulong requiredUserId = 692805583446868008;
            if (ctx.User.Id != requiredUserId)
            {
                await ctx.Channel.SendMessageAsync("You do not have permission to use this command!");
                return;
            }

            var footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = $"This command was executed by: {ctx.User.Username}"
            };

            var message = new DiscordEmbedBuilder
            {
                Title = $"This command was executed by {ctx.User.Username}",
                Description = $"This command was executed by {ctx.User.Mention}",
                Footer = footer,
                Color = DiscordColor.Black
            };

            await ctx.Channel.SendMessageAsync(embed: message);
        }

        [Command("cards")]
        public async Task Cards(CommandContext ctx)
        {

            var userCard = new CardsSystem();

            var footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = $"This command was executed by {ctx.User.Username}"
            };
            var userCardEmbed = new DiscordEmbedBuilder
            {
                Title = $"{ctx.User.Username} {userCard.selectedCard}",
                Color = DiscordColor.Aquamarine,
                Footer = footer
            };

            await ctx.Channel.SendMessageAsync(embed:  userCardEmbed);

            var botCard = new CardsSystem();
            var botCardEmbed = new DiscordEmbedBuilder
            {
                Title = $"Billy drew a {botCard.selectedCard}",
                Color = DiscordColor.Aquamarine,
                Footer = footer
            };

            await ctx.Channel.SendMessageAsync(embed: botCardEmbed);

            if (userCard.selectedNumber > botCard.selectedNumber)
            {
                await ctx.Channel.SendMessageAsync($"{ctx.User.Username} wins!");
                
            } else {
                await ctx.Channel.SendMessageAsync("Billy wins!");
            }

        }
        [Command("testint")] 

        public async Task Interact(CommandContext ctx)
        {
            ulong requiredUserId = 692805583446868008;
            if (ctx.User.Id != requiredUserId)
            {
                await ctx.Channel.SendMessageAsync("You do not have permission to use this command!");
                return;
            }

            var interact = Program.Client.GetInteractivity();

            var messageToRetrieve = await interact.WaitForMessageAsync(message => message.Content == "Hello");

            if(messageToRetrieve.Result.Content == "Hello")
            {
                await ctx.Channel.SendMessageAsync($"{ctx.User.Username} said Hello");
            }
        }
        [Command("testint2")]
        
        public async Task Interact2(CommandContext ctx)
        {

            ulong requiredUserId = 692805583446868008;
            if (ctx.User.Id != requiredUserId)
            {
                await ctx.Channel.SendMessageAsync("You do not have permission to use this command!");
                return;
            }

            var interact = Program.Client.GetInteractivity();

            var messageToReact = await interact.WaitForReactionAsync(message => message.Message.Id == 1207121206000750642);

            if(messageToReact.Result.Message.Id == 1207121206000750642)
            {
                await ctx.Channel.SendMessageAsync($"{ctx.User.Username} used the emoji {messageToReact.Result.Emoji.Name}");
            }
        }

        [Command("poll")]
        public async Task Poll(CommandContext ctx, string opt1, string opt2, string opt3, string opt4, [RemainingText] string pollTitle)
        {
            ulong requiredUserId = 692805583446868008;
            if (ctx.User.Id != requiredUserId)
            {
                await ctx.Channel.SendMessageAsync("You do not have permission to use this command!");
                return;
            }

            var interact = Program.Client.GetInteractivity();
            var pollTime = TimeSpan.FromSeconds(60);

            DiscordEmoji[] emojiOptions = { DiscordEmoji.FromName(Program.Client, ":one:"),
                                            DiscordEmoji.FromName(Program.Client, ":two:"),
                                            DiscordEmoji.FromName(Program.Client, ":three:"),
                                            DiscordEmoji.FromName(Program.Client, ":four:") };

            string optionsDescription = $"{emojiOptions[0]} | {opt1} \n" +
                                        $"{emojiOptions[1]} | {opt2} \n" +
                                        $"{emojiOptions[2]} | {opt3} \n" +
                                        $"{emojiOptions[3]} | {opt4} \n";

            var pollMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Title = pollTitle,
                Description = optionsDescription
            };

            var sentPoll = await ctx.Channel.SendMessageAsync(embed: pollMessage);
            foreach (var emoji in emojiOptions)
            {
                await sentPoll.CreateReactionAsync(emoji);
            }

            var totalReactions = await interact.CollectReactionsAsync(sentPoll, pollTime);

            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;

            foreach (var emoji in totalReactions)
            {
                if (emoji.Emoji == emojiOptions[0])
                {
                    count1++;
                }
                if (emoji.Emoji == emojiOptions[1])
                {
                    count2++;
                }
                if (emoji.Emoji == emojiOptions[2])
                {
                    count3++;
                }
                if (emoji.Emoji == emojiOptions[3])
                {
                    count4++;
                }
            }

            int totalVotes = count1 +
                             count2 +
                             count3 +
                             count4;
            string resultsDescription = $"{emojiOptions[0]}: {count1} Votes \n" +
                                        $"{emojiOptions[1]}: {count2} Votes \n" +
                                        $"{emojiOptions[2]}: {count3} Votes \n" +
                                        $"{emojiOptions[3]}: {count4} Votes \n\n" +
                                        $"Total Votes: {totalVotes}";
            var footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = $"This command was executed by {ctx.User.Username}"
            };

            var resultsEmbed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Title = "Results of Poll",
                Description = resultsDescription,
                Footer = footer
            };

            await ctx.Channel.SendMessageAsync(embed: resultsEmbed);
        }
        [Command("cd")]
        [Cooldown(5, 10, CooldownBucketType.User)]
        public async Task EHandler(CommandContext ctx)
        {
            ulong requiredUserId = 692805583446868008;
            if (ctx.User.Id != requiredUserId)
            {
                await ctx.Channel.SendMessageAsync("You do not have permission to use this command!");
                return;
            }

            await ctx.Channel.SendMessageAsync("DEEZ NUTZ");
        }
        [Command("cc")]
        [Description("Counts the number of times specific keywords were mentioned in the current channel.")]
        public async Task CC(CommandContext ctx)
        {
            ulong requiredUserId = 692805583446868008;
            if (ctx.User.Id != requiredUserId)
            {
                await ctx.Channel.SendMessageAsync("You do not have permission to use this command!");
                return;
            }
            // Delete the user's command message
            await ctx.Message.DeleteAsync();

            // Fetch messages from the current channel
            var messages = await ctx.Channel.GetMessagesAsync();

            // Define keywords to count
            string[] keywords = { "common", "rare", "epic", "legendary", "celestial" };

            // Count occurrences of each keyword in the messages
            Dictionary<string, int> keywordCounts = new Dictionary<string, int>();
            foreach (var message in messages)
            {
                // Use regular expression to extract words from the message
                var words = Regex.Matches(message.Content.ToLower(), @"\b\w+\b");
                foreach (Match wordMatch in words)
                {
                    var word = wordMatch.Value;

                    // Check if the word is one of the keywords
                    if (keywords.Contains(word))
                    {
                        if (keywordCounts.ContainsKey(word))
                        {
                            keywordCounts[word]++;
                        }
                        else
                        {
                            keywordCounts[word] = 1;
                        }
                    }
                }
            }

            // Construct the description for the keyword counts embed
            string keywordDescription = "";
            foreach (var keyword in keywords)
            {
                keywordDescription += $"{keyword}: {(keywordCounts.ContainsKey(keyword) ? keywordCounts[keyword].ToString() : "0")}\n";
            }

            // Construct an embed to display keyword counts
            var keywordEmbed = new DiscordEmbedBuilder
            {
                Title = "Keyword Counts",
                Color = DiscordColor.Green,
                Description = keywordDescription.Trim() // Trim to remove trailing newline
            };

            keywordEmbed.Footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = $"Information extracted from #{ctx.Channel.Name}"
            };

            // Calculate percentages for each rarity
            int totalCards = keywordCounts.Values.Sum();
            var cardPercentages = keywordCounts.ToDictionary(kv => kv.Key, kv => kv.Value / (double)totalCards * 100);

            // Sort the dictionary by rarity order
            var sortedCardPercentages = new Dictionary<string, double>();
            foreach (var keyword in keywords)
            {
                if (cardPercentages.ContainsKey(keyword))
                {
                    sortedCardPercentages[keyword] = cardPercentages[keyword];
                }
            }

            // Construct the description for the percentages embed
            string percentageDescription = "";
            foreach (var kvp in sortedCardPercentages)
            {
                percentageDescription += $"{kvp.Key}: {kvp.Value}%\n";
            }

            // Construct an embed to display card percentages
            var percentageEmbed = new DiscordEmbedBuilder
            {
                Title = "Card Percentages",
                Color = DiscordColor.Blue,
                Description = percentageDescription.Trim() // Trim to remove trailing newline
            };

            percentageEmbed.Footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = $"Information extracted from #{ctx.Channel.Name}"
            };

            // Get the specific channel to send the embeds
            ulong targetChannelId = 1209684805529378846;
            var targetChannel = ctx.Guild.GetChannel(targetChannelId);

            // Send the embeds to the specific channel
            if (targetChannel != null && targetChannel.Type == ChannelType.Text)
            {
                await targetChannel.SendMessageAsync(embed: keywordEmbed);
                await targetChannel.SendMessageAsync(embed: percentageEmbed);
            }
            else
            {
                await ctx.RespondAsync("Failed to find or send to the target channel.");
            }
        }
        [Command("testbuttons")]
        public async Task Button(CommandContext ctx)
        {
            ulong requiredUserId = 692805583446868008;
            if (ctx.User.Id != requiredUserId)
            {
                await ctx.Channel.SendMessageAsync("You do not have permission to use this command!");
                return;
            }

            var button = new DiscordButtonComponent(ButtonStyle.Primary, "Button 1", "Button 1");
            var button2 = new DiscordButtonComponent(ButtonStyle.Primary, "Button 2", "Button 2");

            var message = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Black)
                .WithTitle("Buttonzzz"))
                .AddComponents(button, button2);

            await ctx.Channel.SendMessageAsync(message);
        }
        [Command("calc")]
        public async Task CalculateCommand(CommandContext ctx, double num1, string operation, double num2)
        {
            ulong requiredUserId = 692805583446868008;
            if (ctx.User.Id != requiredUserId)
            {
                await ctx.Channel.SendMessageAsync("You do not have permission to use this command!");
                return;
            }

            double result = 0;

            switch (operation)
            {
                case "+":
                    result = num1 + num2;
                    break;
                case "-":
                    result = num1 - num2;
                    break;
                case "*":
                    result = num1 * num2;
                    break;
                case "/":
                    if (num2 != 0)
                        result = num1 / num2;
                    else
                    {
                        await ctx.Channel.SendMessageAsync("Error: Division by zero.");
                        return;
                    }
                    break;
                default:
                    await ctx.Channel.SendMessageAsync("Error: Invalid operation.");
                    return;
            }

            await ctx.Channel.SendMessageAsync($"Result: {result}");
        }
        [Command("help")]
        public async Task Help(CommandContext ctx)
        {
            ulong requiredUserId = 692805583446868008;
            if (ctx.User.Id != requiredUserId)
            {
                await ctx.Channel.SendMessageAsync("You do not have permission to use this command!");
                return;
            }

            var basicButton = new DiscordButtonComponent(ButtonStyle.Primary, "basicsButton", "Basics");
            var calcButton = new DiscordButtonComponent(ButtonStyle.Success, "calcButton", "Calculator");

            var message = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithTitle("Commands")
                .WithColor(DiscordColor.MidnightBlue)
                .WithDescription("Press a button to view it's command contents."))
                .AddComponents(basicButton, calcButton);

            await ctx.Channel.SendMessageAsync(message);
        }
    }
}

