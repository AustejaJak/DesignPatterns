using System;
using BloonsProject;

namespace BloonLibrary
{
    public class StandardBloonTowerFactory : IBloonTowerFactory
    {
        public Bloon CreateBloonOfType(string bloon)
        {
            if (bloon == BlueBloon.Name) return new BlueBloon();

            if (bloon == GreenBloon.Name) return new GreenBloon();

            if (bloon == RedBloon.Name) return new RedBloon();
            
            if (bloon == OrangeBloon.Name) return new OrangeBloon();
            
            if (bloon == BlackBloon.Name) return new BlackBloon();

            if (bloon == YellowBloon.Name) return new YellowBloon();

            throw new Exception("You are trying to create a bloon type that does not exist.");
        }
            
        public Tower CreateTowerOfType(string tower, string username)
        {
            if (tower == DartTower.Name) return new DartTower(username);

            if (tower == LaserTower.Name) return new LaserTower(username);

            if (tower == SniperTower.Name) return new SniperTower(username);

            throw new Exception("You are trying to create a tower type that does not exist.");
        }
    }
}