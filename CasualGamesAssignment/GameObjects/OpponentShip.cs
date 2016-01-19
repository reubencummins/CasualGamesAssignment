using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasualGamesAssignment.GameObjects
{
    public class OpponentShip : Base.SimpleSprite
    {
        public Vector2 delta;
        public int health;
        public ShipInfo Info { get; set; }

        public OpponentShip(Texture2D spriteImage, Vector2 startPosition):base(spriteImage,startPosition)
        {
        }

        public void UpdateMe(ShipUpdate update)
        {
            delta = update.Delta;
            Position = update.Position;
            Rotation = update.Rotation;
            health = update.Health;
        }

        public override void Update(GameTime gameTime)
        {
            
            Move(delta);

            base.Update(gameTime);
        }
    }
}

