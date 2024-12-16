using BloonLibrary;
using BloonsProject;

namespace BloonsGame.Mediator
{
    public interface IWindowNavigationMediator
    {
        void NavigateToMainWindow(GameClient _gameClient, UserController _userController);
        void NavigateToPauseWindow(IProgramController programController);
        void NavigateToLoseWindow(GameClient gameClient);
        void NavigateToChangePasswordWindow(GameClient gameClient, UserController userController, MainWindow mainWindow);
    }
}