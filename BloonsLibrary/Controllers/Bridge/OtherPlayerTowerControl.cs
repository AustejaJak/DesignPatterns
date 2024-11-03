using BloonsProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BloonLibrary.Controllers.Bridge
{
    public class OtherPlayerTowerControl : TowerContols
    {
        public OtherPlayerTowerControl(Tower t, GameClient client) : base(t, client)
        {
        }

        public override void UpgradeOrSellTower(string option, TowerGuiOptions towerOptions)
        {
            switch (option) // Depending on the option, either upgrade or sell tower.
            {
                case "Upgrade Range":
                    _gameState.InvalidTowerEventMessage = "Can't upgrade other players tower range";
                    break;

                case "Upgrade Firerate":
                    _gameState.InvalidTowerEventMessage = "Can't upgrade other players tower firerate";
                    break;

                case "Sell":
                    _gameState.InvalidTowerEventMessage = "Can't sell other players tower";
                    break;
            }

        }

        public override void SetTowerTargeting(TowerTargetingGuiOptions targetOptions) // Changes the targeting of the tower depending on the target option inputted (enum)
        {
            _gameState.InvalidTowerEventMessage = "Can't change targeting of other players tower";
        }
    }
}
