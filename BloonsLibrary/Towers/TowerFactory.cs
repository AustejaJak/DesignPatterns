using System;

namespace BloonsProject
{
    public static class TowerFactory
    {
        public static Tower CreateTowerOfType(string tower, string username)
        {
            System.Console.WriteLine("Creating tower of type: " + tower + " Using Factory");
            if (tower == DartTower.Name) {
                System.Console.WriteLine("Creating Dart Tower");
                return new DartTower(username);
            }

            if (tower == LaserTower.Name) {
                System.Console.WriteLine("Creating Laser Tower");
                return new LaserTower(username);
            }

            if (tower == SniperTower.Name) {
                System.Console.WriteLine("Creating Sniper Tower");
                return new SniperTower(username);
            }

            throw new Exception("You are trying to create a tower type that does not exist.");
        }
    }
}