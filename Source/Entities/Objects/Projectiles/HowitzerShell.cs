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
    class HowitzerShell : Projectile, IDisposable
    {
        AnimationManager animMgr;
        Animation current_animation;
        double angle;
        float speed = 30;
        Entity target;
        bool exploded;
        float timeToLive = 0.8f;
        float ttlCounter;
        EllipseF elipse;
        Vector2 direction;
        public static int damage = 1000;
        public HowitzerShell(string PATH, Vector2 POS, Vector2 DIMS, Entity target) : base(POS, DIMS, PATH)
        {
            this.target = target;
            animMgr = AnimationManager.GetInstance();
            current_animation = animMgr.GetAnimation("howitzer_bullet_idle");
            elipse = new EllipseF(pos, 69, 50);

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
			if (other.Tag != null && other.Tag.Equals("enemy"))
			{
				colliding = true;

				if (!exploded)
				{
					exploded = true;
					elipse.Center = pos;
					foreach (Entity e in EntityManager.GetInstance().entities)
					{
						if (e.isEnemy && isWithinEllipse(e.pos, elipse))
						{
							e.receiveDamage(damage);
						}
					}
					current_animation = animMgr.PlayOnce("howitzer_bullet_boom");
				}
				if (current_animation.AnimEnded())
				{
					Destroy();
				}
			}
			return false;
		}



		public override void Update(GameTime gameTime)
        {
            ttlCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (ttlCounter > timeToLive)
            {
                if (!exploded)
                {
                    exploded = true;
                    elipse.Center = pos;
                    foreach (Entity e in EntityManager.GetInstance().entities)
                    {
                        if (e.isEnemy && isWithinEllipse(e.pos, elipse))
                        {
                            e.receiveDamage(damage);
                        }
                    }
                    current_animation = animMgr.PlayOnce("howitzer_bullet_boom");
                }
                if (current_animation.AnimEnded())
                {
                    Destroy();
                }
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
                                                            (float)angle-1.57f,
                                                            new Vector2(current_animation.currentFrame.Width / 2, current_animation.currentFrame.Height / 2),
                                                            new SpriteEffects(),
                                                            0.1f);

        }

        public void Dispose()
        {

        }
    }
}
