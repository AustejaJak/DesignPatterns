using BloonLibrary.VisitorImplementation;

namespace BloonsProject
{
    public interface IUpgradeable
    {
         void Accept(IUpgradeOptionVisitor visitor);
    }
}