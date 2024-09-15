using SplashKitSDK;
using System.Linq;

namespace BloonsProject
{
    public class TowerController
    {
        private readonly GameState _gameState = GameState.GetGameStateInstance(); // Game state singleton.

        public void AddTower(Tower tower) // Adds a tower
        {
            if (_gameState.Player.Money < tower.Cost) return; // If the player doesn't have the sufficient funds, return
            _gameState.Towers.Add(tower); // Otherwise add the tower
            _gameState.Player.Money -= tower.Cost; // Then remove cost of the tower from the player's money.
        }

        public void ChangeTowerTargeting(TowerTargetingGuiOptions targetOptions, TowerController towerController)
        {
            foreach (var tower in _gameState.Towers.ToList())
            {
                foreach (var targetOption in targetOptions.TargetingButtonLocations.Values.Where( //Depending on the targeting option that has been selected in the gui
                    targetOption => targetOptions.SelectedInGui == targetOption && tower.Selected))
                {
                    towerController.SetTowerTargeting(tower, targetOptions); // Set the targeting option of the tower accordingly.
                }
            }

            targetOptions.SelectedInGui = TowerTargeting.Unselected; // Change to unselected to ensure if another tower is selected, it won't automatically apply the targeting of the previous tower.
        }

        public void DamageBloons()
        {
            foreach (Projectile projectile in _gameState.ProjectileManager.ProjectilesOnScreen) // For every projectile
            {
                foreach (Bloon bloon in _gameState.Bloons) // For every bloon
                {
                    var bloonCircle = new Circle() { Center = bloon.Position, Radius = bloon.Radius * projectile.ProjectileShotType.ProjectileSize };
                    var projectileLocation = new Point2D()
                    { X = projectile.ProjectileLocation.X + projectile.ProjectileShotType.ProjectileWidth / 2, Y = projectile.ProjectileLocation.Y + projectile.ProjectileShotType.ProjectileLength / 2 };
                    if (SplashKit.PointInCircle(projectileLocation, bloonCircle)) // If the projectile (including the width of the projectile's bitmap) collides with the bloon.
                    {
                        var oldBloonHealth = bloon.Health; // Then decrease the health of the bloon depending on the damage of the shot type.
                        bloon.TakeDamage(projectile.ProjectileShotType.Damage);
                        _gameState.Player.Money += oldBloonHealth - bloon.Health; // Gain money depending on the damage dealt.
                    }
                }
            }
        }

        public bool HaveSufficientFundsToPlaceTower(Tower tower) // Determines whether the user has sufficient funds to place a tower.
        {
            return _gameState.Player.Money >= tower.Cost;
        }

        public void SetTowerTargeting(Tower tower, TowerTargetingGuiOptions targetOptions) // Changes the targeting of the tower depending on the target option inputted (enum)
        {
            tower.Targeting = targetOptions.SelectedInGui switch
            {
                TowerTargeting.First => new TargetFirst(),
                TowerTargeting.Last => new TargetLast(),
                TowerTargeting.Strong => new TargetStrong(),
                TowerTargeting.Weak => new TargetWeak(),
                _ => tower.Targeting
            };
        }

        public void ShootBloons(Map map)
        {
            DamageBloons(); // Damages all bloons in radius of the any projectile.
            _gameState.ProjectileManager.IncrementAllProjectiles(); // Increments all projectiles
            if (_gameState.Towers.Count <= 0) return; // If there're no towers, return
            foreach (var tower in _gameState.Towers.Where(t => t.ShotTimer()))
            {
                if (!tower.ShotTimer()) continue; // If the tower is on cooldown, continue to the next.
                var bloonsInTowerRadius = _gameState.Bloons.Where(b =>
                    SplashKit.PointInCircle(b.Position, SplashKit.CircleAt(tower.Position, tower.Range))).ToList(); // Return all bloons in radius of the tower.
                if (bloonsInTowerRadius.Count == 0) continue; // If there's no bloons in radius of the tower, continue to the next.

                var bloonToTarget = tower.Targeting.BloonToTarget(bloonsInTowerRadius); // Depending on the tower's targeting, get the bloon to target.
                var projectileEndPoint = _gameState.ProjectileManager.GetProjectileEndPoint(bloonToTarget, tower); // Extrapolate the distance to attain the endpoint of the projectile utilizing an algorithm.
                _gameState.ProjectileManager.AddProjectile(tower.Position, projectileEndPoint, tower.ShotType); // Add the projectile to the list of projectiles.
                tower.ResetTimer(); // Reset tower cooldown.
            }
        }

        public void TickAllTowers()
        {
            foreach (Tower tower in _gameState.Towers) // Increments the cooldown ticks of all towers
            {
                tower.ShotTimerTick();
            }
        }

        public void UpgradeOrSellSelectedTower(TowerController towerController, TowerGuiOptions towerOptions)
        {
            foreach (var tower in _gameState.Towers.ToList())
            {
                foreach (var towerOption in towerOptions.UpgradeOptionsInGui.Values.Where( // Obtains the selected tower and the option (upgrade, sell) that's currently selected in the gui
                    towerOption => towerOptions.SelectedInGui == towerOption && tower.Selected))
                {
                    towerController.UpgradeOrSellTower(tower, towerOption, towerOptions); // Apply the selected option to the selected tower
                }
            }

            towerOptions.SelectedInGui = "none"; // Reset the selected option so it doesn't apply to all future bloons.
        }

        public void UpgradeOrSellTower(Tower tower, string option, TowerGuiOptions towerOptions)
        {
            switch (option) // Depending on the option, either upgrade or sell tower.
            {
                case "Upgrade Range":
                    if (tower.ShotType.RangeUpgradeCount == 3) break; // Can't upgrade more than 3 times
                    if (_gameState.Player.Money < tower.ShotType.RangeUpgradeCost) break; // Can't upgrade if player doesn't possess the money.
                    tower.Range += 50; // Increase range by 50.
                    towerOptions.SelectedInGui = "none"; // Unselect the option in the gui.
                    _gameState.Player.Money -= tower.ShotType.RangeUpgradeCost; // Deduct money from player.
                    tower.SellPrice += 0.7 * tower.ShotType.RangeUpgradeCost; // Add 70% of the price put into the upgrade to the sell price
                    tower.ShotType.RangeUpgradeCount++;
                    break;

                case "Upgrade Firerate":
                    if (tower.ShotType.FirerateUpgradeCount == 3) break; // Repeat for firerate
                    if (_gameState.Player.Money < tower.ShotType.FirerateUpgradeCost) break;
                    tower.ShotType.ShotSpeed -= 10;
                    towerOptions.SelectedInGui = "none";
                    _gameState.Player.Money -= tower.ShotType.FirerateUpgradeCost;
                    tower.ShotType.FirerateUpgradeCount++;
                    tower.SellPrice += 0.7 * tower.ShotType.FirerateUpgradeCost;
                    break;

                case "Sell":
                    towerOptions.SelectedInGui = "none";
                    _gameState.Player.Money += tower.SellPrice; // Removes tower and provides player with said tower's sell price.
                    _gameState.Towers.Remove(tower);
                    break;
            }
        }
    }
}
