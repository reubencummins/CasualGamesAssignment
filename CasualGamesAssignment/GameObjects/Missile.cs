using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasualGamesAssignment.GameObjects
{
    class Missile : Base.SimpleSprite
    {
        Vector2 Delta;
        float LifeTime = 200;
        float age = 0;
        public Missile(Texture2D spriteImage, Vector2 startPostition,Vector2 delta):base(spriteImage,startPostition)
        {
            Delta = delta;
        }

        public override void Update(GameTime gameTime)
        {
            Move(Delta);
            age += gameTime.ElapsedGameTime.Milliseconds;
            if (age>=LifeTime)
            {

            }
            base.Update(gameTime);
        }
    }
}
