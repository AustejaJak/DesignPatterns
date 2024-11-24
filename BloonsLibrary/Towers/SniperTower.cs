using System;
using System.IO;
using SplashKitSDK;

namespace BloonsProject
{
    public class SniperTower : Tower
    {
        
        public SniperTower(string username) : base(
            "Sniper Monkey", 
            username, 
            150,
            "Has a powerful shot", 
            FlyweightFactory.GetBitmap("SniperTower", TowerResources.SniperTower.MainSprite),
            new SniperShotBuilder()
                .SetBitmap("Sniper", TowerResources.SniperTower.ProjectileSprite)
                .Build(), 
            300)
        { }

        public static string Name => "Sniper Monkey";

        private static Bitmap _portraitBitmap;
        public static Bitmap Portrait
        {
            get
            {
                if (_portraitBitmap == null)
                {
                    _portraitBitmap = FlyweightFactory.GetBitmap(
                        "Sniper Portrait",
                        TowerResources.SniperTower.PortraitSprite
                    );
                }
                return _portraitBitmap;
            }
        }
    }
}