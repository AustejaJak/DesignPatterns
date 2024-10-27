using SplashKitSDK;

namespace BloonsProject
{
    public class GreenBloon : Bloon
    {
        public GreenBloon() : base(3, "Green Balloon", Color.Green, 4, 4)
        {
        }
        
        public static string Name => "Green Balloon";
    }
}