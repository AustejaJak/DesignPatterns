using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using BloonLibrary;
using BloonLibrary.Adapter;

namespace BloonsProject
{
    public static class MapManager
    {
        private static List<Map> Maps;
        private static readonly List<IMapFileAdapter> MapFileAdapters = new List<IMapFileAdapter>
        {
            new JsonMapFileAdapter(),
            new XmlMapFileAdapter()
        };

        public static List<Map> GetAllMaps()
        {
            if (Maps == null)
            {
                var listOfMaps = new List<Map>();
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string mapFilesPath = Path.Combine(baseDirectory, @"..\..\..\..\BloonsLibrary\Maps\MapJsons");
                DirectoryInfo directoryInfo = new DirectoryInfo(mapFilesPath);

                foreach (var file in directoryInfo.GetFiles())
                {
                    // Find the appropriate adapter for the file type
                    var adapter = MapFileAdapters
                        .FirstOrDefault(a => a.SupportsFileType(file.Extension));

                    if (adapter != null)
                    {
                        try
                        {
                            var map = adapter.DeserializeMap(file.FullName);
                            listOfMaps.Add(map);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error deserializing map file {file.Name}: {ex.Message}");
                        }
                    }
                }

                Maps = listOfMaps;
                return listOfMaps;
            }

            return Maps;
        }

        public static Map GetMapByName(string mapName)
        {
            return GetAllMaps().FirstOrDefault(map => map.Name == mapName);
        }
    }
}