using System.Collections.Generic;

namespace BloonsProject
{
    public class TargetFirst : ITarget
    {
        public TowerTargeting TargetType => TowerTargeting.First;

        public Bloon BloonToTarget(List<Bloon> bloons) // Returns the bloon that has travelled the most distance.
        {
            Bloon targetBloon = null;
            foreach (var bloon in bloons)
            {
                targetBloon ??= bloon;
                if (targetBloon.DistanceTravelled < bloon.DistanceTravelled)
                {
                    targetBloon = bloon;
                }
            }

            return targetBloon;
        }
    }
}