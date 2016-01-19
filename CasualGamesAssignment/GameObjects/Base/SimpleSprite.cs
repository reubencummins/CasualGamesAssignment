using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasualGamesAssignment.GameObjects.Base
{
    public class SimpleSprite
    {
        public Texture2D Image;
        public Vector2 Position;
        public Rectangle BoundingRect;
        public float Rotation;
        public bool Visible = true;

        public float LayerDepth { get; set; }

        public SimpleSprite(Texture2D spriteImage,
                            Vector2 startPosition)
        {
            Image = spriteImage;
            Position = startPosition;
            BoundingRect = new Rectangle((int)startPosition.X, (int)startPosition.Y, Image.Width, Image.Height);

        }

        public virtual void draw(SpriteBatch sp, SpriteFont font)
        {
            if (Visible)
                sp.Draw(Image,Position,null,null, (Vector2.Zero + new Vector2(Image.Width / 2, Image.Height / 2)), Rotation,Vector2.One,Color.White,SpriteEffects.None,LayerDepth);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Rotation>2*Math.PI)
            {
                Rotation -= 2 * (float)Math.PI;
            }
            if (Rotation > 0)
            {
                Rotation += 2 * (float)Math.PI;
            }
        }

        public void Move(Vector2 delta)
        {
            Position += delta;
            BoundingRect = new Rectangle((int)Position.X, (int)Position.Y, Image.Width, Image.Height);
            BoundingRect.X = (int)Position.X;
            BoundingRect.Y = (int)Position.Y;
        }
    }
}