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
using GameProject.Source.Entities.Objects.Projectiles;

namespace GameProject.Source.Entities.Buildings
{
	class FiftyCalTurret : Tower
    {

		AnimationManager animMgr;
		Animation current_animation;
        double angle;
        bool targetLocked;
        float fireRate = 0.13f;
        float timeCounter;
        Entity target;
        EllipseF elipseRange;

        public FiftyCalTurret(Vector2 POS, Vector2 DIMS) : base(POS, DIMS)
		{
            animMgr = AnimationManager.GetInstance();
			current_animation = animMgr.GetAnimation("fifty_idle");
            DrawManager.GetInstance().Add(this, 2);
            elipseRange = new EllipseF(POS, 226, 165);

        }

        public override void Update(GameTime gameTime)
        {
            timeCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!targetLocked)
            {
                foreach (Entity e in EntityManager.GetInstance().entities)
                {
                    if (e.isEnemy)
                    {
                        if (isWithinEllipse(e.pos, elipseRange))
                        {
                            target = e;
                            targetLocked = true;
                        }
                    }
                }
            }

            if (targetLocked && target != null)
            {
                Vector2 direction = target.pos - pos;
                direction.Normalize();
                angle = (float)Math.Atan2(-direction.X, direction.Y);
                if (timeCounter > fireRate)
                {
                    current_animation = animMgr.GetAnimation("fifty_firing");
                    target.receiveDamage(150);
                    timeCounter = 0;
                }

            }
            if (target != null && !isWithinEllipse(target.pos, elipseRange))
            {
                targetLocked = false;
                current_animation = animMgr.GetAnimation("fifty_idle");
            }
            if (target == null || !target.alive)
            {
                targetLocked = false;
                current_animation = animMgr.GetAnimation("fifty_idle");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color = (colliding) ? Color.Red : Color.White;
            spriteBatch.Draw(current_animation.sheet, new Rectangle(
                                                            (int)pos.X,
                                                            (int)pos.Y,
                                                            (int)(current_animation.frameSize * 3),
                                                            (int)(current_animation.frameSize * 3)),
                                                            current_animation.currentFrame,
                                                            Color.White,
                                                            (float)angle,
                                                            new Vector2(current_animation.currentFrame.Width / 2, current_animation.currentFrame.Height / 2),
                                                            new SpriteEffects(),
                                                            0f);
            if (targetLocked)
            {
                spriteBatch.DrawLine(pos, target.pos, Color.White);
            }

            if (DebugInterface.debugViewOn)
            {

            }
        }
    }
}
