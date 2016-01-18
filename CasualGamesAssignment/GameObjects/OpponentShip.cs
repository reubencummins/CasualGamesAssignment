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
        private float enginePower;
        public Vector2 delta;
        private Vector2 force;

        public float Acceleration { get; set; }
        public float RotateSpeed { get; set; }
        public float MaxSpeed { get; set; }
        public float MaxPower { get; set; }
        public float Friction { get; set; }
        public PlayerShip Target { get; set; }

        public OpponentShip(Texture2D spriteImage, Vector2 startPosition):base(spriteImage,startPosition)
        {
            enginePower = 0;
            delta = new Vector2(0, -1);
        }

        public override void Update(GameTime gameTime)
        {

            if (InputEngine.CurrentPadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A) || InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                if (enginePower <= MaxPower)
                {
                    enginePower += Acceleration;
                }
            }
            else enginePower = 0;


            delta = (Target.Position - Position);
            delta.Normalize();

            Rotation = (float)(Math.Atan2(delta.Y, delta.X));

            var currentSpeed = delta.Length();
            if (currentSpeed > MaxSpeed)
            {
                currentSpeed = MaxSpeed;
            }
            if (currentSpeed > 0)
            {
                currentSpeed -= Friction;
            }
            else currentSpeed = 0;
            delta *= currentSpeed;

            delta += force;

            //Move(delta);

            base.Update(gameTime);
        }
    }
}

