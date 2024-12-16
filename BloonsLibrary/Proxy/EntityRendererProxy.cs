using BloonsProject;

namespace BloonLibrary.Proxy
{
    public class EntityRendererProxy : IEntityRenderer
    {
        private RealEntityRenderer _realEntityRenderer;

        public void DrawBloon(Bloon bloon)
        {
            if (_realEntityRenderer == null)
            {
                _realEntityRenderer = new RealEntityRenderer();
            }

            _realEntityRenderer.DrawBloon(bloon);
        }

        public void DrawTower(Tower tower)
        {
            if (_realEntityRenderer == null)
            {
                _realEntityRenderer = new RealEntityRenderer();
            }

            _realEntityRenderer.DrawTower(tower);
        }

        public void DrawTowerRange(Tower tower)
        {
            if (_realEntityRenderer == null)
            {
                _realEntityRenderer = new RealEntityRenderer();
            }

            _realEntityRenderer.DrawTowerRange(tower);
        }
        
        public void TowerProjectileRenderer()
        {
            if (_realEntityRenderer == null)
            {
                _realEntityRenderer = new RealEntityRenderer();
            }
            
            _realEntityRenderer.TowerProjectileRenderer();
        }
    }
}