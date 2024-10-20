using BloonLibrary;
using System;
using System.Windows;

namespace BloonsGame
{
    public partial class LoginRegisterWindow : Window
    {
        private UserController _userController;
        private GameClient _gameclient;

        public LoginRegisterWindow()
        {
            InitializeComponent();
            _gameclient = new GameClient();

            this.Loaded += LoginRegisterWindow_Loaded;
        }

         private async void LoginRegisterWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await _gameclient.ConnectToServer("http://localhost:5000/gamehub");

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
                        await _gameclient.SendUsernameAsync(existingUser.Username);
                        OpenMainWindow(_gameclient, existingUser.Username);
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
                    MessageBox.Show("User registered successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    await _gameclient.SendUsernameAsync(newUser.Username);
                    OpenMainWindow(_gameclient, newUser.Username);
                }
            }
        }
        private void OpenMainWindow(GameClient gameclient, string username)
        {
            var mainWindow = new MainWindow(gameclient, username);
            mainWindow.Show();

            this.Close();
        }
    }
}
