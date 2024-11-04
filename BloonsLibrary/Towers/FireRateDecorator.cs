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
            Console.WriteLine("getting bitmap");
            try
            {
                string towerType = GetTowerType();
                int level = GetFireRateLevel();
                
                if (level == 0)
                    return _tower.GetOriginalBitmap();

                string bitmapPath = Path.Combine(baseDirectory, $@"..\..\..\..\BloonsLibrary\Resources\{towerType}_L{level}.png");
                
                if (!File.Exists(bitmapPath))
                    return _tower.GetOriginalBitmap();

                // Create bitmap with a unique name to prevent conflicts
                string bitmapName = $"{towerType}Tower_L{level}_{_tower.GetHashCode()}";
                return new Bitmap(bitmapName, bitmapPath);
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