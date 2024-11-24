using SplashKitSDK;

namespace BloonsProject
{
    public class SniperShotBuilder : IShotBuilder
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
        private string _bitmapName;
        private string _bitmapPath;
        
        public SniperShotBuilder()
        {
            // Set default values
            _shotSpeed = 200;
            _damage = 3;
            _projectileStationaryTime = 100;
            _projectileLength = 94;
            _projectileWidth = 94;
            _firerateUpgradeCost = 50;
            _rangeUpgradeCost = 50;
            _projectileSpeed = 0.1;
            _projectileSize = 2;
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
            _bitmapName = name;
            _bitmapPath = path;
            return this;
        }

        public IShotType Build()
        {
            // Get the bitmap from FlyweightFactory instead of creating a new instance
            Bitmap projectileBitmap = _bitmapName != null && _bitmapPath != null
                ? FlyweightFactory.GetBitmap(_bitmapName, _bitmapPath)
                : null;

            return new SniperShot(
                _shotSpeed,
                _damage,
                _projectileStationaryTime,
                _projectileLength,
                _projectileWidth,
                _firerateUpgradeCost,
                _rangeUpgradeCost,
                _projectileSpeed,
                _projectileSize,
                projectileBitmap
            );
        }
    }
}