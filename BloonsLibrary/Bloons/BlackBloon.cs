using SplashKitSDK;

namespace BloonsProject
{
    public class BlackBloon : Bloon
    {
        public BlackBloon() : base(2, "Black Balloon", Color.Black, 4, 4)
        {
        }

        public BlackBloon(BlackBloon bloon) : base(2, "Black Balloon", Color.Black, 4, 4)
        {
            this.Position = bloon.Position;
            this.Checkpoint = bloon.Checkpoint;
            this.DistanceTravelled = bloon.DistanceTravelled;
        }


        public static string Name => "Black Balloon";

        public override Bloon Clone()
        {
            return new BlackBloon(this);
        }
    }
}