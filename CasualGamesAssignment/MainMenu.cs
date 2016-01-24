using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTInput;

namespace CasualGamesAssignment
{
    public class MenuItem
    {
        public MainMenu Menu { get; set; }
        public string Text { get; set; }
        public Rectangle Box;
        public Color Color = Color.Black;
        public string Action { get; set; }

        public virtual void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            Vector2 stringOffset = font.MeasureString(Text) / 2;

            spriteBatch.Draw(Menu.BoxBackground, Box, Color.White);
            spriteBatch.DrawString(font, Text, Box.Center.ToVector2() - stringOffset, Color);
        }
    }
    
    public class TextBox : MenuItem
    {
        public string Title { get; set; }
        public int MaxLength { get; set; }
        public bool Hidden { get; set; }
        public bool Edited = false;

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            string hidePass = "";
            for (int i = 0; i < Text.Length; i++)
            {
                hidePass += "*";
            }

            Vector2 stringOffset = font.MeasureString(Text) / 2;

            spriteBatch.Draw(Menu.BoxBackground, Box, Color.White);
            if (!Hidden)
                spriteBatch.DrawString(font, Title + ": " + Text, Box.Center.ToVector2() - stringOffset, Color); 
            else
                spriteBatch.DrawString(font, Title + ": " + hidePass, Box.Center.ToVector2() - stringOffset, Color);
        }
    }

    public class MainMenu
    {
        public List<MenuItem> Items;
        public int Selected = 1;
        public MenuItem SelectedItem;
        public Texture2D BoxBackground;
        public string MenuAction = "";

        public string Username, Password;
        

        bool canSelect = true;

        public MainMenu(Texture2D boxBackground)
        {
            BoxBackground = boxBackground;
            Items = new List<MenuItem>()
            {
                new MenuItem() { Text="Join Game", Action="join"},
                new MenuItem() { Text="Offline Practice", Action="offlinePlay" },
                new MenuItem() { Text="Quit",Action="quit" },
                new TextBox() { Title="Username", Text = "...", MaxLength=15 },
                new TextBox() { Title="Password", Text = "...", MaxLength=15, Hidden=true }
            };
            foreach (var item in Items)
            {
                item.Menu = this;
            }
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

            SelectedItem = Items[Selected];

            if (InputEngine.IsKeyHeld(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                if (SelectedItem is TextBox)
                {
                    TextBox activeBox = SelectedItem as TextBox;
                    if (!activeBox.Edited)
                    {
                        activeBox.Text = "";
                    }
                    activeBox.Edited = true;
                    Helper.Input.CharacterTyped += Input_CharacterTyped;
                }
                else
                {
                    Helper.Input.CharacterTyped -= Input_CharacterTyped;
                    MenuAction = SelectedItem.Action;
                }
            }

            
        }

        private void Input_CharacterTyped(object sender, Microsoft.Xna.Framework.Input.KeyboardCharacterEventArgs e)
        {
            var box = SelectedItem as TextBox;
            
            if (canSelect)
            {
                if (e.Character == '\b')
                {
                    box.Text.Remove(box.Text.Length-1);
                }
                else
                {
                    try
                    {
                        box.Text += e.Character;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                } 
            }
            canSelect = false;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            foreach (MenuItem item in Items)
            {
                item.Draw(spriteBatch, font);
            }
        }
    }
}
