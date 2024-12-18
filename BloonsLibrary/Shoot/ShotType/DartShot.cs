﻿using System;
using System.IO;
using SplashKitSDK;

namespace BloonsProject
{
    public class DartShot : IShotType
    {
        public DartShot(
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
            ShotSpeed = 100;
            Damage = 1;
            FirerateUpgradeCount = 0;
            RangeUpgradeCount = 0;
            FirerateUpgradeCost = 75;
            RangeUpgradeCost = 75;
            TimeSinceLastShot = 0;
            ProjectileBitmap = projectileBitmap;
            ProjectileSpeed = 0.3;
            ProjectileSize = 1.5;
            ProjectileLength = 48;
            ProjectileWidth = 48;
            ProjectileStationaryTime = 10;
        }

        public int Damage { get; }
        public int FirerateUpgradeCost { get; }
        public int FirerateUpgradeCount { get; set; }
        public Bitmap ProjectileBitmap { get; }
        public double ProjectileLength { get; }
        public double ProjectileSize { get; }
        public double ProjectileSpeed { get; set; }
        public double ProjectileStationaryTime { get; }
        public double ProjectileWidth { get; }
        public int RangeUpgradeCost { get; }
        public int RangeUpgradeCount { get; set; }
        public double ShotSpeed { get; set; }
        public int TimeSinceLastShot { get; set; }
    }
}