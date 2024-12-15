using BloonsProject;

namespace BloonLibrary.VisitorImplementation
{
    public interface IUpgradeOptionVisitor
    {
        void Visit(Tower tower);
    }
}