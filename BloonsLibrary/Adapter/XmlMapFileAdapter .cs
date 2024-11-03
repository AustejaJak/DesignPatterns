using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using BloonLibrary.Extensions;
using BloonsProject;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace BloonLibrary.Adapter
{
    // XML Map File Adapter
    public class XmlMapFileAdapter : IMapFileAdapter
    {
        public Map DeserializeMap(string filePath)
        {
            XDocument xdoc = XDocument.Load(filePath);
            var root = xdoc.Root;

            // Extract map properties from XML
            var bloonsMap = Path.GetFullPath(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, 
                @"..\..\..\..\BloonsLibrary\", 
                root.Element("BloonsMap").Value
            ));

            var name = root.Element("Name").Value;
            var height = int.Parse(root.Element("Height").Value);
            var length = int.Parse(root.Element("Length").Value);
            var trackWidthRadius = int.Parse(root.Element("TrackWidthRadius").Value);

            // Parse checkpoints
            var checkpoints = root.Elements("Checkpoints")
                .Select(cp => new VectorExtension(
                    int.Parse(cp.Element("X").Value),
                    int.Parse(cp.Element("Y").Value)
                ))
                .ToList();

            // Create and return Map instance
            return new Map(
                bloonsMap,
                length,
                height,
                trackWidthRadius,
                checkpoints,
                name
            );
        }

        public bool SupportsFileType(string fileExtension)
        {
            return fileExtension.Equals(".xml", StringComparison.OrdinalIgnoreCase);
        }
    }
}