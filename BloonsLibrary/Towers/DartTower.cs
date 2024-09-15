using System;
using System.IO;
using SplashKitSDK;

namespace BloonsProject
{
    public class DartTower : Tower
    {
        public DartTower() : base("Dart Monkey", 120, "The Regular Tower", new Bitmap("DartTower",
            "../BloonsLibrary/Resources/Dart.png"), new DartShot(), 100)
        {
            Console.WriteLine("Directory: " + Directory.GetCurrentDirectory());
        }

        public static string Name => "Dart Monkey";

        public static Bitmap Portrait => new Bitmap("Dart Portrait",
            "../BloonsLibrary/Resources/DartSelect.png");
    }
}