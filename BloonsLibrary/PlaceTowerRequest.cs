﻿using SplashKitSDK;

namespace BloonLibrary
{
    public record PlaceTowerRequest(string TowerType, NetworkPoint2D Position, string Username);

    public record SynchronizeTower(string TowerType, NetworkPoint2D Position, string PlayerName);

    public record NetworkPoint2D(double X, double Y)
    {
        public static NetworkPoint2D Serialize(Point2D point) => new NetworkPoint2D(point.X, point.Y);
        public static Point2D Deserialize(NetworkPoint2D point) => new Point2D 
        { 
            X = (float)point.X, 
            Y = (float)point.Y 
        };
    }
    
}