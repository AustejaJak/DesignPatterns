namespace BloonsProject
{
    public class UpgradedDartTower : Tower
    {
        public UpgradedDartTower(string username) : base(
            "Dart Monkey", 
            username, 
            120, 
            "The Regular Tower", 
            FlyweightFactory.GetBitmap("DartTower", TowerResources.DartTower.MainSprite),
            new DartShotBuilder()
                .SetBitmap("Dart", TowerResources.DartTower.ProjectileSprite)
                .Build(),
            150)
        { }
    }
}