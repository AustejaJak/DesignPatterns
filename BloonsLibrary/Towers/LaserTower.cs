using System;
using System.IO;
using SplashKitSDK;

namespace BloonsProject
{
    public class LaserTower : Tower
    {
        private static string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public LaserTower(string username) : base(
            "Laser Monkey", 
            username, 
            400, 
            "Did someone say rapidfire?", 
            new Bitmap("LaserTower", Path.Combine(baseDirectory, @"..\..\..\..\BloonsLibrary\Resources\Laser.png")), 
            new LaserShotBuilder().Build(), 
            300)
        { }

        public static string Name => "Laser Monkey";

        public static Bitmap Portrait => new Bitmap("Laser Portrait",
            Path.Combine(baseDirectory, @"..\..\..\..\BloonsLibrary\Resources\LaserSelect.png"));
    }
}