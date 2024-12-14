using System.Collections.Generic;
using BloonsProject;

namespace BloonLibrary.VisitorImplementation
{
    public class StrongTargetingVisitor : ITargetingVisitor
    {
        public Bloon VisitBloons(List<Bloon> bloons)
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