using System.IO;

namespace BloonsProject{
    public static class TowerResources
    {
        private static readonly string BaseResourcePath = @"..\..\..\..\BloonsLibrary\Resources";
        
        public static class DartTower
        {
            public static readonly string MainSprite = Path.Combine(BaseResourcePath, "Dart.png");
            public static readonly string ProjectileSprite = Path.Combine(BaseResourcePath, "Spike.png");
            public static readonly string PortraitSprite = Path.Combine(BaseResourcePath, "DartSelect.png");
        }
        
        public static class LaserTower
        {
            public static readonly string MainSprite = Path.Combine(BaseResourcePath, "Laser.png");
            public static readonly string ProjectileSprite = Path.Combine(BaseResourcePath, "Blast.png");
            public static readonly string PortraitSprite = Path.Combine(BaseResourcePath, "LaserSelect.png");
        }
        
        public static class SniperTower
        {
            public static readonly string MainSprite = Path.Combine(BaseResourcePath, "Sniper.png");
            public static readonly string ProjectileSprite = Path.Combine(BaseResourcePath, "Blade.png");
            public static readonly string PortraitSprite = Path.Combine(BaseResourcePath, "SniperSelect.png");
        }
        
        public static readonly string SellIcon = Path.Combine(BaseResourcePath, "sellTower.png");
        public static readonly string UpgradeFirerate = Path.Combine(BaseResourcePath, "firerateUpgrade.png"); 
        public static readonly string UpgradeRange = Path.Combine(BaseResourcePath, "rangeUpgrade.png");
    }
}