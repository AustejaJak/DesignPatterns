using BloonsProject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloonsServer.Observer
{
    public class NotificationService
    {
        private static readonly NotificationService _instance = new NotificationService();
        private Dictionary<TowerEvent, List<ITowerEventListener>> _players;

        public NotificationService()
        {
            List<ITowerEventListener> listeners = new List<ITowerEventListener>();
            _players = new Dictionary<TowerEvent, List<ITowerEventListener>>();

        }

        public static NotificationService GetInstance()
        {
            return _instance;
        }

        public void Subscribe(TowerEvent towerEvent, ITowerEventListener listener)
        {
            // Check if the event exists in dictionary, if not create a new list
            if (!_players.ContainsKey(towerEvent))
            {
                _players[towerEvent] = new List<ITowerEventListener>();
            }

            // Add the listener if it's not already in the list
            if (!_players[towerEvent].Contains(listener))
            {
                _players[towerEvent].Add(listener);
            }
        }

        public void Unsubscribe(TowerEvent towerEvent, string clientId)
        {
            if (_players.ContainsKey(towerEvent))
            {
                _players[towerEvent].RemoveAll(x => x.GetListenerId() == clientId);

                // Optional: remove the event key if no listeners remain
                if (_players[towerEvent].Count == 0)
                {
                    _players.Remove(towerEvent);
                }
            }
        }

        public async Task Notify(TowerEvent towerEvent, string username)
        {
            if (_players.ContainsKey(towerEvent))
            {
                foreach (var listener in _players[towerEvent]) // Create a copy of the list to avoid modification during iteration
                {
                    try
                    {
                        await listener.SendMessage(username);
                    }
                    catch (Exception ex)
                    {
                        // Handle or log any errors that occur during notification
                        Console.WriteLine($"Error notifying listener: {ex.Message}");
                    }
                }
            }
        }
    }
}
