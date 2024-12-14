using BloonsProject;

namespace BloonLibrary.VisitorImplementation
{
    public class FirerateUpgradeVisitor : IUpgradeOptionVisitor
    {
        public void Visit(Tower tower)
        {
            if (tower.ShotType.FirerateUpgradeCount >= 3) return;
            tower.ShotType.ShotSpeed -= 10;
            tower.ShotType.FirerateUpgradeCount++;
            tower.SellPrice += 0.7 * tower.ShotType.FirerateUpgradeCost;
            tower.UpdateDecorator();
        }
    }
}