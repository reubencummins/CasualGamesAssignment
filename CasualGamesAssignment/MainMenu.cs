using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasualGamesAssignment
{
    public class MenuItem
    {
        public string Text { get; set; }
        public Rectangle Box;
        public Color Color = Color.Black;
        public string Action { get; set; }
    }
    

    public class MainMenu
    {
        public List<MenuItem> Items;
        public int Selected = 1;
        Texture2D BoxBackground;
        public string MenuAction = "";

        bool canSelect = true;

        public MainMenu(Texture2D boxBackground)
        {
            BoxBackground = boxBackground;
            Items = new List<MenuItem>()
            {
                new MenuItem() { Text="Join Game", Action="join"},
                new MenuItem() { Text="Quit",Action="quit" }
            };
        }

        public void Update(GameTime gameTime)
        {

            if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                if (canSelect)
                {
                    Selected++;
                    if (Selected >= Items.Count)
                    {
                        Selected = 0;
                    }
                    canSelect = false;
                }
            }
            else if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                if (canSelect)
                {
                    Selected--;
                    if (Selected < 0)
                    {
                        Selected = Selected = Items.Count - 1;
                    }
                    canSelect = false;
                }
            }
            else canSelect = true;

            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Box = new Rectangle(20, 20 + (55 * i), 200, 50);
                if (i==Selected)
                {
                    Items[i].Color = Color.Red;
                }
                else
                {
                    Items[i].Color = Color.Black;
                }
            }

            if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                MenuAction = Items[Selected].Action;
            }

            
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            foreach (MenuItem item in Items)
            {
                Vector2 stringOffset = font.MeasureString(item.Text)/2;

                spriteBatch.Draw(BoxBackground, item.Box, Color.White);
                spriteBatch.DrawString(font, item.Text, item.Box.Center.ToVector2()-stringOffset, item.Color);
                spriteBatch.DrawString(font, MenuAction, new Vector2(200, 200), Color.Red);
            }
        }
    }
}
