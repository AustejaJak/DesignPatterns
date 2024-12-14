using BloonLibrary;
using SplashKitSDK;
using System.Linq;
using System;
using System.Collections.Generic;

namespace BloonsProject
{
    public class TowerController
    {
        public GameClient GameClient;
        public TowerController(GameClient gameClient)
        {
            GameClient = gameClient;
        }
        private readonly GameState _gameState = GameState.GetGameStateInstance(); // Game state singleton.

        public void AddTower(Tower tower) // Adds a tower
        {
            if (_gameState.Player.Money < tower.Cost) return; // If the player doesn't have the sufficient funds, return
            tower.Username = GameClient.Username; 
            //_gameState.Towers.Add(tower);// Otherwise add the tower
            _ = GameClient.PlaceTowerAsync(new PlaceTowerRequest(tower._name, NetworkPoint2D.Serialize(tower.Position), tower.Username));
            _gameState.Player.Money -= tower.Cost; // Then remove cost of the tower from the player's money.
        }

        //public void ChangeTowerTargeting(TowerTargetingGuiOptions targetOptions, TowerController towerController)
        //{
        //    foreach (var tower in _gameState.Towers.ToList())
        //    {
        //        foreach (var targetOption in targetOptions.TargetingButtonLocations.Values.Where( //Depending on the targeting option that has been selected in the gui
        //            targetOption => targetOptions.SelectedInGui == targetOption && tower.Selected))
        //        {
        //            towerController.SetTowerTargeting(tower, targetOptions); // Set the targeting option of the tower accordingly.
        //        }
        //    }

        //    targetOptions.SelectedInGui = TowerTargeting.Unselected; // Change to unselected to ensure if another tower is selected, it won't automatically apply the targeting of the previous tower.
        //}

        public void ChangeTowerTargeting(TowerTargetingGuiOptions targetOptions, TowerController towerController)
        {
            var iterator = _gameState.TowerControlls.CreateIterator();
            while (iterator.MoveNext())
            {
                foreach (var targetOption in targetOptions.TargetingButtonLocations.Values.Where( //Depending on the targeting option that has been selected in the gui
                    targetOption => targetOptions.SelectedInGui == targetOption && iterator.Current.IsTowerSelected()))
                {
                    //towerController.SetTowerTargeting(tower, targetOptions); // Set the targeting option of the tower accordingly.
                    iterator.Current.SetTowerTargeting(targetOptions);
                }
            }

            targetOptions.SelectedInGui = TowerTargeting.Unselected; // Change to unselected to ensure if another tower is selected, it won't automatically apply the targeting of the previous tower.
        }

        public void DamageBloons()
        {
            foreach (Projectile projectile in _gameState.ProjectileManager.ProjectilesOnScreen) // For every projectile
            {
                foreach (Bloon bloon in _gameState.Bloons.Values) // For every bloon
                {
                    var bloonCircle = new Circle() { Center = bloon.Position, Radius = bloon.Radius * projectile.ProjectileShotType.ProjectileSize };
                    var projectileLocation = new Point2D()
                        { X = projectile.ProjectileLocation.X + projectile.ProjectileShotType.ProjectileWidth / 2, Y = projectile.ProjectileLocation.Y + projectile.ProjectileShotType.ProjectileLength / 2 };
                    if (SplashKit.PointInCircle(projectileLocation, bloonCircle)) // If the projectile (including the width of the projectile's bitmap) collides with the bloon.
                    {
                        var oldBloonHealth = bloon.Health; // Then decrease the health of the bloon depending on the damage of the shot type.
                        bloon.TakeDamage(projectile.ProjectileShotType.Damage);
                        _gameState.Player.Money += oldBloonHealth - bloon.Health; // Gain money depending on the damage dealt.
                        //_gameState.Player.MoneyEarned += oldBloonHealth - bloon.Health;
                        
                    }
                }
            }
        }


        public bool HaveSufficientFundsToPlaceTower(Tower tower) // Determines whether the user has sufficient funds to place a tower.
        {
            return _gameState.Player.Money >= tower.Cost;
        }

        public void SetTowerTargeting(Tower tower, TowerTargetingGuiOptions targetOptions)
        {
            tower.SetTargeting(targetOptions.SelectedInGui);
        }

        public void ShootBloons(Map map)
        {
            DamageBloons();
            _gameState.ProjectileManager.IncrementAllProjectiles();
    
            var TowerIterator = _gameState.Towers.CreateIterator();
            while (TowerIterator.MoveNext()) 
            { 
                if (TowerIterator.Current.ShotTimer())
                {
                    var bloonsInTowerRadius = _gameState.Bloons
                        .Where(bloon => SplashKit.PointInCircle(bloon.Value.Position, 
                            SplashKit.CircleAt(TowerIterator.Current.Position, TowerIterator.Current.Range)))
                        .Select(bloon => bloon.Value)
                        .ToList();

                    if (bloonsInTowerRadius.Count == 0) continue;

                    // Use the new visitor-based targeting
                    var bloonToTarget = TowerIterator.Current.GetTargetBloon(bloonsInTowerRadius);
                    var projectileEndPoint = _gameState.ProjectileManager.GetProjectileEndPoint(bloonToTarget, TowerIterator.Current);
                    _gameState.ProjectileManager.AddProjectile(TowerIterator.Current.Position, projectileEndPoint, TowerIterator.Current.ShotType);
                    TowerIterator.Current.ResetTimer();
                }
            }
        }


        public void TickAllTowers()
        {
            var TowerIterator = _gameState.Towers.CreateIterator();
            while (TowerIterator.MoveNext()) // Increments the cooldown ticks of all towers
            {
                TowerIterator.Current.ShotTimerTick();
            }
        }

        //public void UpgradeOrSellSelectedTower(TowerController towerController, TowerGuiOptions towerOptions)
        //{
        //    foreach (var tower in _gameState.Towers.ToList())
        //    {
        //        foreach (var towerOption in towerOptions.UpgradeOptionsInGui.Values.Where( // Obtains the selected tower and the option (upgrade, sell) that's currently selected in the gui
        //            towerOption => towerOptions.SelectedInGui == towerOption && tower.Selected))
        //        {
        //            towerController.UpgradeOrSellTower(tower, towerOption, towerOptions); // Apply the selected option to the selected tower
        //        }
        //    }

        //    towerOptions.SelectedInGui = "none"; // Reset the selected option so it doesn't apply to all future bloons.
        //}

        public void UpgradeOrSellSelectedTower(TowerController towerController, TowerGuiOptions towerOptions)
        {
            var iterator = _gameState.TowerControlls.CreateIterator();
            while (iterator.MoveNext())
            {
                foreach (var towerOption in towerOptions.UpgradeOptionsInGui.Values.Where( // Obtains the selected tower and the option (upgrade, sell) that's currently selected in the gui
                    towerOption => towerOptions.SelectedInGui == towerOption && iterator.Current.IsTowerSelected()))
                {
                    //towerController.UpgradeOrSellTower(tower, towerOption, towerOptions); // Apply the selected option to the selected tower
                    iterator.Current.UpgradeOrSellTower(towerOption, towerOptions);
                }
            }

            towerOptions.SelectedInGui = "none"; // Reset the selected option so it doesn't apply to all future bloons.
        }

        public void UpgradeOrSellTower(Tower tower, string option, TowerGuiOptions towerOptions)
        {
            Console.WriteLine("Upgrading or selling tower");
            switch (option) // Depending on the option, either upgrade or sell tower.
            {
                case "Upgrade Range":
                    if (tower.ShotType.RangeUpgradeCount == 3) break;
                    if (_gameState.Player.Money < tower.ShotType.RangeUpgradeCost) break;
                    tower.Range += 50;
                    towerOptions.SelectedInGui = "none";
                    _gameState.Player.Money -= tower.ShotType.RangeUpgradeCost;
                    tower.SellPrice += 0.7 * tower.ShotType.RangeUpgradeCost;
                    tower.ShotType.RangeUpgradeCount++;
                    //tower.UpdateDecorator(); // Update the tower's appearance
                    _ = GameClient.UpgradeTowerRangeAsync(new UpgradeOrSellTowerRequest(NetworkPoint2D.Serialize(tower.Position), option, tower.ShotType.RangeUpgradeCount));
                    break;

                case "Upgrade Firerate":
                    if (tower.ShotType.FirerateUpgradeCount == 3) break;
                    if (_gameState.Player.Money < tower.ShotType.FirerateUpgradeCost) break;
                    tower.ShotType.ShotSpeed -= 10;
                    towerOptions.SelectedInGui = "none";
                    _gameState.Player.Money -= tower.ShotType.FirerateUpgradeCost;
                    tower.ShotType.FirerateUpgradeCount++;
                    tower.SellPrice += 0.7 * tower.ShotType.FirerateUpgradeCost;
                    tower.UpdateDecorator(); // Update the tower's appearance
                    Console.WriteLine("Upgrading fire rate");
                    _ = GameClient.UpgradeTowerFireRateAsync(new UpgradeOrSellTowerRequest(NetworkPoint2D.Serialize(tower.Position), option, tower.ShotType.FirerateUpgradeCount));
                    break;

                case "Sell":
                    towerOptions.SelectedInGui = "none";
                    _gameState.Player.Money += tower.SellPrice; // Removes tower and provides player with said tower's sell price.
                    _gameState.Towers.RemoveItem(tower);
                    _ = GameClient.SellTowerAsync(new UpgradeOrSellTowerRequest(NetworkPoint2D.Serialize(tower.Position), option, 0));
                    break;
            }
        }
    }
}