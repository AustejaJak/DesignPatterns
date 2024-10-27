using System;

namespace BloonsProject
{
    public class BloonFactory : IBloonFactory
    {
        public Bloon CreateBloon(string bloonType)
        {
            System.Console.WriteLine("Creating bloon of type: " + bloonType + "Using Abstract Factory");
            switch (bloonType)
            {
                case "Red Bloon":
                    System.Console.WriteLine("Creating Red Bloon");
                    return new RedBloon();
                case "Green Balloon":
                    System.Console.WriteLine("Creating Green Bloon");
                    return new GreenBloon();
                case "Blue Balloon":
                    System.Console.WriteLine("Creating Blue Bloon");
                    return new BlueBloon();
                default:
                    throw new ArgumentException("Invalid bloon type");
            }
        }
    }
}