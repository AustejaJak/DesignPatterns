using BloonLibrary;
using System;
using System.Windows;
using BloonsGame.Mediator;

namespace BloonsGame
{
    public partial class LoginRegisterWindow : Window
    {
        private UserController _userController;
        private GameClient _gameclient;
        private IWindowNavigationMediator _navigationMediator;

        public LoginRegisterWindow()
        {
            InitializeComponent();
            _gameclient = new GameClient();
            _navigationMediator = new WindowNavigationMediator();

            this.Loaded += LoginRegisterWindow_Loaded;
        }

         private async void LoginRegisterWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await _gameclient.ConnectToServer("http://192.168.0.104:5000/gamehub");

                MessageBox.Show("Connected to the server!", "Connection Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect to the server: {ex.Message}", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameInput.Text;
            string password = passwordInput.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username and Password cannot be empty.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var dbContext = new GameDbContext())
            {
                if (_userController == null)
                {
                    _userController = new UserController(dbContext);
                }

                User existingUser = _userController.GetUserByUsername(username);

                if (existingUser != null)
                {
                    if (existingUser.Password == password)
                    {
                        MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        _userController.AddPassword(password);
                        await _gameclient.SendUsernameAsync(existingUser.Username);
                        _gameclient.JoinGameAsync(existingUser.Username);
                        _navigationMediator.NavigateToMainWindow(_gameclient, _userController);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    User newUser = new User
                    {
                        Username = username,
                        Password = password,
                    };

                    _userController.CreateUser(newUser);
                    _userController.AddPassword(password);
                    MessageBox.Show("User registered successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    await _gameclient.SendUsernameAsync(newUser.Username);
                    _gameclient.JoinGameAsync(newUser.Username);
                    _navigationMediator.NavigateToMainWindow(_gameclient, _userController);
                    this.Close();
                }
            }
        }
    }
}