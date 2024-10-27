using System.Linq;
using System.Runtime.CompilerServices;
using SplashKitSDK;

namespace BloonsCreator
{
    public class ProgramLoop
    {
        public TileEditorTool TileEditorTool = new TileEditorTool();
        private CreatorState _creatorState = CreatorState.GetClickHandlerEvents();
        private Renderer _renderer = new Renderer(50, 50);
        private SaveButton _saveButton = new SaveButton();
        private SaveManager _saveManager;
        private Window _window = new Window("BloonsCreator", 800, 700);

        public ProgramLoop(string mapName)
        {
            _creatorState.Window = _window;
            _creatorState.Buttons.Add(_saveButton);
            _saveManager = new SaveManager(mapName);
            TileEditorTool.InitializeAllTilesAsGreen();
        }

        public void RunProgram()
        {
            do
            {
                SplashKit.ClearWindow(_creatorState.Window, Color.DarkGreen);
                _renderer.RenderButton(_saveButton.TemplateTileBitmap, _saveButton.SaveButtonRectangle);
                _renderer.RenderTemplateTiles(TileEditorTool.TemplateTiles);
                _renderer.RenderTiles(_creatorState.Tiles);
                _renderer.RenderGrid();
                var selectedButton = TileEditorTool.CurrentSelectedTileButton();
                _renderer.ShadeButton(SplashKit.RectangleFrom(selectedButton.Position, selectedButton.Height, selectedButton.Width));

                if (SplashKit.MouseClicked(MouseButton.LeftButton))
                {
                    _creatorState.UpdateOnButtonPress();
                    TileEditorTool.AddTileAt(SplashKit.MousePosition());
                }

                SplashKit.RefreshScreen(60);
                SplashKit.ProcessEvents();
            } while (!SplashKit.WindowCloseRequested("BloonsCreator"));
        }
    }
}