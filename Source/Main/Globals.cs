using GameProject.Source.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProject
{
    public class Globals
    {
        public static ContentManager content;
        public static SpriteBatch spriteBatch;
        public static int originalSize = 16;
        public static int scale = 3;
        public static int tileSize = originalSize * scale;
        public static DebugInterface debugUI;
        public static int mapWidth;
        public static int mapHeight;
        public static float box2d = 0.01f;


        public static Vector2 ToCartesian(Vector2 isometric)
        {
            Vector2 cartesian;
			cartesian.X = (2 * isometric.Y + isometric.X) / 2;
			cartesian.Y = (2 * isometric.Y - isometric.X) / 2;

			return cartesian;
        }

        public static Vector2 ToIsometric(Vector2 cartesian)
        {
            Vector2 isometric;
            isometric.X = (int)(cartesian.X - cartesian.Y);
            isometric.Y = (int)(cartesian.X + cartesian.Y) / 2;
            return isometric;

        }



    }


}
