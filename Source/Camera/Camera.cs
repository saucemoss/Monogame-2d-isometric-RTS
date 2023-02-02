
using GameProject.Source.Entities;
using GameProject.Source.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProject.Source.Main
{
	public class Camera
	{

		public static Matrix Transform { get; private set; }

		public static Matrix InverseTransform()
		{
			return Matrix.Invert(Transform);
		}

		public void Follow(Entity target) 
		{
			var targetPos = Matrix.CreateTranslation(
				-target.pos.X - (target.img.Bounds.Width / 2),
				-target.pos.Y - (target.img.Bounds.Height / 2),
				0); 
			var screenOffset = Matrix.CreateTranslation(
				Game1.screenWidth/2,
				Game1.screenHeight/2,
				0);
			Transform = targetPos * screenOffset;

		}

		public void Follow(View target)
		{
			var targetPos = Matrix.CreateTranslation(
				-target.pos.X,
				-target.pos.Y,
				0);
			var screenOffset = Matrix.CreateTranslation(
				Game1.screenWidth / 2,
				Game1.screenHeight / 2,
				0);
			Transform = targetPos * screenOffset;

		}

	}


	public class View
	{
		public Vector2 pos;
		Vector2 direction;
		float speed = 5;

		public View(Vector2 pos)
		{
			this.pos = pos;
		}

		public void Update(GameTime gameTime)
		{
			direction = Vector2.Zero;
			if (InputManager.IsKeyPressed(Keys.W))
			{
				direction.Y = -1f;
			}
			if (InputManager.IsKeyPressed(Keys.S))
			{
				direction.Y = 1f;
			}
			if (InputManager.IsKeyPressed(Keys.A))
			{
				direction.X = -1f;
			}
			if (InputManager.IsKeyPressed(Keys.D))
			{
				direction.X = 1f;
			}
			if (!direction.Equals(Vector2.Zero))
			{
				direction.Normalize();
			}
			
			pos = pos + direction * speed;

		}
	}
}
