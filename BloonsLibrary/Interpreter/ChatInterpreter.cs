using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloonLibrary
{
    public interface IExpression
    {
        Task<bool> Interpret(Context context);
    }

    public class Context
    {
        public GameClient GameClient { get; set; }
        public string Username { get; set; }
        public string RawInput { get; set; }
        public static List<ICommand> CommandHistory { get; } = new List<ICommand>();

        public Context(GameClient gameClient, string username, string input)
        {
            GameClient = gameClient;
            Username = username;
            RawInput = input;
        }
    }

    public class UndoExpression : IExpression
    {
        public async Task<bool> Interpret(Context context)
        {
            if (Context.CommandHistory.Count > 0)
            {
                var lastCommand = Context.CommandHistory.Last();
                Context.CommandHistory.RemoveAt(Context.CommandHistory.Count - 1);
                lastCommand.Undo();
                await context.GameClient.SendInfoMessageAsync("Last message has been removed.");
            }
            else
            {
                await context.GameClient.SendInfoMessageAsync("No messages to undo.");
            }
            return true; // Command handled
        }
    }

    public class HelpExpression : IExpression
    {
        public async Task<bool> Interpret(Context context)
        {
            var helpMessage =
                "Available commands:\n" +
                "/pm <username> <message> - Send a private message\n" +
                "/ready - Set your status to ready\n" +
                "/unready - Set your status to not ready\n" +
                "/undo - Remove your last message\n" +
                "/help - Show this help message";

            await context.GameClient.SendInfoMessageAsync(helpMessage);
            return true; // Command handled
        }
    }

    public class PrivateMessageExpression : IExpression
    {
        public async Task<bool> Interpret(Context context)
        {
            var parts = context.RawInput.Split(' ');
            if (parts.Length < 3)
            {
                await context.GameClient.SendInfoMessageAsync("Usage: /pm <username> <message>");
                return true;
            }

            var targetUser = parts[1];
            var message = string.Join(" ", parts.Skip(2));
            await context.GameClient.SendPrivateMessageAsync(targetUser, message);
            return true; // Command handled
        }
    }

    public class ReadyExpression : IExpression
    {
        public async Task<bool> Interpret(Context context)
        {
            await context.GameClient.SetPlayerReadyAsync(true);
            return true; // Command handled
        }
    }

    public class UnreadyExpression : IExpression
    {
        public async Task<bool> Interpret(Context context)
        {
            await context.GameClient.SetPlayerReadyAsync(false);
            return true; // Command handled
        }
    }

    public class CommandParser
    {
        private readonly Dictionary<string, IExpression> _commands;

        public CommandParser()
        {
            _commands = new Dictionary<string, IExpression>
            {
                {"/undo", new UndoExpression()},
                {"/help", new HelpExpression()},
                {"/pm", new PrivateMessageExpression()},
                {"/ready", new ReadyExpression()},
                {"/unready", new UnreadyExpression()}
            };
        }

        public async Task ParseAndExecute(Context context)
        {
            var input = context.RawInput.Trim();

            // Handle commands
            if (input.StartsWith("/"))
            {
                var commandParts = input.Split(' ');
                var commandName = commandParts[0].ToLower();

                if (_commands.TryGetValue(commandName, out var expression))
                {
                    var handled = await expression.Interpret(context);
                    if (handled)
                    {
                        return; // Exit here if command was handled
                    }
                }
                else
                {
                    await context.GameClient.SendInfoMessageAsync("Unknown command. Type /help for available commands.");
                    return;
                }
            }

            // Only handle regular messages if no command was processed
            var sendMessageCommand = new SendMessageCommand(context.GameClient, input);
            Context.CommandHistory.Add(sendMessageCommand);
            sendMessageCommand.Execute();
        }
    }
}
