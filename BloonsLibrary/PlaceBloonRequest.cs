namespace BloonLibrary
{
    public class PlaceBloonsRequest
    {
        public record PlaceBloonRequest(string TowerType, NetworkPoint2D Position, string Username);

        public record SynchronizeBloon(string TowerType, NetworkPoint2D Position, string PlayerName);
    }
}