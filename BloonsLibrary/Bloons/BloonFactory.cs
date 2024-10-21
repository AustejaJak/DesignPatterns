using System;

namespace BloonsProject
{
    public static class BloonFactory
    {

        public static Bloon CreateBloon(string bloonType)
        {
            switch (bloonType)
            {
                case "Red Bloon":
                    return new RedBloon();
                case "Green Balloon":
                    return new GreenBloon();
                case "Blue Balloon":
                    return new BlueBloon();
                default:
                    throw new ArgumentException("Invalid bloon type");
            }
        }
    }
}