using System;
using System.IO;
using SplashKitSDK;

namespace BloonsProject
{
    public class Gui
    {
        private static string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public static Bitmap GuiBitmap = new Bitmap("GameGUI", Path.Combine(baseDirectory, @"..\..\..\..\BloonsLibrary\Resources\BLOONSGUI3.png"));
    }
}