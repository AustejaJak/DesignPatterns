using SplashKitSDK;

namespace BloonsProject
{
    public class BlueBloon : Bloon
    {
        public BlueBloon() : base(2, "Blue Balloon", Color.Blue, 3, 3)
        {
        }

        public BlueBloon(BlueBloon bloon) : base(2, "Blue Balloon", Color.Blue, 3, 3)
        {
            this.Position = bloon.Position;
            this.Checkpoint = bloon.Checkpoint;
            this.DistanceTravelled = bloon.DistanceTravelled;
        }


        public static string Name => "Blue Balloon";

        public override Bloon Clone()
        {
            return new BlueBloon(this);
        }
    }
}