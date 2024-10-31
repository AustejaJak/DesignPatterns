using System;
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
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string mapJsonPath = Path.Combine(baseDirectory, @"..\..\..\..\BloonsLibrary\Maps\MapJsons");
                DirectoryInfo directoryInfo = new DirectoryInfo(mapJsonPath);

                foreach (var json in directoryInfo.GetFiles("*.json"))
                {
                    string jsonString = File.ReadAllText(json.ToString());
                    var map = JsonSerializer.Deserialize<Map>(jsonString);
    
                    // Use the path from JSON, making it a full path
                    map.BloonsMap = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\..\BloonsLibrary\", map.BloonsMap));
    
                    listOfMaps.Add(map);
                }

                Maps = listOfMaps;
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