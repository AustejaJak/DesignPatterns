using SplashKitSDK;

namespace BloonsCreator
{
    public class SaveButton : Button
    {
        public SaveButton() : base(300, 100, new Bitmap("savegame", "../../BloonsLibrary/Resources/Savegame.png"), ButtonTypes.Save)
        {
            Position = new Point2D() { X = 275, Y = 575 };
            SaveButtonRectangle = SplashKit.RectangleFrom(new Point2D() { X = 250, Y = 575 }, Width, Height);
        }

        public Rectangle SaveButtonRectangle { get; }
    }
}