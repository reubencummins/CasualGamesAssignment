using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasualGamesAssignment.GameObjects
{
    public class Missile : Base.SimpleSprite
    {
        Vector2 Delta;
        float age = 0;


        public float Speed { get; set; }
        public float LifeTime { get; set; }

        public Missile(Texture2D spriteImage, Vector2 startPostition,float angle):base(spriteImage,startPostition)
        {
            LifeTime = 100;
            Speed = 5;
            var rotMatrix = Matrix.CreateRotationZ(angle);
            Delta = new Vector2(1,0);
            Delta = Vector2.Transform(Delta, rotMatrix);
            Rotation = angle;
        }

        public override void Update(GameTime gameTime)
        {

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
            Helper.RemoveObject(this,Helper.Missiles);
        }
    }
}
