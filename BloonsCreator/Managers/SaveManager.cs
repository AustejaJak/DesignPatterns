using BloonsProject;
using BloonsProject.Models.Extensions;
using H.Utilities;
using RandomNameGeneratorLibrary;
using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.Json;
using Bitmap = System.Drawing.Bitmap;
using Rectangle = System.Drawing.Rectangle;

namespace BloonsCreator
{
    public class SaveManager
    {
        private readonly CreatorState _creatorState = CreatorState.GetClickHandlerEvents();
        private string _mapName;

        public SaveManager(string mapName)
        {
            _mapName = mapName;
            _creatorState.buttonClickEvent += OnButtonClicked;
        }

        public void CreateMapFor(Bitmap screenshot, List<Point2D> tileManagerCheckpoints)
        {
            var serializablePoints = tileManagerCheckpoints.Select(t => SplashKitExtensions.VectorFromPoint(t)).ToList();
            var mapToSave = new Map($"../../BloonsLibrary/Resources/{_mapName}.jpeg", screenshot.Width, screenshot.Height, 25, serializablePoints, _mapName);

            using var createStream = File.Create($"../../BloonsLibrary/Maps/MapJsons/{_mapName}.json");
            JsonSerializer.SerializeAsync(createStream, mapToSave);
        }

        public Bitmap TakeScreenshotOf(Window window, string name)
        {
            var screenShot = Screenshoter.Shot(new Rectangle(window.X, window.Y, window.Width, window.Height - 150));
            screenShot.Save($"../../BloonsLibrary/Resources/{name}.jpeg", ImageFormat.Jpeg);
            return screenShot;
        }

        public void OnButtonClicked(Button button)
        {
            if (!(button is SaveButton)) return;
            if (_creatorState.Checkpoints.Count() <= 1) return;
            var screenshot = TakeScreenshotOf(_creatorState.Window, _mapName);
            CreateMapFor(screenshot, _creatorState.Checkpoints);
            Environment.Exit(0);
        }
    }
}