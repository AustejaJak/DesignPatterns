using BloonsProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloonLibrary.VisitorImplementation;

namespace BloonLibrary.ChainOfResponsibility
{
    public class UpgradeFirerateHandler : TowerActionHandler
    {
        public override void Handle(string option, Tower tower, TowerGuiOptions towerOptions, GameState gameState, GameClient gameClient)
        {
            if (option == "Upgrade Firerate")
            {
                var visitor = new FirerateUpgradeVisitor();
                tower.Accept(visitor);
                towerOptions.SelectedInGui = "none";
                gameState.Player.Money -= tower.ShotType.FirerateUpgradeCost;
                Console.WriteLine("Upgrading fire rate");
                _ = gameClient.UpgradeTowerFireRateAsync(new UpgradeOrSellTowerRequest(NetworkPoint2D.Serialize(tower.Position), option, tower.ShotType.FirerateUpgradeCount));

                //tower.Range += 50;
                //towerOptions.SelectedInGui = "none";
                //gameState.Player.Money -= tower.ShotType.RangeUpgradeCost;
                //tower.SellPrice += 0.7 * tower.ShotType.RangeUpgradeCost;
                //tower.ShotType.RangeUpgradeCount++;

                //_ = gameClient.UpgradeTowerRangeAsync(
                //    new UpgradeOrSellTowerRequest(NetworkPoint2D.Serialize(tower.Position), option, tower.ShotType.RangeUpgradeCount));
            }
            else
            {
                _nextHandler?.Handle(option, tower, towerOptions, gameState, gameClient);
            }
        }
    }

}
