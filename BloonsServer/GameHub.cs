using Microsoft.AspNetCore.SignalR;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using BloonsProject;

public class GameHub : Hub
{
    private static List<string> _connectedUsernames = new List<string>();
    private static GameState _gameState = GameState.GetGameStateInstance();
    private string _username;
    private readonly MySqlConnection _dbConnection;

    public GameHub(MySqlConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task SendUsername(string username, string password)
    {
        // Fetch user details from the database
        BloonsProject.User user = GetUserFromDatabase(username, password);
        if (user != null)
        {
            _connectedUsernames.Add(username);
            _username = username;

            _gameState.AddUser(username, user);  // Correct usage of GameState.AddUser

            // Notify all clients of the new user joining and their initial money
            await Clients.All.SendAsync("SendUsername", username, user.Money); 
        }
        else
        {
            // Notify the client of a failed login attempt
            await Clients.Caller.SendAsync("LoginFailed", "Invalid username or password");
        }
    }

    // Method to retrieve user from the database based on username and password
    private BloonsProject.User GetUserFromDatabase(string username, string password)
{
    try
    {
        _dbConnection.Open();

        string query = "SELECT * FROM users WHERE Username = @Username AND Password = @Password";
        MySqlCommand cmd = new MySqlCommand(query, _dbConnection);
        cmd.Parameters.AddWithValue("@Username", username);
        cmd.Parameters.AddWithValue("@Password", password);

        using (var reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                // Return a BloonsProject.User object
                return new BloonsProject.User
                {
                    UserID = Convert.ToInt32(reader["UserID"]),
                    Username = reader["Username"].ToString(),
                    Password = reader["Password"].ToString(),
                    Round = Convert.ToInt32(reader["Round"]),
                    Money = Convert.ToDouble(reader["Money"]),
                    Lives = Convert.ToInt32(reader["Lives"])
                };
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error fetching user: {ex.Message}");
    }
    finally
    {
        _dbConnection.Close();
    }

    return null;
}

    // Send tower location and deduct money for the tower
    /**
    public async Task SendTowerLocation(string location)
    {
        var user = _gameState.GetUser(_username);
        if (user != null)
        {
            // Deduct money for the tower (assuming tower costs 100, for example)
            double towerCost = 100;
            if (user.Money >= towerCost)
            {
                user.Money -= towerCost;

                // Instantiate a concrete tower, e.g., CannonTower
                Tower newTower = new CannonTower(location); // Assuming CannonTower is a subclass of Tower
                user.Towers.Add(newTower);

                // Notify all clients that a tower has been placed by this user
                await Clients.All.SendAsync("SendTowerLocation", location, _username);
            }
            else
            {
                // Notify the client if they don't have enough money to place the tower
                await Clients.Caller.SendAsync("NotEnoughMoney", "Not enough money to place the tower.");
            }
        }
    }
    **/

    // Update the user's money (formerly Balance)
    public async Task UpdateBalance(string username, int newMoney)
    {
        var user = _gameState.GetUser(username);
        if (user != null)
        {
            user.Money = newMoney; // Changed from 'Balance' to 'Money'
            await Clients.All.SendAsync("UpdatePlayerBalance", username, newMoney);
        }
    }

    // Handle user disconnection
    public override Task OnDisconnectedAsync(Exception exception)
    {
        _connectedUsernames.Remove(_username);
        return base.OnDisconnectedAsync(exception);
    }
}
