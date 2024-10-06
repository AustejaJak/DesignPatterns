using SplashKitSDK;

namespace BloonsProject
{
    public class LaserTower : Tower
    {
        public LaserTower(string username) : base("Laser Monkey", username, 400, "Did someone say rapidfire?", new Bitmap("LaserTower", "../BloonsLibrary/Resources/Laser.png"), new LaserShot(), 300)
        {
        }

        public static string Name => "Laser Monkey";

        public static Bitmap Portrait => new Bitmap("Laser Portrait",
            "../BloonsLibrary/Resources/LaserSelect.png");
    }
}