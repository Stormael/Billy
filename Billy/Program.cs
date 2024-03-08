using Billy.Slash;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using System;
using System.Linq;
using System.Threading.Tasks;
using TokenType = DSharpPlus.TokenType;


namespace Billy
{
    internal class Program
    {
        public static DiscordClient Client { get; private set; }
        public static CommandsNextExtension Commands { get; private set; }
        static async Task Main(string[] args)
        {
            var jsonReader = new JSONreader();
            await jsonReader.ReadJSON();

            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = jsonReader.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };

            Client = new DiscordClient(discordConfig);

            Client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(2)
            });
            
            Client.Ready += Client_Ready;
            Client.VoiceStateUpdated += VoiceChannelHandler;
            Client.ComponentInteractionCreated += IntEventHandler;
            Client.ModalSubmitted += ModalSumbitted;

            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { jsonReader.prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false
            };

            Commands = Client.UseCommandsNext(commandsConfig);
            var slashCommandsConfiguration = Client.UseSlashCommands();

            Commands.CommandErrored += CommandEventHandler;

            Commands.RegisterCommands<commands.Commands>();
            Commands.RegisterCommands<commands.DiscordComponentCommands>();
            slashCommandsConfiguration.RegisterCommands<BasicSlashCommands>();


            await Client.ConnectAsync();
            await Task.Delay(-1);
        }
        private static async Task ModalSumbitted(DiscordClient sender, ModalSubmitEventArgs e)
        {
            ulong targetChannelId = 1213988373438537749;
            var targetGuild = await sender.GetGuildAsync(1204483820187951195); // Replace GUILD_ID with your guild's ID
            var targetChannel = targetGuild.GetChannel(targetChannelId);

            

            // Send the embeds to the specific channel
            if (targetChannel != null && targetChannel.Type == ChannelType.Text && e.Interaction.Type == InteractionType.ModalSubmit)
            {
                var values = e.Values;
                var embed = new DiscordEmbedBuilder
                {
                    Title = $"{e.Interaction.User.Username} Sumbitted a Suggestion",
                    Description = $"{values.Values.First().ToString()}",
                    Color = DiscordColor.Black,
                };
                var sentMessage = await targetChannel.SendMessageAsync(embed);

                // Add reactions to the message
                await sentMessage.CreateReactionAsync(DiscordEmoji.FromName(sender, ":arrow_up:"));
                await sentMessage.CreateReactionAsync(DiscordEmoji.FromName(sender, ":arrow_down:"));
                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Suggestion submitted successfully."));
            }
            else
            {
                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Failed to find or send to the target channel."));
            }
        }

        private static async Task IntEventHandler(DiscordClient sender, ComponentInteractionCreateEventArgs args)
        {
            //Dropdown Events
            if(args.Id == "dropdown" && args.Interaction.Data.ComponentType == ComponentType.StringSelect)
            {
                var options = args.Values;
                foreach(var option in options)
                {
                    switch(option)
                    {
                        case "option1":
                            await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent("This is Option 1"));
                            break;
                        case "option2":
                            await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent("This is Option 2"));
                            break;
                        case "option3":
                            await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent("This is Option 3"));
                            break;

                    }
                }
            } else if (args.Id == "channelDropdownList")
            {
                var options = args.Values;
                foreach (var channel in options)
                {
                    var selectedChannel = await Client.GetChannelAsync(ulong.Parse(channel));
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"<@{args.User.Id}> -> <#{selectedChannel.Id}>"));
                }
            } else if(args.Id == "mentionList")
            {
                var userId = ulong.Parse(args.Values[0]);
                var guild = args.Guild;
                var user = await guild.GetMemberAsync(userId);
                var embedBuilder = new DiscordEmbedBuilder()
               .WithTitle($"User Information - {user.Username}")
               .AddField("Date Joined the Server", user.JoinedAt.ToString("yyyy-MM-dd HH:mm:ss") ?? "Unknown")
               .AddField("Date Joined Discord", user.CreationTimestamp.ToString("yyyy-MM-dd HH:mm:ss"))
               .AddField("Roles", string.Join(", ", user.Roles.Select(r => r.Mention)))
               .WithColor(DiscordColor.Blue);

                var options = args.Values;
                foreach(var userr in options)
                {
                    var selectedUser = await Client.GetUserAsync(ulong.Parse(userr));
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().AddEmbed(embedBuilder));
                }
            }


            //Button Events
            switch (args.Interaction.Data.CustomId)
            {
                case "Button 1":
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"{args.User.Username} has pressed button 1"));
                    break;
                case "Button 2":
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"{args.User.Username} has pressed button 2"));
                    break;
                case "basicsButton":
                    var basicCommandEmbed = new DiscordEmbedBuilder
                    {
                        Color = DiscordColor.Black,
                        Title = "Basic Commands",
                        Description = ">testbuttons | Sends a embed with buttons\n" +
                                      ">cc | Sends embed of extracted information from targeted channel\n" +
                                      ">cd | Test Cooldown Event Handler\n" +
                                      ">poll | Makes a poll with 4 options, seperate options by spaces\n" +
                                      ">testint | Test command for interactivity between Bot & Person\n" +
                                      ">cards | HighLow game but with cards\n" +
                                      ">embed | Embed using DiscordMessageBuilder\n" +
                                      ">embed2 | Embed using DiscordEmbedBuilder\n"
                    };
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().AddEmbed(basicCommandEmbed));
                    break;
                case "calcButton":
                    var calcCommandEmbed = new DiscordEmbedBuilder
                    {
                        Color = DiscordColor.Black,
                        Title = "Calculator Commands",
                        Description = ">calc num + num\n" +
                                      ">calc num - num\n" +
                                      ">calc num / num\n" +
                                      ">calc num * num\n"
                    };
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().AddEmbed(calcCommandEmbed));
                    break;
            }
        }

        private static async Task CommandEventHandler(CommandsNextExtension sender, CommandErrorEventArgs e)
        {
            if (e.Exception is ChecksFailedException exception)
            {
                string timeLeft = string.Empty;
                
                foreach(var check in exception.FailedChecks)
                {
                    var coolDown = (CooldownAttribute)check;
                    timeLeft = coolDown.GetRemainingCooldown(e.Context).ToString(@"hh\:mm\:ss");
                }

                var coolDownMessage = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Red,
                    Title = "Cooldown",
                    Description = $"You have {timeLeft} left"
                };

                await e.Context.Channel.SendMessageAsync(coolDownMessage);
            }
        }

        private static async Task VoiceChannelHandler(DiscordClient sender, VoiceStateUpdateEventArgs e)
        {
            if(e.Before == null && e.Channel.Name == "Create") 
            {
                await e.Channel.SendMessageAsync($"{e.User.Username} joined the voice channel");
            }
        }

        private static Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
