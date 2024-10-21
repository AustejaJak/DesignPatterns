using SplashKitSDK;

namespace BloonLibrary
{ 
        public record PlaceBloonRequest(string BloonType, NetworkPoint2D Position);
        
        public record SynchronizeBloon(string BloonType, NetworkPoint2D Position);
        
}