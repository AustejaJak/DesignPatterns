namespace BloonLibrary
{
    public class SendMessageCommand : ICommand
    {
        private readonly GameClient _gameClient;
        private readonly string _message;

        public SendMessageCommand(GameClient gameClient, string message)
        {
            _gameClient = gameClient;
            _message = message;
        }

        public async void Execute()
        {
            await _gameClient.SendChatMessageAsync(_message);
        }
    }
}