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
    class Flamethrower : Tower
    {

        AnimationManager animMgr;
        Animation current_animation;
        double angle;
        bool targetLocked;
        float fireRate = 0.05f;
        float timeCounter;
        Entity target;
        EllipseF elipseRange;
        Random random;

        public Flamethrower(Vector2 POS, Vector2 DIMS) : base(POS, DIMS)
        {
            animMgr = AnimationManager.GetInstance();
            current_animation = animMgr.GetAnimation("flamethrower_idle");
            DrawManager.GetInstance().Add(this, 2);
            elipseRange = new EllipseF(POS, 150, 114);
            random = new Random();
        }
        public override Vector2 getDrawPosition => new Vector2(pos.X, pos.Y + 70);

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

                    Vector2 bulletOrigin1 = new Vector2(pos.X, pos.Y);
                    Vector2 t = new Vector2(target.pos.X + (float)random.Next(0, 30), target.pos.Y + (float)random.Next(0, 30));
                    Flame flame1 = new Flame(bulletOrigin1, new Vector2(4, 4), t, angle);
                    timeCounter = 0;
                }

            }
            if (target != null && !isWithinEllipse(target.pos, elipseRange))
            {
                targetLocked = false;
                current_animation = animMgr.GetAnimation("flamethrower_idle");
            }
            if (target == null || !target.alive)
            {
                targetLocked = false;
                current_animation = animMgr.GetAnimation("flamethrower_idle");
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
                                                            (float)angle,
                                                            new Vector2(current_animation.currentFrame.Width / 2, current_animation.currentFrame.Height / 2),
                                                            new SpriteEffects(),
                                                            0f);
            //spriteBatch.DrawEllipse(pos, new Vector2(206, 150), 50, c);




        }
    }
}
