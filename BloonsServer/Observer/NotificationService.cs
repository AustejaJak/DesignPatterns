using BloonsProject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloonsServer.Observer
{
    public class NotificationService
    {
        private Dictionary<TowerEvent, List<ITowerEventListener>> _players;

        public NotificationService()
        {
            List<ITowerEventListener> listeners = new List<ITowerEventListener>();
            _players = new Dictionary<TowerEvent, List<ITowerEventListener>>();

        }

        public void Subscribe(TowerEvent towerEvent, ITowerEventListener listener)
        {
            if (!_players.ContainsKey(towerEvent))
            {
                _players[towerEvent] = new List<ITowerEventListener>();
            }

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
                foreach (var listener in _players[towerEvent]) 
                {
                    try
                    {
                        await listener.SendMessage(username);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error notifying listener: {ex.Message}");
                    }
                }
            }
        }
    }
}
