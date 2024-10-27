using SplashKitSDK;
using System.Collections.Generic;

namespace BloonsCreator
{
    public class GridCalculations
    {
        private static int _numberOfHorizontalGridPoints;
        private static int _numberOfVerticalGridPoints;

        public static List<Line> GetGridHorizontalLines(int tileWidth, int tileHeight)
        {
            _ = GetGridPoints(tileWidth, tileHeight);
            var listOfHorizontalLines = new List<Line>();
            var xCoordinateToAdd = 0;
            var yCoordinateToAdd = 0;

            for (var i = 0; i <= _numberOfVerticalGridPoints; i++)
            {
                listOfHorizontalLines.Add(new Line() { StartPoint = new Point2D() { X = xCoordinateToAdd, Y = yCoordinateToAdd }, EndPoint = new Point2D() { X = xCoordinateToAdd + _numberOfHorizontalGridPoints * tileWidth, Y = yCoordinateToAdd } });
                yCoordinateToAdd += tileHeight;
            }
            return listOfHorizontalLines;
        }

        public static IEnumerable<Point2D> GetGridPoints(int tileWidth, int tileHeight)
        {
            var listOfGridPoints = new List<Point2D>();
            var xCoordinateToAdd = 0;
            var yCoordinateToAdd = 0;

            _numberOfHorizontalGridPoints = 800 / tileWidth;
            _numberOfVerticalGridPoints = 560 / tileHeight;

            for (var i = 0; i < _numberOfVerticalGridPoints; i++)
            {
                for (var j = 0; j < _numberOfHorizontalGridPoints; j++)
                {
                    listOfGridPoints.Add(new Point2D() { X = xCoordinateToAdd, Y = yCoordinateToAdd });
                    xCoordinateToAdd += tileWidth;
                }
                xCoordinateToAdd = 0;
                yCoordinateToAdd += tileHeight;
            }
            return listOfGridPoints;
        }

        public static List<Line> GetGridVerticalLines(int tileWidth, int tileHeight)
        {
            _ = GetGridPoints(tileWidth, tileHeight);
            var listOfVerticalLines = new List<Line>();
            var xCoordinateToAdd = 0;
            var yCoordinateToAdd = 0;

            for (var i = 0; i < _numberOfHorizontalGridPoints; i++)
            {
                listOfVerticalLines.Add(new Line() { StartPoint = new Point2D() { X = xCoordinateToAdd, Y = yCoordinateToAdd }, EndPoint = new Point2D() { X = xCoordinateToAdd, Y = yCoordinateToAdd + _numberOfVerticalGridPoints * tileHeight } });
                xCoordinateToAdd += tileWidth;
            }
            return listOfVerticalLines;
        }
    }
}