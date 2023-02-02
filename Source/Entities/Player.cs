using GameProject.Source.Entities.Objects.Weapons;
using GameProject.Source.Inputs;
using GameProject.Source.Main;
using GameProject.Source.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace GameProject.Source.Entities
{
	public class Player : Entity
	{
		AnimationManager animMgr;
		Animation current_animation;
		Vector2 direction;
		float speed = 2f;
		bool attacking = false;
		Weapon weapon;
		CollisionManager colMgr;


		public override Vector2 getDrawPosition => pos;

		public Player(string PATH, Vector2 POS, Vector2 DIMS) : base(POS, DIMS, PATH)
        {
			animMgr = AnimationManager.GetInstance();
            current_animation = animMgr.GetAnimation("player_idle_" + weapon);
            direction = Vector2.Zero;

			colMgr = CollisionManager.GetInstance();


		}

        public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			direction = Vector2.Zero;

			if (InputManager.WasKeyPressed(Keys.E))
			{
				swtichWeapons();
			}
			if (InputManager.IsMousePressed(MouseButton.Left))
			{
				attacking = true;
			}
			else if (!attacking)
			{
				if (InputManager.IsKeyPressed(Keys.W))
				{
					direction.Y = -0.7f;
					current_animation = animMgr.GetAnimation("player_up_" + weapon);
				}
				if (InputManager.IsKeyPressed(Keys.S))
				{
					direction.Y = 0.7f;
					current_animation = animMgr.GetAnimation("player_down_" + weapon);
				}
				if (InputManager.IsKeyPressed(Keys.A))
				{
					direction.X = -1f;
					current_animation = animMgr.GetAnimation("player_left_" + weapon);
				}
				if (InputManager.IsKeyPressed(Keys.D))
				{
					direction.X = 1f;
					current_animation = animMgr.GetAnimation("player_right_" + weapon);
				}
				if (!InputManager.IsKeyPressed(Keys.W) &&
					!InputManager.IsKeyPressed(Keys.A) &&
					!InputManager.IsKeyPressed(Keys.S) &&
					!InputManager.IsKeyPressed(Keys.D))
				{
					direction = Vector2.Zero;
					current_animation = animMgr.GetAnimation("player_idle_" + weapon);
				}
			}


			if (attacking)
			{
				current_animation = animMgr.GetAnimation("player_axe_down_attack");
				if (current_animation.end)
				{
					attacking = false;
				}
			}

			direction = normalizeDirection(direction);


		}

		public override bool OnCollision(Fixture fixture, Fixture fixture1, Contact contact) 
		{
			return true;
		}


        public Vector2 getPos()
        {
            return pos;
        }

        private void getWeapon()
        {
            weapon = Weapons.weapon;
        }
		private void swtichWeapons()
		{
            Weapons.nextWeapon();
            getWeapon();
        }

		public override void Draw(SpriteBatch spriteBatch)
        {
            Color color = (colliding) ? Color.Red : Color.White;
            spriteBatch.Draw(current_animation.sheet, new Microsoft.Xna.Framework.Rectangle(
                                                            (int)pos.X,
                                                            (int)pos.Y,
                                                            (int)(current_animation.frameSize * 3),
                                                            (int)(current_animation.frameSize * 3)),
                                                            current_animation.currentFrame,
                                                            color,
                                                            0.0f,
                                                            new Vector2(current_animation.currentFrame.Width / 2, current_animation.currentFrame.Height / 2),
                                                            new SpriteEffects(),
                                                            0.1f);
            if (DebugInterface.debugViewOn)
            {


            }
        }

	}
}
