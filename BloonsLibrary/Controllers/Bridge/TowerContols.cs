using BloonsProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonLibrary.Controllers.Bridge
{
    public abstract class TowerContols
    {
        protected Tower tower;

        protected readonly GameState _gameState = GameState.GetGameStateInstance();

        protected GameClient _gameClient;

        protected TowerContols(Tower t, GameClient client)
        {
            tower = t;
            _gameClient = client;
        }

        public bool IsTowerSelected()
        {
            return tower.Selected;
        }

        public Tower GetTower()
        {
            return tower;
        }

        public abstract void UpgradeOrSellTower(string option, TowerGuiOptions towerOptions);
        public abstract void SetTowerTargeting(TowerTargetingGuiOptions targetOptions);
    }
}
