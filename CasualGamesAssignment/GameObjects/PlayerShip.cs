using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasualGamesAssignment.GameObjects
{
    public class PlayerShip : Base.SimpleSprite
    {
        private float enginePower;
        public Vector2 delta;
        private bool canFire;
        private float fireTimer;

        private Vector2 normal;

        public int Health;
        public ShipInfo Info { get; set; }
        

        public PlayerShip(Texture2D spriteImage,Vector2 startPosition, ShipInfo info):base(spriteImage,startPosition)
        {
            enginePower = 0;
            delta = new Vector2(1,0);
            canFire = false;
            normal = delta;
            Info = info;
            Health = Info.MaxHealth;
        }

        public override void Update(GameTime gameTime)
        {
            bool sendUpdate = false;


            //wrap around screen
            Position = Helper.ScreenWrap(Position);


            //fire weapons
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


            //rotation
            Rotation += InputEngine.CurrentPadState.ThumbSticks.Left.X * Info.RotateSpeed;


            if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                Rotation -= Info.RotateSpeed;
                sendUpdate = true;
            }
            if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                Rotation += Info.RotateSpeed;
                sendUpdate = true;
            }

            //acceleration
            if (InputEngine.CurrentPadState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A) || InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                if (enginePower <= Info.MaxPower)
                {
                    enginePower += Info.Acceleration;
                }
                sendUpdate = true;
            }
            else enginePower = 0;
            
            //get force to apply from engine power and rotation
            var rotate = Matrix.CreateRotationZ(Rotation);
            normal = Vector2.Transform(new Vector2(1,0), rotate);
            delta += normal*enginePower;

            //clamp delta to max speed
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

            //if (sendUpdate)
            //{
            //    Helper.UpdateMe(new ShipUpdate
            //        (
            //        Position,
            //        delta,
            //        Rotation,
            //        Info.ID,
            //        Health));
            //}

            if (Health<=0)
            {
                Die();
            }
            base.Update(gameTime);
        }

        private void Die()
        {
            Helper.SubmitScore();
        }

        public override void draw(SpriteBatch sp, SpriteFont font)
        {
            //sp.DrawString(
            //    font,
            //    "Player Position: " + Position.ToString(),
            //    Helper.NextLine(),
            //    Color.White
            //    );
            //sp.DrawString(
            //    font,
            //    "Player Rotation: " + (Rotation / Math.PI * 180),
            //    Helper.NextLine(),
            //    Color.White);

            for (int i = 0; i < Health; i++)
            {
                sp.Draw(
                    new Texture2D(sp.GraphicsDevice, 1, 1),
                    new Rectangle(
                        20+i*21,
                        sp.GraphicsDevice.Viewport.Height - 20,
                        20,
                        20),
                    Color.Green
                    );
            }
            base.draw(sp, font);
        }
    }
}
