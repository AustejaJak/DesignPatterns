using System;

namespace BloonsServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Bloons server";
            Server.Start(4, 26950);
            Console.ReadKey();
        }

    }
}
