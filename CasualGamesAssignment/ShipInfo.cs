using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasualGamesAssignment
{
    public class ShipInfo
    {
        public Guid ID;
        public float Acceleration { get; set; }
        public float RotateSpeed { get; set; }
        public float MaxSpeed { get; set; }
        public float MaxPower { get; set; }
        public float Friction { get; set; }
        public Texture2D MissileImage { get; set; }
        public float FireDelay { get; set; }

        public int MaxHealth { get; set; }

        public ShipInfo()
        {
            MaxSpeed = 5f;
            Acceleration = 0.1f;
            RotateSpeed = 0.05f;
            Friction = 0.01f;
            MaxPower = 0.4f;
            FireDelay = 500;
            MaxHealth = 5;
            ID = new Guid();
        }
    }
}