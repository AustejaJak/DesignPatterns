using System;
using System.IO;
using SplashKitSDK;

namespace BloonsProject
{
    public class SniperShot : IShotType
    {
        private readonly string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public SniperShot(
            double shotSpeed,
            int damage,
            double projectileStationaryTime,
            double projectileLength,
            double projectileWidth,
            int firerateUpgradeCost,
            int rangeUpgradeCost,
            double projectileSpeed,
            double projectileSize,
            Bitmap projectileBitmap
        )
        {
            ShotSpeed = 200;
            Damage = 3;
            FirerateUpgradeCount = 0;
            RangeUpgradeCount = 0;
            FirerateUpgradeCost = 50;
            RangeUpgradeCost = 50;
            TimeSinceLastShot = 0;
            ProjectileBitmap = projectileBitmap;
            ProjectileSpeed = 0.1;
            ProjectileSize = 2;
            ProjectileLength = 94;
            ProjectileWidth = 94;
            ProjectileStationaryTime = 100;
        }

        public Bitmap ProjectileBitmap { get; }
        public double ProjectileStationaryTime { get; }
        public double ProjectileLength { get; }
        public double ProjectileWidth { get; }
        public double ProjectileSize { get; }
        public double ProjectileSpeed { get; set; }

        public double ShotSpeed { get; set; }
        public int Damage { get; }
        public int FirerateUpgradeCost { get; }
        public int RangeUpgradeCost { get; }
        public int FirerateUpgradeCount { get; set; }
        public int RangeUpgradeCount { get; set; }
        public int TimeSinceLastShot { get; set; }
    }
}