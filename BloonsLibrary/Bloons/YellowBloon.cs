using SplashKitSDK;

namespace BloonsProject
{
    public class YellowBloon : Bloon
    {
        public YellowBloon() : base(5, "Yellow Balloon", Color.Orange, 2, 2)
        {
        }

        public YellowBloon(YellowBloon bloon) : base(5, "Yellow Balloon", Color.Orange, 2, 2)
        {
            this.Position = bloon.Position;
            this.Checkpoint = bloon.Checkpoint;
            this.DistanceTravelled = bloon.DistanceTravelled;
        }


        public static string Name => "Yellow Balloon";

        //public override Bloon Clone()
        //{
        //    return new YellowBloon(this);
        //}
    }
}