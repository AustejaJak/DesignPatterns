using System.Collections.Generic;

namespace BloonsProject
{
    public class TargetStrong : ITarget
    {
        public TowerTargeting TargetType => TowerTargeting.Strong; // Returns the bloon with the most health.

        public Bloon BloonToTarget(List<Bloon> bloons)
        {
            Bloon targetBloon = null;
            foreach (var bloon in bloons)
            {
                targetBloon ??= bloon;
                if (targetBloon.Health < bloon.Health)
                {
                    targetBloon = bloon;
                }
            }

            return targetBloon;
        }
    }
}