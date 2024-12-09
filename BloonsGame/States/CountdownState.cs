using BloonLibrary;
using System;

namespace BloonsGame.States
{
    public class CountdownState : IGameUIState
    {
        private MainWindow _window;
        private GameClient _gameClient;

        public CountdownState(MainWindow window, GameClient gameClient)
        {
            _window = window;
            _gameClient = gameClient;
            Console.WriteLine("Countdown state");
        }

        public void HandleAllPlayersReady()
        {
            // Already all ready, countdown is already running.
            // No additional action needed.
        }
    }
}
