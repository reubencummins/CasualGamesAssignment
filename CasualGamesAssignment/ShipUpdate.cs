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

        public ShipUpdate(Vector2 position, Vector2 delta, float rotation)
        {
            Position = position;
            Delta = delta;
            Rotation = rotation;
        }
    }
}
