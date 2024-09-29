using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonLibrary
{
    internal class GameClient
    {
        private HubConnection _connection;

        public async Task ConnectToServer(string url)
        {
            _connection = new HubConnectionBuilder()
                            .WithUrl(url)
                            .Build();

            _connection.On<string>("ReceiveMessage", (message) =>
            {
                Console.WriteLine($"Message from server: {message}");
            });

            _connection.On<string>("TowerPlaced", (towerInfo) =>
            {
                Console.WriteLine($"Tower placed: {towerInfo}");
            });

            try
            {
                await _connection.StartAsync();
                Console.WriteLine("Connected to SignalR server.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect to server: {ex.Message}");
            }
        }

        public async Task SendMessageToServer(string message)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("SendMessage", message);
            }
        }

        public async Task PlaceTower(string towerInfo)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("PlaceTower", towerInfo);
            }
        }

        public async Task Disconnect()
        {
            if (_connection != null)
            {
                await _connection.StopAsync();
                await _connection.DisposeAsync();
                Console.WriteLine("Disconnected from server.");
            }
        }
    }
}
