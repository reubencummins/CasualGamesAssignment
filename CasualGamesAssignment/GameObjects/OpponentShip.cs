using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasualGamesAssignment.GameObjects
{
    class OpponentShip : Base.SimpleSprite
    {
        public Vector2 delta;

        public ShipInfo Info { get; set; }

        public OpponentShip(Texture2D spriteImage, Vector2 startPosition):base(spriteImage,startPosition)
        {
        }

        public override void Update(GameTime gameTime)
        {
            
            Move(delta);

            base.Update(gameTime);
        }
    }
}

