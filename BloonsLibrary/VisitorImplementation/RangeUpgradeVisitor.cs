using BloonsProject;

namespace BloonLibrary.VisitorImplementation
{
    public class RangeUpgradeVisitor : IUpgradeOptionVisitor
    {
        public void Visit(Tower tower)
        {
            if (tower.ShotType.RangeUpgradeCount >= 3) return;
            tower.Range += 50;
            tower.ShotType.RangeUpgradeCount++;
            tower.SellPrice += 0.7 * tower.ShotType.RangeUpgradeCost;
            tower.UpdateDecorator();
        }
    }
}