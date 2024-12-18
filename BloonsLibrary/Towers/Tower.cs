﻿using SplashKitSDK;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using BloonLibrary.VisitorImplementation;

namespace BloonsProject
{
    public abstract class Tower: IUpgradeable
    {
        public static int Length = 35; // Each tower has an identical length of 35
        private readonly string _description; // Each tower has a description and name that is never changed.
        public readonly string _name;

        public string Username {get;  set;}

        private ITowerDecorator _decorator;
        private Bitmap _originalBitmap;

        private Bitmap _towerBitmap;

        public Tower(string name, string username, int cost, string description, Bitmap towerBitmap, IShotType shotType, int range)
        {
            _originalBitmap = towerBitmap;
            Username = username;
            Selected = false;
            Cost = cost;
            Position = SplashKit.MousePosition();
            _description = description;
            _name = name;
            SellPrice = 0.7 * Cost;
            DebugModeSelected = false;
            ShotType = shotType;
            Range = range;
            var targetCreator = new ConcreteTargetFirst();
            Targeting = targetCreator.CreateTarget();
            _decorator = new BaseTowerDecorator(this);
        }

        public Bitmap TowerBitmap 
        { 
            get => GetCurrentBitmap();
        }

        public Bitmap GetCurrentBitmap()
        {
            return _decorator.GetTowerBitmap();
        }

        public Bitmap GetOriginalBitmap()
        {
            return _originalBitmap;
        }

        public void UpdateDecorator()
        {
            Console.WriteLine("updating decorator");
            // Only update appearance for fire rate upgrades
            if (ShotType.FirerateUpgradeCount > 0)
            {
                _decorator = new FireRateDecorator(this);
            }
            else
            {
                _decorator = new BaseTowerDecorator(this);
            }
        }

        public int Cost { get; }
        public bool DebugModeSelected { get; set; } // If selected, will display cooldown above tower.

        public List<string> FullDescription => // Contains information to be displayed in the GUI about the tower.
            new List<string>
            {
                _name,
                _description,
                "Attack Speed " + 200 / ShotType.ShotSpeed,
                "Range " + Range,
                "Cost " + Cost
            };

        public Point2D Position { get; set; } // Tower's position on the map.
        public int Range { get; set; } // Tower's attack range.
        public bool Selected { get; set; } // Whether the user has selected the tower via left click.
        public double SellPrice { get; set; } // The price as which the player can sell the tower for.
        public IShotType ShotType { get; } // The projectile this tower uses.
        public ITarget Targeting { get; set; } // The targeting this tower is set to.
        //public Bitmap TowerBitmap { get; } // The bitmap corresponding to the tower.

        public void ResetTimer() // Resets the tower's cooldown.
        {
            ShotType.TimeSinceLastShot = 0;
        }

        public bool ShotTimer() // Checks to see if the cooldown has expired.
        {
            return ShotType.TimeSinceLastShot > ShotType.ShotSpeed;
        }

        public void ShotTimerTick() // Increments tower's cooldown towards being ready to shoot.
        {
            ShotType.TimeSinceLastShot++;
        }
        
        public void Accept(IUpgradeOptionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}