using BloonLibrary.Extensions;
using SplashKitSDK;

namespace BloonsProject.Models.Extensions
{
    public class SplashKitExtensions // Linearly interpolates between two points
    {
        private static double Lerp(double firstDouble, double secondDouble, double by)
        {
            return firstDouble * (1 - by) + secondDouble * by;
        }

        public static Point2D Lerp(Point2D firstVector, Point2D secondVector, double by)
        {
            double retX = Lerp(firstVector.X, secondVector.X, by);
            double retY = Lerp(firstVector.Y, secondVector.Y, by);
            return new Point2D() { X = retX, Y = retY };
        }

        public static VectorExtension VectorFromPoint(Point2D point) => new VectorExtension() { X = (float)point.X, Y = (float)point.Y }; // Convert Point2D to a serializable coordinate system.

        public static Point2D PointFromVector(VectorExtension vector) => new Point2D() { X = vector.X, Y = vector.Y };  // Gives point from vector
    }
}