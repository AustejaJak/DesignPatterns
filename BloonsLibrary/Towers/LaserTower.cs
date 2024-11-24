using System;
using System.IO;
using SplashKitSDK;

namespace BloonsProject
{
    public class LaserTower : Tower
    {
        public LaserTower(string username) : base(
            "Laser Monkey", 
            username, 
            400, 
            "Did someone say rapidfire?", 
            FlyweightFactory.GetBitmap("LaserTower", TowerResources.LaserTower.MainSprite),
            new LaserShotBuilder()
                .SetBitmap("Laser", TowerResources.LaserTower.ProjectileSprite)
                .Build(), 
            300)
        { }

        public static string Name => "Laser Monkey";

        private static Bitmap _portraitBitmap;

        public static Bitmap Portrait
        {
            get
            {
                if (_portraitBitmap == null)
                {
                    _portraitBitmap = FlyweightFactory.GetBitmap(
                        "Laser Portrait",
                        TowerResources.LaserTower.PortraitSprite
                    );
                }
                return _portraitBitmap;
            }
        }
    }
}