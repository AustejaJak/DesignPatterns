using System.Collections.Generic;
using BloonsProject;

namespace BloonLibrary.VisitorImplementation
{
    public interface ITargetingVisitor
    {
        Bloon VisitBloons(List<Bloon> bloons);
    }
}