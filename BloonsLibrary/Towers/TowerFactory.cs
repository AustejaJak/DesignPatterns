using System;

namespace BloonsProject
{
    public static class TowerFactory
    {
        public static Tower CreateTowerOfType(string tower)
        {
            if (tower == DartTower.Name) return new DartTower();

            if (tower == LaserTower.Name) return new LaserTower();

            if (tower == SniperTower.Name) return new SniperTower();

            throw new Exception("You are trying to create a tower type that does not exist.");
        }
    }
}