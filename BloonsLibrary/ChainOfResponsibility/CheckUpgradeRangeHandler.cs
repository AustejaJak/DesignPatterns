using BloonsProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloonLibrary.VisitorImplementation;

namespace BloonLibrary.ChainOfResponsibility
{
    public class UpgradeRangeHandler : TowerActionHandler
    {
        public override void Handle(string option, Tower tower, TowerGuiOptions towerOptions, GameState gameState, GameClient gameClient)
        {
            if (option == "Upgrade Range")
            {
                var visitor = new RangeUpgradeVisitor();
                tower.Accept(visitor);
                towerOptions.SelectedInGui = "none";
                gameState.Player.Money -= tower.ShotType.RangeUpgradeCost;
                Console.WriteLine("Upgrading range");
                _ = gameClient.UpgradeTowerRangeAsync(new UpgradeOrSellTowerRequest(NetworkPoint2D.Serialize(tower.Position), option, tower.ShotType.RangeUpgradeCount));
            }
            else
            {
                _nextHandler?.Handle(option, tower, towerOptions, gameState, gameClient);
            }
        }
    }

}
