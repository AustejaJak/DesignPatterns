using System.Collections.Generic;
using BloonsProject;

namespace BloonLibrary.Bloons
{
    public class CompositeBloon : IBloon
    {
        private readonly List<IBloon> _bloons = new List<IBloon>();

        public void Add(IBloon bloon) => _bloons.Add(bloon);
        public void Remove(IBloon bloon) => _bloons.Remove(bloon);

        public void MoveBloonInDirection(Direction direction)
        {
            foreach (var bloon in _bloons)
                bloon.MoveBloonInDirection(direction);
        }

        public void TakeDamage(int damage)
        {
            foreach (var bloon in _bloons)
                bloon.TakeDamage(damage);
        }
        
    }

}