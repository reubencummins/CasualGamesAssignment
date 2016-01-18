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


        public float Speed { get; set; }

        public Missile(Texture2D spriteImage, Vector2 startPostition,float angle):base(spriteImage,startPostition)
        {
            Speed = 5;
            var rotMatrix = Matrix.CreateRotationZ(angle);
            Delta = new Vector2(0, -1);
            Delta = Vector2.Transform(Delta, rotMatrix);
        }

        public override void Update(GameTime gameTime)
        {
            Delta += new Vector2((float)Math.Sin(age));
            Move(Delta*Speed);
            age ++;
            if (age>=LifeTime)
            {
                Die();
            }


            base.Update(gameTime);
        }

        public void Die()
        {
            Helper.RemoveObject(this);
        }
    }
}
