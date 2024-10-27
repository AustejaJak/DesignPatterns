using SplashKitSDK;

namespace BloonLibrary
{ 
        public record PlaceBloonRequest(int Health, string Name, Color Color, float VelocityX, float VelocityY);

        public record SynchronizeBloon(int Health, string Name, Color Color, float VelocityX, float VelocityY);

}