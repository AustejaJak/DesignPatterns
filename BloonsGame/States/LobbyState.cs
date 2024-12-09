using BloonLibrary;
using System;

namespace BloonsGame.States
{
    public class LobbyState : IGameUIState
    {
        private MainWindow _window;
        private GameClient _gameClient;

        public LobbyState(MainWindow window, GameClient gameClient)
        {
            _window = window;
            _gameClient = gameClient;
            Console.WriteLine("Lobby state");

        }

        public void HandleAllPlayersReady()
        {
            // Transition to CountdownState using the method
            _window.ChangeState(new CountdownState(_window, _gameClient));
            _window.StartCountdown();
        }
    }
}
