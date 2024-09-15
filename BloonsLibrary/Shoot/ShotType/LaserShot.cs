using SplashKitSDK;

namespace BloonsProject
{
    public class LaserShot : IShotType
    {
        public LaserShot()
        {
            ShotSpeed = 50;
            Damage = 1;
            FirerateUpgradeCount = 0;
            RangeUpgradeCount = 0;
            FirerateUpgradeCost = 100;
            RangeUpgradeCost = 150;
            TimeSinceLastShot = 0;
            ProjectileBitmap = new Bitmap("Laser", "../../BloonsLibrary/Resources/Blast.png");
            ProjectileSpeed = 0.3;
            ProjectileSize = 2;
            ProjectileLength = 40;
            ProjectileWidth = 40;
            ProjectileStationaryTime = 0;
        }

        public Bitmap ProjectileBitmap { get; }
        public double ProjectileSpeed { get; set; }
        public double ShotSpeed { get; set; }
        public int Damage { get; }
        public double ProjectileStationaryTime { get; }
        public double ProjectileLength { get; }
        public double ProjectileWidth { get; }
        public int FirerateUpgradeCost { get; }
        public int RangeUpgradeCost { get; }
        public int FirerateUpgradeCount { get; set; }
        public int RangeUpgradeCount { get; set; }
        public int TimeSinceLastShot { get; set; }

        public double ProjectileSize { get; }
    }
}