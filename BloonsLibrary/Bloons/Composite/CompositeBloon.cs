using System;
using SplashKitSDK;
using System.Collections.Generic;
using System.Linq;
using BloonsProject;

namespace BloonLibrary.Bloons
{
    public class CompositeBloon : Bloon
    {
        // Changed from List<IBloon> to List<Bloon> to store only Bloon objects.
        public List<Bloon> Children = new List<Bloon>();

        public CompositeBloon(int health = 1) : base(health, "CompositeBloon", Color.Black, 1, 1)
        {
        }

        // Method to add a child bloon to the composite
        public void Add(Bloon bloon)
        {
            Children.Add(bloon);
            Health += bloon.Health; // Increase composite health by the child's health
        }

        // Method to remove a child bloon from the composite
        public void Remove(Bloon bloon)
        {
            if (Children.Remove(bloon))
            {
                // Decrease composite health when removing a bloon
                Health -= bloon.Health;
            }
        }

        // Override move method to move both the composite and its children
        public override void MoveBloonInDirection(Direction direction)
        {
            // Move the composite bloon itself
            base.MoveBloonInDirection(direction);

            // Move each child bloon
            foreach (var bloon in Children)
            {
                bloon.MoveBloonInDirection(direction);
            }
        }

        // Override the damage-taking method, distributing damage among children
        public override void TakeDamage(int damage)
        {
            if (Children.Any())
            {
                // Distribute damage equally among children
                int damagePerChild = damage / Children.Count;
                foreach (var bloon in Children.ToList())
                {
                    bloon.TakeDamage(damagePerChild);
                    
                    // Remove children that are dead
                    if (bloon.Health <= 0)
                    {
                        Remove(bloon);
                    }
                }
            }
            else
            {
                base.TakeDamage(damage); // No children, take damage as normal
            }
        }

        // Clone method for the composite bloon, cloning all its children as well
        public override Bloon CloneToType(Type targetType)
        {
            CompositeBloon clone = (CompositeBloon)base.CloneToType(targetType);
            
            // Clone each child and add to the new composite bloon
            foreach (var child in Children)
            {
                clone.Add(child.CloneToType(child.GetType()));
            }
            
            return clone;
        }
        
        // Read-only list of children
        public IReadOnlyList<Bloon> GetChildren() => Children.AsReadOnly();
        
        // Property to get the count of children
        public int ChildCount => Children.Count;
        
        // Property to get and set the position of the composite bloon and its children
        public new Point2D Position
        {
            get => base.Position;
            set
            {
                var deltaX = value.X - Position.X;
                var deltaY = value.Y - Position.Y;
                
                base.Position = value;
                
                // Update the position of each child bloon
                foreach (var child in Children)
                {
                    child.Position = new Point2D()
                    {
                        X = child.Position.X + deltaX,
                        Y = child.Position.Y + deltaY
                    };
                }
            }
        }
    }
}
