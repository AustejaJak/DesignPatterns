using System;
using System.IO;
using SplashKitSDK;

namespace BloonsProject
{
    public class UpgradedLaserTower : Tower
    {
        private static string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public UpgradedLaserTower(string username) : base("Laser Monkey", username, 400, "Did someone say rapidfire?", new Bitmap("LaserTower", Path.Combine(baseDirectory, @"..\..\..\..\BloonsLibrary\Resources\Laser.png")), new LaserShotBuilder().Build(), 350)
        { }

        public static string Name => "Upgraded Laser Monkey";

        public static Bitmap Portrait => new Bitmap("Laser Portrait",
            Path.Combine(baseDirectory, @"..\..\..\..\BloonsLibrary\Resources\LaserSelect.png"));
    }
}