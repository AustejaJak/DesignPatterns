using Microsoft.EntityFrameworkCore.Update.Internal;
using SplashKitSDK;
using System;


namespace BloonsProject
{
    public class Cursor
    {
        private Point2D _position;
        public Color Color { get; set; }
        public Cursor(Color color)
        {
            Color = color;
            // Initialize the cursor position with the current mouse position
            _position = SplashKit.MousePosition();
        }

        public void UpdatePosition()
        {
            _position = SplashKit.MousePosition();
        }

        public void Draw()
        {
            int cursorSize = 20;

            double angle = 315 * (Math.PI / 180); // Convert degrees to radians

            // Function to rotate a point around the cursor position
            (float, float) RotatePoint(float x, float y, float centerX, float centerY, double angleRadians)
            {
                float rotatedX = (float)((x - centerX) * Math.Cos(angleRadians) - (y - centerY) * Math.Sin(angleRadians) + centerX);
                float rotatedY = (float)((x - centerX) * Math.Sin(angleRadians) + (y - centerY) * Math.Cos(angleRadians) + centerY);
                return (rotatedX, rotatedY);
            }

            // Original triangle points before rotation
            float x1 = (float)_position.X;
            float y1 = (float)(_position.Y - cursorSize);       // Top point

            float x2 = (float)_position.X - cursorSize / 2;
            float y2 = (float)_position.Y + cursorSize / 2;   // Bottom left point

            float x3 = (float)_position.X + cursorSize / 2;
            float y3 = (float)_position.Y + cursorSize / 2;   // Bottom right point

            // Rotate each point by 45 degrees around the cursor's position (_position)
            (float rotatedX1, float rotatedY1) = RotatePoint(x1, y1, (float)_position.X, (float)_position.Y, angle);
            (float rotatedX2, float rotatedY2) = RotatePoint(x2, y2, (float)_position.X, (float)_position.Y, angle);
            (float rotatedX3, float rotatedY3) = RotatePoint(x3, y3, (float)_position.X, (float)_position.Y, angle);

            // Draw the filled triangle using the rotated points
            SplashKit.FillTriangle(
                Color,
                rotatedX1, rotatedY1,  // Rotated top point
                rotatedX2, rotatedY2,  // Rotated bottom left point
                rotatedX3, rotatedY3   // Rotated bottom right point
            );
        }

    }
}