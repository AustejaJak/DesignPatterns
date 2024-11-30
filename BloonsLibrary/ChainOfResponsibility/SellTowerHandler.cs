using BloonLibrary.Controllers.Bridge;
using BloonsProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonLibrary.ChainOfResponsibility
{
    public class SellTowerHandler : TowerActionHandler
    {
        public override void Handle(string option, Tower tower, TowerGuiOptions towerOptions, GameState gameState, GameClient gameClient)
        {
            if (option == "Sell")
            {
                towerOptions.SelectedInGui = "none";
                gameState.Player.Money += tower.SellPrice;
                
                
                var ControllsIterator = gameState.TowerControlls.CreateIterator();
                while (ControllsIterator.MoveNext())
                {
                    if (ControllsIterator.Current.GetTower() == null || ControllsIterator.Current.GetTower() == tower)
                    {
                        gameState.TowerControlls.RemoveItem(ControllsIterator.Current);
                        break;
                    }
                }
                gameState.Towers.RemoveItem(tower);
                _ = gameClient.SellTowerAsync(
                    new UpgradeOrSellTowerRequest(NetworkPoint2D.Serialize(tower.Position), option, 0));
            }
            else
            {
                _nextHandler?.Handle(option, tower, towerOptions, gameState, gameClient);
            }
        }
    }

}
