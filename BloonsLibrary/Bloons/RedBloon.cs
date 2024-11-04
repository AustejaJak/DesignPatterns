using SplashKitSDK;

namespace BloonsProject
{
    public class RedBloon : Bloon
    {
        public RedBloon() : base(1, "Red Bloon", Color.Red, 2, 2)
        {
        }

        public RedBloon(Bloon bloon) : base(1, "Red Bloon", Color.Red, 2, 2)
        {
            this.Position = bloon.Position;
            this.Checkpoint = bloon.Checkpoint;
            this.DistanceTravelled = bloon.DistanceTravelled;
        }
        
        public static string Name => "Red Bloon";

        //public override Bloon Clone()
        //{
        //    return new RedBloon(this);
        //}
    }
}