using CasualGamesAssignment.GameObjects.Base;
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
        private static List<SimpleSprite> objectPool;

        public static void Initialize(GraphicsDeviceManager graphics, List<SimpleSprite> ObjectPool)
        {
            viewportSize = new Vector2(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            objectPool = ObjectPool;
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

        public static void AddObject(SimpleSprite newObject)
        {
            objectPool.Add(newObject);
        }

        public static void RemoveObject(SimpleSprite oldObject)
        {
            if (objectPool.Contains(oldObject))
            {
                objectPool.Remove(oldObject);
            }
        }
    }
}
