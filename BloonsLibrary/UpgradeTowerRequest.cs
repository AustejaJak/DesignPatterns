using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonLibrary
{
    public record UpgradeOrSellTowerRequest(NetworkPoint2D Position, string option, int upgradeCount);

    public record UpgradeTowerRangeRequest(NetworkPoint2D Position, string option, int upgradeCount);

}
