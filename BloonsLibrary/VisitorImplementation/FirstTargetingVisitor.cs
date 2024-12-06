using System.Collections.Generic;
using BloonsProject;

namespace BloonLibrary.VisitorImplementation
{
    public class FirstTargetingVisitor : ITargetingVisitor
    {
        public Bloon VisitBloons(List<Bloon> bloons)
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