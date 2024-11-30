using System.IO;

namespace BloonsProject
{
    public static class MapResources
    {
        private static readonly string BaseResourcePath = @"..\..\..\..\BloonsLibrary\Resources";

        public static readonly string MapGui = Path.Combine(BaseResourcePath, "BLOONSGUI3.png");
    }
}