using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BloonsProject
{
    public static class MapManager
    {
        private static List<Map> Maps;

        public static List<Map> GetAllMaps()
        {
            if (Maps == null)
            {
                var listOfMaps = new List<Map>();
                string mapJsonPath = "../BloonsLibrary/Maps/MapJsons";
                DirectoryInfo directoryInfo = new DirectoryInfo(mapJsonPath); // Gets the path of the directory of json files

                foreach (var json in directoryInfo.GetFiles("*.json")) // Iterates through each file
                {
                    string jsonString = File.ReadAllText(json.ToString());
                    listOfMaps.Add(JsonSerializer.Deserialize<Map>(jsonString)); // Deserializes and adds it to list of maps
                }

                Maps = listOfMaps; // Set maps property to the list of maps and returns it
                return listOfMaps;
            }

            return Maps;
        }

        public static Map GetMapByName(string mapName) // Gets map from a string.
        {
            return GetAllMaps().FirstOrDefault(map => map.Name == mapName);
        }
    }
}