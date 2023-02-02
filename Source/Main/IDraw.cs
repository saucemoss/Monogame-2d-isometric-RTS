using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProject.Source.Main
{
	interface IDraw
	{
		Vector2 getDrawPosition { get; }

		public void Draw(SpriteBatch batch);
	}
}
