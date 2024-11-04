using SplashKitSDK;

namespace BloonsProject
{
    public class GreenBloon : Bloon
    {
        public GreenBloon() : base(3, "Green Balloon", Color.Green, 4, 4)
        {
        }

        public GreenBloon(Bloon bloon) : base(3, "Green Balloon", Color.Green, 4, 4)
        {
            this.Position = bloon.Position;
            this.Checkpoint = bloon.Checkpoint;
            this.DistanceTravelled = bloon.DistanceTravelled;
        }

        
        
        public static string Name => "Green Balloon";

        //public override Bloon Clone()
        //{
        //    return new GreenBloon(this);
        //}
    }
}