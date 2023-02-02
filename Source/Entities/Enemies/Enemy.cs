using GameProject.Source.Entities.Buildings;
using GameProject.Source.Entities.Objects.Projectiles;
using GameProject.Source.Main;
using GameProject.Source.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using static GameProject.Source.Tiles.IsometricTilemap;

namespace GameProject.Source.Entities.Enemies
{
	abstract class Enemy : Entity
	{
		public Animation current_animation;
		//bool flying, armored, light, heavy, ground;
		
		public bool atTarget;
		public EllipseF range;
		float timer = 0;
		public Enemy(Vector2 POS, Vector2 DIMS, string PATH = "placeholder") : base(POS, DIMS, PATH)
		{
			isEnemy = true;
			current_animation = AnimationManager.GetInstance().GetAnimation("big_zombie_down");
			range = new EllipseF(pos, 140, 100);

		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (timer > 1f)
			{
				atTarget = false;
				timer = 0;
			}
			//target = new Vector2(500, 500);
			foreach (Tile t in GameSpace.GetInstance().isometricTilemap.tilesArray)
			{
				if (t.isWall && isWithinEllipse(t.isoPos * 3, range))
				{
					target = t.isoPos * 3;
				}
			}
		}

		public override bool OnCollision(Fixture fixture, Fixture other, Contact contact)
		{
			switch (other.Tag)
			{
				case null:
					
					break;
				case "50cal":
					receiveDamage(Bullet.damage);
					break;
				case "flame":
					receiveDamage(Flame.damage);
					return false;
				case "shell":
					//receiveDamage(HowitzerShell.damage);
					break;
				case "wall":
					atTarget = true;
					break;
				case "enemy":

					break;
			}
			
			colliding = true;
			return false;
		}
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			if (HP < maxHP)
			{
				int dim = current_animation.frameSize * 3 / 2;
				float hpBarValue = (float)HP / (float)maxHP * dim;
				Globals.spriteBatch.DrawRectangle(new RectangleF(pos.X - dim / 2, pos.Y - dim - 4, dim, 4), Color.Red);
				Globals.spriteBatch.FillRectangle(new RectangleF(pos.X - dim / 2, pos.Y - dim - 4, hpBarValue, 4), Color.Red);
			}

			if (UI.DebugInterface.debugViewOn)
			{
				Globals.spriteBatch.DrawRectangle(new RectangleF(pos.X - dims.X / 2, pos.Y - dims.Y / 2, dims.X, dims.Y), Color.Red);
			}
		}
	}


}
