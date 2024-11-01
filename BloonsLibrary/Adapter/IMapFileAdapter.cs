using BloonsProject;

namespace BloonLibrary
{
    // Interface for map file adapters
    public interface IMapFileAdapter
    {
        Map DeserializeMap(string filePath);
        bool SupportsFileType(string fileExtension);
    }
}