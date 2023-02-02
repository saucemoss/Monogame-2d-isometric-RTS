using GameProject.Source.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
namespace GameProject.Source.Main
{
	public class CollisionBox : ICollidable
	{
		public Vector2 position, direction, target;
		public RectangleF drawRectangle;
		public float x, y, w, h, r, speed;
		public bool colliding;
		public Color color;
		public CollisionManager cm;

		RectangleF ICollidable.drawRectangle { get => drawRectangle; }
		float ICollidable.x { get => x; }
		float ICollidable.y { get => y; }
		float ICollidable.w { get => w; }
		float ICollidable.h { get => h; }
		bool ICollidable.colliding { get => colliding; set { colliding = value; } }


		public CollisionBox(float x, float y, float w, float h, bool ignoreOffset = false)
		{
			cm = CollisionManager.GetInstance();
			this.w = w;
			this.h = h;

			position = new Vector2(x, y);
			direction = Vector2.Zero;
			if (ignoreOffset)
			{
				drawRectangle = new RectangleF(x, y, w, h);
				this.x = x;
				this.y = y;
			}
			else
			{
				drawRectangle = new RectangleF(x - w / 2, y - h / 2, w, h);
				this.x = x - w / 2;
				this.y = y - h / 2;
			}
			Random rand = new Random();
			int r = rand.Next(0, 0);
			int g = rand.Next(0, 255);
			int b = rand.Next(0, 255);
			color = new Color(r, g, b);
			cm.Add(this);

		}

		public void Destroy()
		{
			cm.Remove(this);
		}

		public int distanceTo(Vector2 target)
		{
			int xDiff = Math.Abs((int)position.X - (int)target.X);
			int yDiff = Math.Abs((int)position.Y - (int)target.Y);
			return (int)Math.Sqrt(xDiff + yDiff);
		}


		public virtual void Draw(SpriteBatch spritebatch, GameTime gameTime)
		{
			Color c = (colliding) ? Color.Red : color;
			spritebatch.DrawRectangle(drawRectangle, c, 2f);
		}

	}
}
