using System;
using BloonsProject;
using SplashKitSDK;

namespace BloonLibrary.Bloons
{
    public interface IBloon
    {
        int Health { get; set; }
        Color Color { get; }
        int Radius { get; }
        Point2D Position { get; set; }
        float VelocityX { get; }
        float VelocityY { get; }

        int Checkpoint { get; set; }
        double DistanceTravelled { get; set; }

        void MoveBloonInDirection(Direction direction); // Method to move the bloon
        void TakeDamage(int damage); // Method to apply damage to the bloon
        Bloon CloneToType(Type targetType); // Method to clone the bloon
    }
}