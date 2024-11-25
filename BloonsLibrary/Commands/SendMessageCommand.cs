using System;
using System.Threading.Tasks;

namespace BloonLibrary
{
    public class SendMessageCommand : ICommand
    {
        private readonly GameClient _gameClient;
        private readonly string _message;
        private readonly string _messageId;

        public SendMessageCommand(GameClient gameClient, string message)
        {
            _gameClient = gameClient;
            _message = message;
            _messageId = Guid.NewGuid().ToString();
        }

        public void Execute()
        {
            _ = ExecuteAsync();
        }

        public void Undo()
        {
            _ = UndoAsync();
        }

        private async Task ExecuteAsync()
        {
            await _gameClient.SendChatMessageAsync(_message, _messageId);
        }

        private async Task UndoAsync()
        {
            await _gameClient.DeleteMessageAsync(_messageId);
        }
    }
}
