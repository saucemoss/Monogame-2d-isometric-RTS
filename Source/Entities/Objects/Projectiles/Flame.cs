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
    class Flame : Projectile, IDisposable
    {
        AnimationManager animMgr;
        Animation current_animation;
        float speed = 20;
		private readonly Vector2 targetPos;
        float timeToLive;
        float ttlCounter;
        Vector2 direction;
        public static int damage = 3;
        Random random;
        public Flame(Vector2 POS, Vector2 DIMS, Vector2 targetPos, double angle) : base(POS, DIMS)
        {

            random = new Random();
            timeToLive = random.NextSingle(0.2f, 3);
			this.targetPos = targetPos;
			animMgr = AnimationManager.GetInstance();
            current_animation = animMgr.GetAnimation("flame_1");

            direction.X = (float)(Math.Cos(angle + 1.57f));
            direction.Y = (float)(Math.Sin(angle + 1.57f));
            if (!direction.Equals(Vector2.Zero))
            {
                direction.Normalize();
            }

        }


		public override Vector2 getDrawPosition => isoPos();

        private Vector2 isoPos()
        {
            Vector2 drawPos = Globals.ToIsometric(pos);
            drawPos.Y += 94 * Globals.tileSize;
            return drawPos;
        }

        public override void Update(GameTime gameTime)
        {
            if (current_animation.AnimEnded())
            {
                current_animation = animMgr.PlayOnce("flame_" + random.Next(1, 4));
            } 
            ttlCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (ttlCounter > timeToLive || !alive)
            {
                Destroy();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Color color = (colliding) ? Color.Red : Color.White;
            //angle = (colliding) ? 0f : angle - 1.57f;
            spriteBatch.Draw(current_animation.sheet, new Rectangle(
                                                            (int)pos.X,
                                                            (int)pos.Y,
                                                            (int)(current_animation.frameSize * 3),
                                                            (int)(current_animation.frameSize * 3)),
                                                            current_animation.currentFrame,
                                                            Color.White,
                                                            0f,
                                                            new Vector2(current_animation.currentFrame.Width / 2, current_animation.currentFrame.Height / 2),
                                                            new SpriteEffects(),
                                                            0f);

        }

        public void Dispose()
        {

        }
    }
}
