using SplashKitSDK;

namespace BloonLibrary
{ 
        public record PlaceBloonRequest(int Health, string Name, Color Color, float VelocityX, float VelocityY);

        public record SynchronizeBloon(int Health, string Name, Color Color, float VelocityX, float VelocityY);

        public record BloonState(string Name, int Health, NetworkPoint2D Position, int Checkpoint, double DistanceTravelled);
        public record BloonStateRequest(string Name, int Health, NetworkPoint2D Position, int Checkpoint, double DistanceTravelled);
}