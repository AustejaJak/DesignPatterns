using BloonLibrary.Bloons;
using SplashKitSDK;
using System;
using System.Diagnostics;

namespace BloonsProject
{
    // One Bloon logic
    public abstract class Bloon : IBloon
    {
        private Point2D _position;
        public string Name;

        public Bloon(int health, string name, Color color, int velocityX, int velocityY)
        {
            Health = health;
            Name = name;
            Color = color;
            Radius = 15;
            _position.X = 20;
            _position.Y = 100;
            VelocityX = velocityX;
            VelocityY = velocityY;
            Checkpoint = 0;
            DistanceTravelled = 0;
        }
        
        public int Checkpoint { get; set; } // Last checkpoint passed on the map.
        public Color Color { get; } // Bloons have unique colours.
        public double DistanceTravelled { get; set; } // How far along the map the bloon has travelled
        public int Health { get; set; } // Health of bloon.

        public Point2D Position //Current position on map.
        {
            get => _position;
            set => _position = value;
        }

        public int Radius { get; } //Radius of bloon on map.
        public float VelocityX { get; set; } // Magnitute at which bloon's x coordinate changes with each movement.
        public float VelocityY { get; set; } // Magnitute at which bloon's y coordinate changes with each movement.

        public virtual void MoveBloonInDirection(Direction direction) // Provides a direction (enum) and moves the bloon in that direction
        {
            switch (direction)
            {
                case Direction.Right:
                    _position.X += VelocityX;
                    break;

                case Direction.Left:
                    _position.X -= VelocityX;
                    break;

                case Direction.Up:
                    _position.Y -= VelocityY;
                    break;

                case Direction.Down:
                    _position.Y += VelocityY;
                    break;
            }
        }

        public virtual void TakeDamage(int damage) // Decreases health of the bloon by the damage of the projectile it is shot by.
        {
            if (damage > Health) Health = damage;
            Health -= damage;
        }
        
        public virtual Bloon CloneToType(Type targetType)
        {
            Bloon clone = (Bloon)Activator.CreateInstance(targetType);
        
            clone.Position = this.Position;
            clone.Checkpoint = this.Checkpoint;
            clone.DistanceTravelled = this.DistanceTravelled;
        
            return clone;
        }

    }
}