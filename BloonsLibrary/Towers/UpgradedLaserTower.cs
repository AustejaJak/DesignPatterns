using System;
using System.IO;
using SplashKitSDK;

namespace BloonsProject
{
    public class UpgradedLaserTower : Tower
    {
        public UpgradedLaserTower(string username) : base(
            "Laser Monkey", 
            username,
            400, 
            "Did someone say rapidfire?", 
            FlyweightFactory.GetBitmap("LaserTower", TowerResources.LaserTower.MainSprite),
            new LaserShotBuilder()
                .SetBitmap("Laser", TowerResources.LaserTower.ProjectileSprite)
                .Build(), 
            350)
        { }
    }
}