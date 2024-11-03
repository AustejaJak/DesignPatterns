using System;
using BloonLibrary;

namespace BloonsProject
{
    public class TowerFactory : IBloonTowerFactory
    {
        public Tower CreateTowerOfType(string tower, string username)
        {
            if (tower == DartTower.Name) return new DartTower(username);

            if (tower == LaserTower.Name) return new LaserTower(username);

            if (tower == SniperTower.Name) return new SniperTower(username);

            throw new Exception("You are trying to create a tower type that does not exist.");
        }
        public Bloon CreateBloonOfType(string bloonType)
        {
            throw new NotImplementedException("This factory does not create bloons.");
        }
    }
}