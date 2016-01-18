using CasualGamesAssignment.GameObjects;
using CasualGamesAssignment.GameObjects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace CasualGamesAssignment
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont debugFont;
        InputEngine input;

        PlayerShip player;
        OpponentShip opponent;

        List<GameObjects.Base.SimpleSprite> Missiles;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Missiles = new List<GameObjects.Base.SimpleSprite>();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            input = new InputEngine(this);
            Helper.Initialize(graphics,Missiles);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            Texture2D playerSprite = Content.Load<Texture2D>("arrow");
            Texture2D missileSprite = Content.Load<Texture2D>("missile");
            debugFont = Content.Load<SpriteFont>("debug");

            player = new GameObjects.PlayerShip(playerSprite, new Vector2(200, 200))
            {
                MaxSpeed = 5f,
                Acceleration = 0.1f,
                RotateSpeed = 0.05f,
                Friction = 0.01f,
                MaxPower = 0.4f,
                FireDelay = 500,
                MissileImage = missileSprite,
                LayerDepth = 0.1f
            };

            opponent = new OpponentShip(playerSprite, new Vector2(500, 200))
            {
                RotateSpeed = 1,
                Target = player
            };
            
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            for (int i = 0; i<Missiles.Count; i++)
            {
                Missiles[i].Update(gameTime);
            }
            player.Update(gameTime);
            opponent.Update(gameTime);
            // TODO: Add your update logic here
            input.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            player.draw(spriteBatch,debugFont);
            opponent.draw(spriteBatch, debugFont);
            foreach (var m in Missiles)
            {
                m.draw(spriteBatch,debugFont);
            }
            spriteBatch.DrawString(debugFont, InputEngine.CurrentPadState.ThumbSticks.Left.X.ToString(), new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(debugFont, player.delta.ToString(), new Vector2(10, 30), Color.Green);
            spriteBatch.DrawString(debugFont, Missiles.Count.ToString(), new Vector2(10, 50), Color.Black);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
