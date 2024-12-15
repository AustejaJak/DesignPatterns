using BloonLibrary;
using BloonsGame.Forms;
using BloonsProject;

namespace BloonsGame.Mediator
{
    public class WindowNavigationMediator : IWindowNavigationMediator
    {
        public void NavigateToMainWindow(GameClient _gameClient, UserController _userController)
        {
            MainWindow mainWindow = new MainWindow(_gameClient, _userController);
            mainWindow.Show();
            //mainWindow.Close();
        }

        public void NavigateToPauseWindow(IProgramController programController)
        {
            var pauseWindow = new PauseWindow(programController);
            pauseWindow.Show();
            pauseWindow.Hide();
        }

        public void NavigateToLoseWindow(GameClient gameClient)
        {
            var loseWindow = new LoseWindow(gameClient);
            loseWindow.Show();
            //loseWindow.Hide();
        }

        public void NavigateToChangePasswordWindow(GameClient gameClient, UserController userController, MainWindow mainWindow)
        {
            var changePasswordWindow = new ChangePasswordWindow(gameClient, userController, mainWindow);
            changePasswordWindow.Show();
            mainWindow.Hide();
        }
    }
}