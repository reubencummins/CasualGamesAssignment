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
        List<OpponentShip> opponents;

        Texture2D background;
        Rectangle gameField;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1440;
            graphics.PreferredBackBufferHeight = 900;
            graphics.ToggleFullScreen();
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
            Helper.Initialize(graphics);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            Texture2D playerSprite = Content.Load<Texture2D>("blueship1");
            Texture2D enemySprite1 = Content.Load<Texture2D>("redship1");
            Texture2D enemySprite2 = Content.Load<Texture2D>("blackship1");
            Texture2D enemySprite3 = Content.Load<Texture2D>("orangeship1");
            Texture2D missileSprite = Content.Load<Texture2D>("missile");
            background = Content.Load<Texture2D>("starfield");
            debugFont = Content.Load<SpriteFont>("debug");

            player = new GameObjects.PlayerShip(playerSprite, new Vector2(200, 200))
            {
                Info = new ShipInfo()
                {
                    MaxSpeed = 5f,
                    Acceleration = 0.1f,
                    RotateSpeed = 0.05f,
                    Friction = 0.01f,
                    MaxPower = 0.4f,
                    FireDelay = 500,
                    MissileImage = missileSprite
                }
            };

            opponents = new List<OpponentShip>()
            {
                new OpponentShip(enemySprite1, new Vector2(200, 700))
                {
                    RotateSpeed = 1,
                    Target = player
                },
                new OpponentShip(enemySprite2, new Vector2(500, 700))
                {
                    RotateSpeed = 1,
                    Target = player
                },
                new OpponentShip(enemySprite3, new Vector2(1000, 800))
                {
                    RotateSpeed = 1,
                    Target = player
                }
            };

            spriteBatch = new SpriteBatch(GraphicsDevice);

            gameField = new Rectangle(GraphicsDevice.Viewport.Width/2 - background.Width/2, GraphicsDevice.Viewport.Height/2 - background.Height/2, background.Width, background.Height);

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
            player.Update(gameTime);
            foreach (var op in opponents)
            {
                op.Update(gameTime);
            }
            // TODO: Add your update logic here
            input.Update(gameTime);
            Helper.Update(gameTime);
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
            spriteBatch.Begin(SpriteSortMode.Deferred);
            spriteBatch.Draw(background, gameField, Color.White);
            player.draw(spriteBatch,debugFont); foreach (var op in opponents)
            {
                op.draw(spriteBatch,debugFont);
            }
            Helper.Draw(spriteBatch, debugFont);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
