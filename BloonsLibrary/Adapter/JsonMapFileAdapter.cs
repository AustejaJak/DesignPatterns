using System.Xml;
using System;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using BloonsProject;

namespace BloonLibrary
{
    // JSON Map File Adapter
    public class JsonMapFileAdapter : IMapFileAdapter
    {
        public Map DeserializeMap(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            var map = JsonSerializer.Deserialize<Map>(jsonString);

            // Adjust the path to be a full path
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            map.BloonsMap = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\..\BloonsLibrary\", map.BloonsMap));

            return map;
        }

        public bool SupportsFileType(string fileExtension)
        {
            return fileExtension.Equals(".json", StringComparison.OrdinalIgnoreCase);
        }
    }
}