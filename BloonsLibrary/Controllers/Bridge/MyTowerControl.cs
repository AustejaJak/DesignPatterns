using BloonLibrary.ChainOfResponsibility;
using BloonsProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonLibrary.Controllers.Bridge
{
    public class MyTowerControl : TowerContols
    {
        TowerActionHandler checkMoneyHandler = new CheckMoneyHandler();
        TowerActionHandler checkMaxUpgradeHandler = new CheckMaxUpgradeHandler();
        TowerActionHandler upgradeRangeHandler = new UpgradeRangeHandler();
        TowerActionHandler upgradeFirerateHandler = new UpgradeFirerateHandler();
        TowerActionHandler sellTowerHandler = new SellTowerHandler();

        public MyTowerControl(Tower t, GameClient client) : base(t, client)
        {
            // Build the chain
            sellTowerHandler.SetNext(checkMaxUpgradeHandler);
            checkMaxUpgradeHandler.SetNext(checkMoneyHandler);
            checkMoneyHandler.SetNext(upgradeRangeHandler);
            upgradeRangeHandler.SetNext(upgradeFirerateHandler);
        }

        public override void UpgradeOrSellTower(string option, TowerGuiOptions towerOptions)
        {
            sellTowerHandler.Handle(option, tower, towerOptions, _gameState, _gameClient);

            //switch (option) // Depending on the option, either upgrade or sell tower.
            //{
            //    case "Upgrade Range":
            //        if (tower.ShotType.RangeUpgradeCount == 3) break; // Can't upgrade more than 3 times
            //        if (_gameState.Player.Money < tower.ShotType.RangeUpgradeCost) break; // Can't upgrade if player doesn't possess the money.
            //        tower.Range += 50; // Increase range by 50.
            //        towerOptions.SelectedInGui = "none"; // Unselect the option in the gui.
            //        _gameState.Player.Money -= tower.ShotType.RangeUpgradeCost; // Deduct money from player.
            //        tower.SellPrice += 0.7 * tower.ShotType.RangeUpgradeCost; // Add 70% of the price put into the upgrade to the sell price
            //        tower.ShotType.RangeUpgradeCount++;
            //        _ = _gameClient.UpgradeTowerRangeAsync(new UpgradeOrSellTowerRequest(NetworkPoint2D.Serialize(tower.Position), option, tower.ShotType.RangeUpgradeCount));
            //        break;

            //    case "Upgrade Firerate":
            //        if (tower.ShotType.FirerateUpgradeCount == 3) break; // Repeat for firerate
            //        if (_gameState.Player.Money < tower.ShotType.FirerateUpgradeCost) break;
            //        tower.ShotType.ShotSpeed -= 10;
            //        towerOptions.SelectedInGui = "none";
            //        _gameState.Player.Money -= tower.ShotType.FirerateUpgradeCost;
            //        tower.ShotType.FirerateUpgradeCount++;
            //        tower.SellPrice += 0.7 * tower.ShotType.FirerateUpgradeCost;
            //        tower.UpdateDecorator(); // Update the tower's appearance
            //        Console.WriteLine("Upgrading fire rate");
            //        _ = _gameClient.UpgradeTowerFireRateAsync(new UpgradeOrSellTowerRequest(NetworkPoint2D.Serialize(tower.Position), option, tower.ShotType.FirerateUpgradeCount));
            //        break;

            //    case "Sell":
            //        towerOptions.SelectedInGui = "none";
            //        _gameState.Player.Money += tower.SellPrice; // Removes tower and provides player with said tower's sell price.
            //        _ = _gameClient.SellTowerAsync(new UpgradeOrSellTowerRequest(NetworkPoint2D.Serialize(tower.Position), option, 0));
            //        _gameState.Towers.RemoveItem(tower);
            //        _gameState.TowerControlls.RemoveItem(this);
            //        break;
            //}
        }

        public override void SetTowerTargeting(TowerTargetingGuiOptions targetOptions) // Changes the targeting of the tower depending on the target option inputted (enum)
        {
            TargetCreator targetCreator = targetOptions.SelectedInGui switch
            {
                TowerTargeting.First => new ConcreteTargetFirst(),
                TowerTargeting.Last => new ConcreteTargetLast(),
                TowerTargeting.Strong => new ConcreteTargetStrong(),
                TowerTargeting.Weak => new ConcreteTargetWeak(),
                _ => throw new ArgumentException("Invalid targeting strategy type"),
            };
            
            tower.Targeting = targetCreator.CreateTarget();
        }

    }
}
