using CasualGamesAssignment.GameObjects;
using CasualGamesAssignment.GameObjects.Base;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using static CasualGamesAssignment.Helper;

namespace CasualGamesAssignment
{
    public class Game1 : Game
    {
        private static object gameLock = new object();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont debugFont;
        InputEngine input;

        CTInput.Input _eventDrivenInput;

        PlayerShip player;
        List<OpponentShip> opponents;
        List<AutoShip> autos;

        Dictionary<string, Texture2D> sprites;
        

        HubConnection Connection;
        IHubProxy proxy;

        MainMenu menu;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //graphics.PreferredBackBufferWidth = 1366;
            //graphics.PreferredBackBufferHeight = 768;
            //graphics.ToggleFullScreen();


            Connection = new HubConnection("http://localhost:50416/");
            proxy = Connection.CreateHubProxy("GameHub");

            Action<List<ShipInfo>> play = Play;
            proxy.On("play", play);
            
            Action join = Join;
            proxy.On("join", join);

            Action<ShipUpdate> updateOpponent = UpdateOpponent;
            proxy.On("updateOpponent", updateOpponent);

            Action<ShipInfo> confirmJoin = ConfirmJoin;
            proxy.On("confirmJoin", confirmJoin);

            state = GAMESTATE.STARTING;
        }

        private void Join()
        {
            proxy.Invoke("join");
        }

        private void ConfirmJoin(ShipInfo playerShip)
        {
            player = new PlayerShip(sprites[playerShip.ShipImage], playerShip.StartPosition, playerShip);
            state = GAMESTATE.WAITING;
        }

        protected override void Initialize()
        {
            _eventDrivenInput = new CTInput.MonoGameInput(this);
            input = new InputEngine(this);
            Helper.Initialize(graphics,proxy,_eventDrivenInput);
            Helper.Player = player;
            base.Initialize();
            Connection.Start();
        }
        
        protected override void LoadContent()
        {
            sprites = new Dictionary<string, Texture2D>();
            sprites.Add("ship0", Content.Load<Texture2D>("blueship1"));
            sprites.Add("ship1", Content.Load<Texture2D>("redship1"));
            sprites.Add("ship2", Content.Load<Texture2D>("blackship1"));
            sprites.Add("ship3", Content.Load<Texture2D>("orangeship1"));
            sprites.Add("background", Content.Load<Texture2D>("starfield"));
            sprites.Add("missile", Content.Load<Texture2D>("missile"));
            sprites.Add("noImage", Content.Load<Texture2D>("pixel"));
            debugFont = Content.Load<SpriteFont>("debugFont");

            menu = new MainMenu(sprites["noImage"]);

            Helper.Opponents = opponents;

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        
        protected override void UnloadContent()
        {
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                state = GAMESTATE.STARTING;
                Connection.Stop();
            }

            if (state==GAMESTATE.PLAYING)       
            {
                //game
                player.Update(gameTime);
                foreach (var op in opponents)
                {
                    op.Update(gameTime);
                }
                foreach (var auto in autos)
                {
                    auto.Update(gameTime);
                }
            }

            if (state==GAMESTATE.STARTING)
            {
                //menu
                menu.Update(gameTime);
                switch (menu.MenuAction)
                {
                    case "quit":
                        Exit();
                        break;
                    case "join":
                        Console.WriteLine("");
                        if (Connection.State == ConnectionState.Connected)
                        {
                            PlayerAuthentication.Login(menu.Username, menu.Password);

                        }
                        else Connection.Start();
                        break;

                    case "offlinePlay":
                        player = new PlayerShip(sprites["ship0"], new Vector2(300, 600), new ShipInfo("", "ship0", new Vector2(300, 600)) { MissileImage = sprites["missile"] });
                        autos = new List<AutoShip>()
                        {
                            new AutoShip(sprites["ship1"],new Vector2(800,500)) { Target=player },
                            new AutoShip(sprites["ship2"],new Vector2(1000,200)) { Target=player },
                            new AutoShip(sprites["ship3"],new Vector2(200,600)) { Target=player }
                        };
                        state = GAMESTATE.PLAYING;
                        break;
                    default:
                        break;
                }

                if (PlayerAuthentication.PlayerStatus==AUTHSTATUS.OK)
                {
                    proxy.Invoke("join");
                }
            }

            if (state==GAMESTATE.WAITING)
            {
                //wait screen
            }

            Helper.Update(gameTime);
            input.Update(gameTime);
            base.Update(gameTime); 
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            spriteBatch.Begin(SpriteSortMode.Deferred);
            if (state==GAMESTATE.PLAYING)
            {
                spriteBatch.Draw(sprites["background"], new Rectangle(0,0,GraphicsDevice.Viewport.Width,GraphicsDevice.Viewport.Height), Color.White);
                player.draw(spriteBatch, debugFont);
                foreach (var op in opponents)
                {
                    op.draw(spriteBatch, debugFont);
                }
                foreach (var auto in autos)
                {
                    auto.draw(spriteBatch, debugFont);
                }
                Helper.Draw(spriteBatch, debugFont);
            }

            if (state==GAMESTATE.STARTING)
            {
                menu.Draw(spriteBatch, debugFont);
            }

            if (state==GAMESTATE.WAITING)
            {
                Vector2 stringOffset = debugFont.MeasureString("Loading...");
                spriteBatch.DrawString(debugFont, "Loading...", new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height) / 2 - stringOffset,Color.LimeGreen);
            }
            spriteBatch.DrawString(debugFont, Connection.State.ToString(), Helper.NextLine(), Color.Yellow);
            spriteBatch.End();
            base.Draw(gameTime);
        }


        private void UpdateOpponent(ShipUpdate opponent)
        {
            OpponentShip opp = Helper.Opponents.FirstOrDefault(op => op.Info.ID == opponent.ID);
            lock (gameLock)
            {
                opp.UpdateMe(opponent);
            }
        }

        private void Play(List<ShipInfo> Opponents)
        {
            foreach (var item in Opponents)
            {
                opponents.Add(new OpponentShip(sprites[item.ShipImage], item.StartPosition));
            }
            state = GAMESTATE.PLAYING;
        }

    }
}