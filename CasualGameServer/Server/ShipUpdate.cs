using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasualGamesAssignment
{
    public class ShipUpdate
    {
        public Vector2 Position;
        public Vector2 Delta;
        public float Rotation;
        public Guid ID;
        public int Health;

        public ShipUpdate(Vector2 position, Vector2 delta, float rotation, Guid id, int health)
        {
            Position = position;
            Delta = delta;
            Rotation = rotation;
            ID = id;
            Health = health;
        }
    }
}
