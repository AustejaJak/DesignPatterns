using System.Collections.Generic;

namespace BloonsProject
{
    public interface ITarget
    {
        TowerTargeting TargetType { get; }

        Bloon BloonToTarget(List<Bloon> bloons);
    }
}