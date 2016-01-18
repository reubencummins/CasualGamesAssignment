using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasualGamesAssignment.GameObjects
{
    public class Particle : Base.SimpleSprite
    {
        public enum ParticleType
        {
            Grow, Shrink
        }

        Vector2 Delta;
        float age = 0;
        public ParticleType Type { get; set; }
        public float LifeTime {get;set;}

        public Particle(Texture2D spriteImage,Vector2 startPosition, Vector2 delta):base(spriteImage,startPosition)
        {
            Delta = delta;
            LifeTime = 200;
        }

        public override void Update(GameTime gameTime)
        {
            switch (Type)
            {
                case ParticleType.Grow:
                    BoundingRect.Width++;
                    BoundingRect.Height++;
                    break;
                case ParticleType.Shrink:
                    BoundingRect.Width--;
                    BoundingRect.Height--;
                    break;
                default:
                    break;
            }
            age++;
            if (age>=LifeTime)
            {
                Die();
            }
            base.Update(gameTime);
        }

        private void Die()
        {
            if (Helper.Particles.Contains(this))
            {
                Helper.RemoveObject(this, Helper.Particles);
            }
        }
    }
}
