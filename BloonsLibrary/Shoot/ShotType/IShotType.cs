using SplashKitSDK;

namespace BloonsProject
{
    public interface IShotType
    {
        public double ShotSpeed { get; set; } // How often a projectile is shot.
        public int Damage { get; } // The damage dealt by the projectile to the bloons health.
        public double ProjectileStationaryTime { get; } // How long a projectile stays on the map once it reaches it's endpoint.
        public double ProjectileLength { get; } // The length of the bitmap
        public double ProjectileWidth { get; } // The width of the bitmap
        public int FirerateUpgradeCost { get; } // The cost of upgrading the projectiles firerate.
        public int RangeUpgradeCost { get; } // The cost of upgrading the projectiles range.
        public int FirerateUpgradeCount { get; set; } // How many times the projectile's firerate has been upgraded.
        public int RangeUpgradeCount { get; set; } // How many times the projectile's range has been upgraded.
        public int TimeSinceLastShot { get; set; } // How much time has passed since the projectile has been fired
        public Bitmap ProjectileBitmap { get; } // The projectile's bitmap.
        public double ProjectileSpeed { get; set; } // The speed at which the projectile travels.
        public double ProjectileSize { get; } // The size of the projectile's bitmap.
    }
}