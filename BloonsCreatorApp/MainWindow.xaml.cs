using System.Windows;
using System;
using BloonLibrary.Extensions;
using BloonsCreator;
using BloonsProject;
using BloonsProject.Models.Extensions;
using SplashKitSDK;
using Window = System.Windows.Window;

namespace BloonsCreatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProgramLoop _gameLoop;
        private CreatorState _creatorState = CreatorState.GetClickHandlerEvents();

        public MainWindow()
        {
            InitializeComponent();
            foreach (var map in MapManager.GetAllMaps())
                MapComboBox.Items.Add(map.Name); // Adds the maps to the combobox on the WPF display from the map manager.
        }

        private void EnterMapCreatorButton_Click(object sender, RoutedEventArgs e)
        {
            NameInputErrorLabel.Visibility = Visibility.Hidden;
            if (NameInputBox.Text.Trim() == "")
            {
                NameInputErrorLabel.Visibility = Visibility.Visible;
                return;
            }
            _gameLoop = new ProgramLoop(NameInputBox.Text);
            Hide();
            _gameLoop.RunProgram();
        }

        private void EditMapButton_Click(object sender, RoutedEventArgs e)
        {
            MapInputErrorLabel.Visibility = Visibility.Hidden;
            if (MapComboBox.SelectionBoxItem == null)
            {
                MapInputErrorLabel.Visibility = Visibility.Visible;
                return;
            }
            _gameLoop = new ProgramLoop(MapComboBox.SelectionBoxItem.ToString());
            var selectedMap = MapManager.GetMapByName(MapComboBox.SelectionBoxItem.ToString());
            foreach (var checkpoint in selectedMap.Checkpoints)
            {
                var tile = _gameLoop.TileEditorTool.GetTileOnGrid(SplashKitExtensions.PointFromVector(checkpoint));
                _creatorState.Checkpoints.Add(SplashKitExtensions.PointFromVector(checkpoint));
                _gameLoop.TileEditorTool.RemoveTile(tile);
                _creatorState.Tiles.Add(tile);
            }
            Hide();
            _gameLoop.RunProgram();
        }
    }
}