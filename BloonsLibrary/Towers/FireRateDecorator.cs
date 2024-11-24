using SplashKitSDK;
using System.IO;
using System;

namespace BloonsProject
{
    public class FireRateDecorator : BaseTowerDecorator
    {
        
        public FireRateDecorator(Tower tower) : base(tower) { }

        public override Bitmap GetTowerBitmap()
        {
            try
            {
                string towerType = GetTowerType();
                int level = GetFireRateLevel();

                if (level == 0)
                    return _tower.GetOriginalBitmap();

                string bitmapName = $"{towerType}_L{level}";
                string relativePath = $@"..\..\..\..\BloonsLibrary\Resources\{bitmapName}.png";

                // The factory will handle caching and reuse
                return FlyweightFactory.GetBitmap(bitmapName, relativePath);
            }
            catch
            {
                return _tower.GetOriginalBitmap();
            }
        }

        private string GetTowerType()
        {
            if (_tower is DartTower) return "Dart";
            if (_tower is LaserTower) return "Laser";
            if (_tower is SniperTower) return "Sniper";
            return "Unknown";
        }
    }
}