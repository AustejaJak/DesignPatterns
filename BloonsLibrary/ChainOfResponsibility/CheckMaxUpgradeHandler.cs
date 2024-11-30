using BloonsProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonLibrary.ChainOfResponsibility
{
    public class CheckMaxUpgradeHandler : TowerActionHandler
    {
        public override void Handle(string option, Tower tower, TowerGuiOptions towerOptions, GameState gameState, GameClient gameClient)
        {
            var maxedOut = option switch
            {
                "Upgrade Range" => tower.ShotType.RangeUpgradeCount >= 3,
                "Upgrade Firerate" => tower.ShotType.FirerateUpgradeCount >= 3,
                _ => false
            };

            if (maxedOut)
            {
                Console.WriteLine("Tower has reached the maximum upgrade level.");
                return; // Stop the chain
            }

            _nextHandler?.Handle(option, tower, towerOptions, gameState, gameClient);
        }
    }

}
