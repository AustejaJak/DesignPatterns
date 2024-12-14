using BloonLibrary;
using System;

namespace BloonsGame.States
{
    public class InGameState : IGameUIState
    {
        private MainWindow _window;
        private GameClient _gameClient;

        public InGameState(MainWindow window, GameClient gameClient)
        {
            _window = window;
            _gameClient = gameClient;
            Console.WriteLine("InGame state");

        }

        public void HandleAllPlayersReady()
        {
            // Once in game, this doesn't matter; do nothing.
        }
    }
}
