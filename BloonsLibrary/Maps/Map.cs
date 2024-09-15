using BloonLibrary.Extensions;
using SplashKitSDK;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BloonsProject
{
    public class Map
    {
        [JsonConstructor]
        public Map(string bloonsMap, int length, int height, int trackWidthRadius, List<VectorExtension> checkpoints, string name)
        {
            BloonsMap = bloonsMap;
            Length = length;
            Height = height;
            TrackWidthRadius = trackWidthRadius;
            Checkpoints = checkpoints;
            Name = name;
        }

        public string BloonsMap { get; } // Bitmap directory for map
        public List<VectorExtension> Checkpoints { get; } // Map checkpoint locations
        public int Height { get; } // Height of map
        public int Length { get; } // Length of map
        public string Name { get; } // Map name
        public int TrackWidthRadius { get; } // Radius of the track the bloons follow

        public Dictionary<Color, int> BloonsPerRound(int round) // Bloons to be spawned each round for each type.
        {
            const int bloonAmount = 1;
            var currentRound = new Dictionary<Color, int>();

            var redBloonAmount = 2 * bloonAmount * (round + 1);
            var blueBloonAmount = bloonAmount * round * round;
            var greenBloonAmount = bloonAmount * (round - 1) * (round - 1) * (round - 1);

            currentRound[Color.Red] = redBloonAmount;
            currentRound[Color.Blue] = blueBloonAmount;
            currentRound[Color.Green] = greenBloonAmount;

            return currentRound;
        }
    }
}