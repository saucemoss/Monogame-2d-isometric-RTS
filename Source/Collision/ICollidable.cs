using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProject.Source.Main
{
	public interface ICollidable
	{
		public float x { get; }
		public float w { get; }
		public float y { get; }
		public float h { get; }
		public RectangleF drawRectangle { get; }
		bool colliding { get; set; }

		void Draw(SpriteBatch spritebatch, GameTime gameTime);
	}
}
