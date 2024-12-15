using SplashKitSDK;

namespace BloonsProject
{
    public class YellowBloon : Bloon
    {
        public YellowBloon() : base(1, "Yellow Balloon", Color.Yellow, 4, 4)
        {
        }

        public YellowBloon(YellowBloon bloon) : base(1, "Yellow Balloon", Color.Yellow, 4, 4)
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