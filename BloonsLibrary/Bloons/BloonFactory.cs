using System;

namespace BloonsProject
{
    public class BloonFactory
    {
            public static Bloon CreateBloonOfType(string bloon)
            {
                if (bloon == BlueBloon.Name) return new BlueBloon();

                if (bloon == GreenBloon.Name) return new GreenBloon();

                if (bloon == RedBloon.Name) return new RedBloon();

                throw new Exception("You are trying to create a bloon type that does not exist.");
            }
    }
}
