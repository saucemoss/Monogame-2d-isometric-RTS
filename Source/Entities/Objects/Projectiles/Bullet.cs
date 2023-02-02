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

namespace GameProject.Source.Entities.Objects.Projectiles
{
	class Bullet : Projectile, IDisposable
	{
		AnimationManager animMgr;
		Animation current_animation;
		double angle;
        float speed = 5;
		Entity target;
        float timeToLive = 0.2f;
        float ttlCounter;
        Vector2 direction;
        public static int damage = 125;
        public Bullet(string PATH, Vector2 POS, Vector2 DIMS, Entity target) : base(POS, DIMS, PATH)
		{
            this.target = target;
            animMgr = AnimationManager.GetInstance();
            current_animation = animMgr.GetAnimation("bullet");

            direction = target.pos - pos;
            direction.Normalize();
            angle = (float)Math.Atan2(direction.Y, direction.X);

            direction.X = (float)(Math.Cos(angle));
            direction.Y = (float)(Math.Sin(angle));
            if (!direction.Equals(Vector2.Zero))
            {
                direction.Normalize();
            }

		}
		public override bool OnCollision(Fixture fixture, Fixture other, Contact contact)
		{
			if (other.Tag != null && !other.Tag.Equals("50cal"))
			{
				Destroy();
			}
			if (other.Tag != null && other.Tag.Equals("enemy"))
			{
				Destroy();
				return true;
			}
			else
			{
				return false;
			}
		}



		public override void Update(GameTime gameTime)
        {
            ttlCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (ttlCounter > timeToLive || !alive)
            {
                Destroy();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Color color = (colliding) ? Color.Red : Color.White;
            spriteBatch.Draw(current_animation.sheet, new Rectangle(
                                                            (int)pos.X,
                                                            (int)pos.Y,
                                                            (int)(current_animation.frameSize * 3),
                                                            (int)(current_animation.frameSize * 3)),
                                                            current_animation.currentFrame,
                                                            Color.White,
                                                            0.0f,
                                                            new Vector2(current_animation.currentFrame.Width / 2, current_animation.currentFrame.Height / 2),
                                                            new SpriteEffects(),
                                                            0.1f);

        }

		public void Dispose()
		{

		}
	}
}
