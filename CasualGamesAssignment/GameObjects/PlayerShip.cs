using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasualGamesAssignment.GameObjects
{
    class PlayerShip : Base.SimpleSprite
    {
        private float enginePower;
        public Vector2 delta;
        private Vector2 force;
        private bool canFire;
        private float fireTimer;

        public ShipInfo Info { get; set; }
        

        public PlayerShip(Texture2D spriteImage,Vector2 startPosition):base(spriteImage,startPosition)
        {
            enginePower = 0;
            delta = new Vector2(0,-1);
            canFire = true;
        }

        public override void Update(GameTime gameTime)
        {
            Position = Helper.ScreenWrap(Position);

            if (InputEngine.CurrentKeyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space) || InputEngine.CurrentPadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.X))
            {
                if (canFire)
                {
                    Helper.AddObject(new Missile(Info.MissileImage, Position, Rotation) { Speed=0.1f,LayerDepth=0f });
                    canFire = false;
                    fireTimer =0;
                }
                else fireTimer += gameTime.ElapsedGameTime.Milliseconds;
            }

            if (fireTimer >= Info.FireDelay)
                canFire = true;


            Rotation += InputEngine.CurrentPadState.ThumbSticks.Left.X * Info.RotateSpeed;

            if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Left))
                Rotation -= Info.RotateSpeed;
            if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Right))
                Rotation += Info.RotateSpeed;

            if (InputEngine.CurrentPadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A) || InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                if (enginePower <= Info.MaxPower)
                {
                    enginePower += Info.Acceleration;
                }
            }
            else enginePower = 0;
            

            var rotate = Matrix.CreateRotationZ(Rotation);
            force = Vector2.Transform(new Vector2(0,-1), rotate);
            force *= enginePower;

            var currentSpeed = delta.Length();
            if (currentSpeed > Info.MaxSpeed)
            {
                currentSpeed = Info.MaxSpeed;
            }
            if (currentSpeed > 0)
            {
                currentSpeed -= Info.Friction;
            }
            else currentSpeed = 0;
            delta.Normalize();
            delta *= currentSpeed;

            delta += force;

            Move(delta);
            //base.Update(gameTime);
        }

        public override void draw(SpriteBatch sp, SpriteFont font)
        {
            sp.DrawString(font, "Player Position: " + Position.ToString(), Helper.NextLine(), Color.White);
            sp.DrawString(font, "Player Rotation: " + Rotation.ToString(), Helper.NextLine(), Color.White);
            base.draw(sp, font);
        }
    }
}
