using BloonsProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonLibrary.ChainOfResponsibility
{
    public class CheckMoneyHandler : TowerActionHandler
    {
        public override void Handle(string option, Tower tower, TowerGuiOptions towerOptions, GameState gameState, GameClient gameClient)
        {
            var cost = option switch
            {
                "Upgrade Range" => tower.ShotType.RangeUpgradeCost,
                "Upgrade Firerate" => tower.ShotType.FirerateUpgradeCost,
                _ => 0
            };

            if (cost > 0 && gameState.Player.Money < cost)
            {
                Console.WriteLine("Not enough money for this action.");
                return; // Stop the chain
            }

            _nextHandler?.Handle(option, tower, towerOptions, gameState, gameClient);
        }
    }
}
