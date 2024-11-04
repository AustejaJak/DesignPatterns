using BloonsProject;

namespace BloonLibrary
{
    public interface IBloonTowerFactory
    {
        Tower CreateTowerOfType(string towerType, string username);
        Bloon CreateBloonOfType(string bloonType);
    }
}