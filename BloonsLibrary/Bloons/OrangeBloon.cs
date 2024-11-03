using SplashKitSDK;

namespace BloonsProject
{
    public class OrangeBloon : Bloon
    {
        public OrangeBloon() : base(2, "Orange Balloon", Color.Orange, 5, 5)
        {
        }

        public OrangeBloon(OrangeBloon bloon) : base(2, "Orange Balloon", Color.Orange, 5, 5)
        {
            this.Position = bloon.Position;
            this.Checkpoint = bloon.Checkpoint;
            this.DistanceTravelled = bloon.DistanceTravelled;
        }


        public static string Name => "Orange Balloon";

        public override Bloon Clone()
        {
            return new OrangeBloon(this);
        }
    }
}