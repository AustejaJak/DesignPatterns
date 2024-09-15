using BloonsProject.Models.Extensions;
using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BloonsProject
{
    public class ProjectileManager
    {
        public ProjectileManager()
        {
            ProjectilesOnScreen = new List<Projectile>();
        }

        public List<Projectile> ProjectilesOnScreen { get; set; }

        public void AddProjectile(Point2D initialPosition, Point2D projectileDestination, IShotType shotType) // Add a projectile by passing in it's location, endpoint and shot type to the list of projectiles.
        {
            var projectile = new Projectile(initialPosition, projectileDestination, shotType);
            ProjectilesOnScreen.Add(projectile);
        }

        public Point2D GetProjectileEndPoint(Bloon bloon, Tower tower)
        {
            var towerToBloonAngle =
                        Math.Atan((bloon.Position.Y - tower.Position.Y) / (bloon.Position.X - tower.Position.X)) + Math.PI; // Calculates angle between tower and bloon.

            if (bloon.Position.X > tower.Position.X) // If the bloon is either to the right or left of the tower, perform a different calculation for the angle.
            {
                towerToBloonAngle = Math.Atan((bloon.Position.Y - tower.Position.Y) /
                                              (bloon.Position.X - tower.Position.X));
            }
            ; var projectileEndPoint = new Point2D() // Extrapolate the distance from the tower to the bloon, to the end of the towers radius in the same angle.
            {
                X = tower.Position.X + (tower.Range * Math.Cos(towerToBloonAngle)),
                Y = tower.Position.Y + (tower.Range * Math.Sin(towerToBloonAngle))
            };
            projectileEndPoint.X -= tower.ShotType.ProjectileWidth / 2; // Bitmap draws from the top-left of the image. Re-configure bitmap so that the origin of the bitmap is where it is drawn and calculated from.
            projectileEndPoint.Y -= tower.ShotType.ProjectileLength / 2;
            return projectileEndPoint;
        }

        public void IncrementAllProjectiles()
        {
            foreach (Projectile projectile in ProjectilesOnScreen.ToList())
            {
                if (projectile.ProjectileStationaryTimeSpent == 0) // If the projectile isn't stationary (hasn't reached endpoint) keep 'lerping' it.
                {
                    projectile.ProjectileLocation = SplashKitExtensions.Lerp(projectile.ProjectileLocation, projectile.ProjectileDestination, projectile.ProjectileShotType.ProjectileSpeed);
                }

                if (!(Math.Abs(projectile.ProjectileLocation.X - projectile.ProjectileDestination.X) < 5) ||
                    !(Math.Abs(projectile.ProjectileLocation.Y - projectile.ProjectileDestination.Y) < 5)) continue; //If it hasn't reached the endpoint, stop here and continue.
                projectile.ProjectileStationaryTimeSpent++; // If it has, then increment the stationary time spent.
                if (projectile.ProjectileStationaryTimeSpent <
                    projectile.ProjectileShotType.ProjectileStationaryTime) continue; // If it hasn't spent enough time stationary in accordance to the required time, then continue.

                ProjectilesOnScreen.Remove(projectile); // Otherwise remove the projectile.
            }
        }
    }
}