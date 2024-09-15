using SplashKitSDK;

namespace BloonsProject
{
    public class Projectile
    {
        public Projectile(Point2D projectileLocation, Point2D projectileDestination, IShotType projectileShotType)
        {
            ProjectileLocation = projectileLocation;
            ProjectileDestination = projectileDestination;
            ProjectileShotType = projectileShotType;
            ProjectileStationaryTimeSpent = 0;
        }

        public Point2D ProjectileDestination { get; set; } // Projectile endpoint
        public Point2D ProjectileLocation { get; set; } // Projectile current point
        public IShotType ProjectileShotType { get; set; } // Shot type of projectile (eg dart shot, laser shot, sniper shot)
        public double ProjectileStationaryTimeSpent { get; set; } // When projectile endpoint is reached, remain at the endpoint and keep track of it with this property.
    }
}