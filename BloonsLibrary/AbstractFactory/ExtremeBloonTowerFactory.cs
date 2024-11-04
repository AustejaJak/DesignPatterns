using System;
using BloonsProject;

namespace BloonLibrary
{
    public class ExtremeBloonTowerFactory : IBloonTowerFactory
    {
        public Bloon CreateBloonOfType(string bloon)
        {
            if (bloon == BlackBloon.Name) return new BlackBloon();

            if (bloon == OrangeBloon.Name) return new OrangeBloon();

            if (bloon == YellowBloon.Name) return new YellowBloon();

            throw new Exception("You are trying to create a bloon type that does not exist.");
        }
            
        public Tower CreateTowerOfType(string tower, string username)
        {
            if (tower == DartTower.Name) return new UpgradedDartTower(username);

            if (tower == LaserTower.Name) return new UpgradedLaserTower(username);

            if (tower == SniperTower.Name) return new UpgradedSniperTower(username);

            throw new Exception("You are trying to create a tower type that does not exist.");
        }
    }
}