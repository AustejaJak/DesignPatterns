using System;
using System.IO;
using SplashKitSDK;

namespace BloonsProject
{
    public class SniperTower : Tower
    {
        private static string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        
        public SniperTower(string username) : base(
            "Sniper Monkey", 
            username, 
            150,
            "Has a powerful shot", 
            new Bitmap("SniperTower", Path.Combine(baseDirectory, @"..\..\..\..\BloonsLibrary\Resources\Sniper.png")), 
            new SniperShot(), 
            300)
        { }

        public static string Name => "Sniper Monkey";

        public static Bitmap Portrait => new Bitmap("Sniper Portrait",
            Path.Combine(baseDirectory, @"..\..\..\..\BloonsLibrary\Resources\SniperSelect.png"));
    }
}