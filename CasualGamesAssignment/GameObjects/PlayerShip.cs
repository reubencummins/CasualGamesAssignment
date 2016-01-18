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
        private bool canFire;
        private float fireTimer;

        private Vector2 normal;

        public ShipInfo Info { get; set; }
        

        public PlayerShip(Texture2D spriteImage,Vector2 startPosition):base(spriteImage,startPosition)
        {
            enginePower = 0;
            delta = new Vector2(1,0);
            canFire = true;
            normal = delta;
        }

        public override void Update(GameTime gameTime)
        {
            Position = Helper.ScreenWrap(Position);

            if (InputEngine.CurrentKeyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space) || InputEngine.CurrentPadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.X))
            {
                if (canFire)
                {
                    Helper.AddObject(new Missile(Info.MissileImage, Position+(normal*Image.Width/2), Rotation) { Speed=10f,LayerDepth=0f },Helper.Missiles);
                    canFire = false;
                    fireTimer =0;
                }
            }

            fireTimer += gameTime.ElapsedGameTime.Milliseconds;
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
            normal = Vector2.Transform(new Vector2(1,0), rotate);

            delta += normal*enginePower;
            //clamp 
            var currentSpeed = delta.Length();
            if (currentSpeed > Info.MaxSpeed)
            {
                currentSpeed = Info.MaxSpeed;
            }
            if (currentSpeed > 0)
            {
                currentSpeed -= Info.Friction;
            }
            else currentSpeed -= Math.Abs(currentSpeed);
            delta.Normalize();

            delta *= currentSpeed;


            Move(delta);
            //base.Update(gameTime);
        }

        public override void draw(SpriteBatch sp, SpriteFont font)
        {
            sp.DrawString(font, "Player Position: " + Position.ToString(), Helper.NextLine(), Color.White);
            sp.DrawString(font, "Player Rotation: " + (Rotation / Math.PI * 180), Helper.NextLine(), Color.White);
            base.draw(sp, font);
        }
    }
}
