using SplashKitSDK;
using System.IO;
using System;

namespace BloonsProject
{
    public class BaseTowerDecorator : ITowerDecorator
    {
        protected Tower _tower;

        public BaseTowerDecorator(Tower tower)
        {
            _tower = tower;
        }

        public virtual Bitmap GetTowerBitmap()
        {
            return _tower.GetOriginalBitmap();
        }

        public virtual int GetFireRateLevel()
        {
            return _tower.ShotType.FirerateUpgradeCount;
        }

        public virtual int GetRangeLevel()
        {
            return _tower.ShotType.RangeUpgradeCount;
        }
    }
}