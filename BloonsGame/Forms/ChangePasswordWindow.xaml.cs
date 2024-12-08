using BloonLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BloonsGame.Forms
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private UserController _userController;
        private GameClient _gameclient;
        public ChangePasswordWindow(GameClient gameClient, UserController userController)
        {
            var dbcontect = new GameDbContext();
            _userController = userController;
            _gameclient = gameClient;
            InitializeComponent();
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var existingUser = _userController.CurrentUser;
            if (existingUser != null)
            {
                if (existingUser.Password == PasswordInput.Password && NewpasswordInput.Password == NewPasswordReInput_Copy.Password)
                {
                    _userController.ChangePassword(NewpasswordInput.Password);
                    _userController.AddPassword(NewpasswordInput.Password);
                    //_gameclient.passwordHistory.SavePassword(NewpasswordInput.Password);

                }
                else
                {
                    MessageBox.Show("Passwords do not match", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
            }
            else
            {
                MessageBox.Show("Username not valid", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Password Changed!", "Password changed Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
            var mainWindow = new MainWindow(_gameclient, _userController);
            mainWindow.Show();

            this.Close();
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow(_gameclient, _userController);
            mainWindow.Show();

            this.Close();
        }

        private void ChangeBackPassword_Click(object sender, RoutedEventArgs e)
        {
            if (_userController.RestorePreviuosPassword())
            {
                MessageBox.Show("Previous password restored!", "Password changed successfully", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No previous password found!", "No previous password found", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
            var mainWindow = new MainWindow(_gameclient, _userController);
            mainWindow.Show();

            this.Close();
        }
    }
}
