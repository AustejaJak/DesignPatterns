using System;
using System.IO;
using SplashKitSDK;

namespace BloonsProject
{
    public class UpgradedSniperTower : Tower
    {
        public UpgradedSniperTower(string username) : base(
            "Sniper Monkey", 
            username, 
            150, 
            "Has a powerful shot",
            FlyweightFactory.GetBitmap("SniperTower", TowerResources.SniperTower.MainSprite),
            new SniperShotBuilder()
                .SetBitmap("Sniper", TowerResources.SniperTower.ProjectileSprite)
                .Build(), 
            400)
        {
        }
    }
}