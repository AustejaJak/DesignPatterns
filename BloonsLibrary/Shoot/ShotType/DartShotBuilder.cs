using SplashKitSDK;

namespace BloonsProject
{
     public class DartShotBuilder : IShotBuilder
    {
        private double _shotSpeed;
        private int _damage;
        private double _projectileStationaryTime;
        private double _projectileLength;
        private double _projectileWidth;
        private int _firerateUpgradeCost;
        private int _rangeUpgradeCost;
        private double _projectileSpeed;
        private double _projectileSize;
        private Bitmap _projectileBitmap;

        public DartShotBuilder()
        {
            // Set default values
            _shotSpeed = 100;
            _damage = 1;
            _projectileStationaryTime = 10;
            _projectileLength = 48;
            _projectileWidth = 48;
            _firerateUpgradeCost = 75;
            _rangeUpgradeCost = 75;
            _projectileSpeed = 0.3;
            _projectileSize = 1.5;
        }

        public IShotBuilder SetShotSpeed(double speed)
        {
            _shotSpeed = speed;
            return this;
        }

        public IShotBuilder SetDamage(int damage)
        {
            _damage = damage;
            return this;
        }

        public IShotBuilder SetProjectileStationaryTime(double time)
        {
            _projectileStationaryTime = time;
            return this;
        }

        public IShotBuilder SetProjectileDimensions(double length, double width)
        {
            _projectileLength = length;
            _projectileWidth = width;
            return this;
        }

        public IShotBuilder SetUpgradeCosts(int firerateCost, int rangeCost)
        {
            _firerateUpgradeCost = firerateCost;
            _rangeUpgradeCost = rangeCost;
            return this;
        }

        public IShotBuilder SetProjectileProperties(double speed, double size)
        {
            _projectileSpeed = speed;
            _projectileSize = size;
            return this;
        }

        public IShotBuilder SetBitmap(string name, string path)
        {
            _projectileBitmap = new Bitmap(name, path);
            return this;
        }

        public IShotType Build()
        {
            return new DartShot(
                _shotSpeed,
                _damage,
                _projectileStationaryTime,
                _projectileLength,
                _projectileWidth,
                _firerateUpgradeCost,
                _rangeUpgradeCost,
                _projectileSpeed,
                _projectileSize,
                _projectileBitmap
            );
        }
    }
}