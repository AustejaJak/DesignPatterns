using System;
using System.IO;
using SplashKitSDK;

namespace BloonsProject
{
    public class Gui
    {
        public static readonly Bitmap GuiBitmap = FlyweightFactory.GetBitmap("GameGUI", MapResources.MapGui);
    }
}