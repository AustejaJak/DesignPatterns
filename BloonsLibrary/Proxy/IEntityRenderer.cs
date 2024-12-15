using BloonsProject;

namespace BloonLibrary.Proxy
{
    public interface IEntityRenderer
    {
        void DrawBloon(Bloon bloon);
        void DrawTower(Tower tower);
        void DrawTowerRange(Tower tower);
        void TowerProjectileRenderer();
    }
}