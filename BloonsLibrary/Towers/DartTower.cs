using System;
using System.IO;
using SplashKitSDK;

namespace BloonsProject
{
    public class DartTower : Tower
    {
        public DartTower(string username) : base(
            "Dart Monkey",
            username,
            120,
            "The Regular Tower",
            FlyweightFactory.GetBitmap("DartTower", TowerResources.DartTower.MainSprite),
            new DartShotBuilder()
                .SetBitmap("Dart", TowerResources.DartTower.ProjectileSprite)
                .Build(),
            100)
        { }

        public static string Name => "Dart Monkey";

        // Cache the portrait bitmap using the flyweight pattern
        private static Bitmap _portraitBitmap;
        public static Bitmap Portrait
        {
            get
            {
                if (_portraitBitmap == null)
                {
                    _portraitBitmap = FlyweightFactory.GetBitmap(
                        "Dart Portrait",
                        TowerResources.DartTower.PortraitSprite
                    );
                }
                return _portraitBitmap;
            }
        }
    }
}