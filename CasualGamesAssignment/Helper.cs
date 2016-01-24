using CasualGamesAssignment.GameObjects;
using CasualGamesAssignment.GameObjects.Base;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasualGamesAssignment
{
    public static class Helper
    {
        private static Vector2 viewportSize;
        public static List<OpponentShip> Opponents;
        public static List<SimpleSprite> Missiles;
        public static List<SimpleSprite> Particles;

        public static PlayerShip Player;
        public static int Score;

        public static Texture2D ParticleImage { get; set; }

        public static Game1 Game;
        public static CTInput.Input Input;
        public enum GAMESTATE { STARTING, WAITING, PLAYING, ENDING }

        public static GAMESTATE state;

        private static Vector2 line = new Vector2(10, -10);

        static IHubProxy Proxy;

        public static Vector2 NextLine()
        {
            line.Y += 20;
            return line;
        }

        public static void Initialize(GraphicsDeviceManager graphics, IHubProxy proxy, CTInput.Input input)
        {
            viewportSize = new Vector2(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            Missiles = new List<SimpleSprite>();
            Particles = new List<SimpleSprite>();
            Opponents = new List<OpponentShip>();
            Proxy=proxy;
            Input = input;
        }

        public static void Update(GameTime gameTime)
        {
            line = new Vector2(10, 10);
            for (int i = 0; i < Missiles.Count; i++)
            {
                Missiles[i].Update(gameTime);
                try
                {
                    if (Math.Abs((Missiles[i].Position - Player.Position).Length()) < 50)
                    {
                        Missile mis = Missiles[i] as Missile;
                        mis.Die();
                        DamagePlayer(1);
                    }

                    foreach (SimpleSprite ship in Opponents)
                    {
                        if (Math.Abs((Missiles[i].Position - ship.Position).Length()) < 50)
                        {
                            Missile mis = Missiles[i] as Missile;
                            mis.Die();
                            DamageShip(ship, 1);
                            Score++;
                        }
                    }
                }
                catch (Exception)
                { }
            }
            for (int i = 0; i < Particles.Count; i++)
            {
                Particles[i].Update(gameTime);
            }
            Input.Update(gameTime);
        }

        private static void DamagePlayer(int amount)
        {
            Player.Health -= amount;
        }

        private static void DamageShip(SimpleSprite opponent, int amount)
        {
            if (opponent is OpponentShip)
            {
                OpponentShip opp = opponent as OpponentShip;
                opp.health-=amount; 
            }
        }

        public static Vector2 ScreenWrap(Vector2 position)
        {
            if (position.X<-15)
            {
                position.X = viewportSize.X + 10;
            }
            if (position.X> viewportSize.X + 15)
            {
                position.X = -10;
            }
            if (position.Y < -15)
            {
                position.Y = viewportSize.Y + 10;
            }
            if (position.Y > viewportSize.Y + 15)
            {
                position.Y = -10;
            }
            return position;
        }

        public static void AddObject(SimpleSprite newObject, List<SimpleSprite> list)
        {
            list.Add(newObject);
        }

        public static void RemoveObject(SimpleSprite oldObject, List<SimpleSprite> list)
        {
            if (list.Contains(oldObject))
            {
                list.Remove(oldObject);
            }
        }

        public static void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {

            foreach (var m in Missiles)
            {
                m.draw(spriteBatch, font);
            }
            foreach (var p in Particles)
            {
                p.draw(spriteBatch, font);
            }
        }

        public static void AddParticle(Vector2 position, Vector2 delta, Particle.ParticleType type)
        {
            AddObject(new Particle(ParticleImage, position, delta) { Type = type }, Particles);
        }

        public static void UpdateMe(ShipUpdate ship)
        {
            Proxy.Invoke("updateShip", ship);
        }


        public static void SubmitScore()
        {
            GameScoreObject finalScore = new GameScoreObject() { PlayerID = Player.Info.ID.ToString(), Score = Score };
        }
    }
}