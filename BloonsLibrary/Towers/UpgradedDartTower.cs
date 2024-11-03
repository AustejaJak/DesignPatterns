using System;
using System.IO;
using SplashKitSDK;

namespace BloonsProject
{
    public class UpgradedDartTower : Tower
    {
        private static string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public UpgradedDartTower(string username) : base("Dart Monkey", username, 120, "The Regular Tower", new Bitmap(
            "DartTower",
            Path.Combine(baseDirectory, @"..\..\..\..\BloonsLibrary\Resources\Dart.png")), new DartShotBuilder()  // Use the builder here
            .SetBitmap("Dart", Path.Combine(baseDirectory, @"..\..\..\..\BloonsLibrary\Resources\Spike.png"))
            .Build(), 150)
        { }

        public static string Name => "Upgraded Dart Monkey";

        public static Bitmap Portrait => new Bitmap("Dart Portrait",
            Path.Combine(baseDirectory, @"..\..\..\..\BloonsLibrary\Resources\DartSelect.png"));
    }
}