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
    class Howitzer : Tower
    {

        AnimationManager animMgr;
        Animation current_animation;
        Animation base_texture;
        double angle;
        bool targetLocked;
        float fireRate = 1.8f;
        float timeCounter;
        Entity target;
        EllipseF elipseRange;

        public Howitzer(Vector2 POS, Vector2 DIMS) : base(POS, DIMS)
        {
            animMgr = AnimationManager.GetInstance();
            current_animation = animMgr.GetAnimation("howitzer_idle");
            base_texture = animMgr.GetAnimation("howitzer_base");
            DrawManager.GetInstance().Add(this, 1);
            elipseRange = new EllipseF(POS, 412, 300);

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
                    current_animation = animMgr.PlayOnce("howitzer_fire");
                    Vector2 bulletOrigin1 = new Vector2(pos.X, pos.Y);
                    HowitzerShell shell = new HowitzerShell("Sprites\\bullet", bulletOrigin1, new Vector2(3, 3), target);
                    timeCounter = 0;
                }

            }
            if (target != null && !isWithinEllipse(target.pos, elipseRange))
            {
                targetLocked = false;
                current_animation = animMgr.GetAnimation("howitzer_idle");
            }
            if (target == null || !target.alive)
            {
                targetLocked = false;
                current_animation = animMgr.GetAnimation("howitzer_idle");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Color color = (colliding) ? Color.Red : Color.White;
            spriteBatch.Draw(base_texture.sheet, new Rectangle(
                                                (int)pos.X,
                                                (int)pos.Y,
                                                (int)(base_texture.frameSize * 3),
                                                (int)(base_texture.frameSize * 3)),
                                                base_texture.currentFrame,
                                                Color.White,
                                                0f,
                                                new Vector2(base_texture.currentFrame.Width / 2, base_texture.currentFrame.Height / 2),
                                                new SpriteEffects(),
                                                0f);
            spriteBatch.Draw(current_animation.sheet, new Rectangle(
                                                            (int)pos.X,
                                                            (int)pos.Y,
                                                            (int)(current_animation.frameSize * 3),
                                                            (int)(current_animation.frameSize * 3)),
                                                            current_animation.currentFrame,
                                                            Color.White,
                                                            (float)angle,
                                                            new Vector2(16, 18),
                                                            new SpriteEffects(),
                                                            0f);
        }
    }
}
