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

namespace GameProject.Source.Entities.Enemies
{
    class BigZombie : Enemy
    {
        AnimationManager animMgr;
        Vector2 direction;
        CollisionBox collider;
        string moveX, moveY;
        float speed = 0.3f;
        int skin;
        Random random;

        private float timer;

        public override Vector2 getDrawPosition => pos;



        public BigZombie(Vector2 POS, Vector2 DIMS, Vector2 target) : base(POS, DIMS)
        {
            this.target = target;
            HP = 6000;
            maxHP = HP;
            random = new Random();
            skin = random.Next(0, 5);
            animMgr = AnimationManager.GetInstance();
            collider = new CollisionBox(POS.X, POS.Y, DIMS.X, DIMS.Y);
            current_animation = animMgr.GetAnimation("big_zombie_down");
            direction = Vector2.Zero;
            alive = true;

            moveX = "idle";
            moveY = "idle";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //direction = Vector2.Zero;

            Move(gameTime);
            if (!alive)
            {
                Destroy();
            }

        }

        private void Move(GameTime gameTime)
        {

            MoveTo(target);

            if (direction.Y >= 0.5f || direction.Y <= -0.5f)
            {
                current_animation = animMgr.GetAnimation("big_zombie_" + moveY);
            }
            else
            {
                current_animation = animMgr.GetAnimation("big_zombie_" + moveX);
            }
        }

        private void MoveTo(Vector2 target)
        {
            double distanceX = pos.X - target.X;
            double distanceY = pos.Y - target.Y;
            double angle = Math.Atan2(distanceY, distanceX);
            direction.X = -(float)(Math.Cos(angle));
            direction.Y = -(float)(Math.Sin(angle));
            if (pos.X < target.X)
            {
                moveX = "right";
            }
            if (pos.Y < target.Y)
            {
                moveY = "down";
            }
            if (pos.X > target.X)
            {
                moveX = "left";
            }
            if (pos.Y > target.Y)
            {
                moveY = "up";
            }
            if (!direction.Equals(Vector2.Zero))
            {
                direction.Normalize();
            }
            Vector2 move = direction * speed;
            if (!collider.cm.WillAABBSpatialCollide(collider, move))
            {
                colliding = false;
            }
            else if (!collider.cm.WillAABBSpatialCollide(collider, new Vector2(0, move.Y)))
            {
                move = new Vector2(0, move.Y);
            }
            else if (!collider.cm.WillAABBSpatialCollide(collider, new Vector2(move.X, 0)))
            {
                move = new Vector2(move.X, 0);
            }
            else
            {
                move = Vector2.Zero;
                colliding = true;
            }

            if (colliding)
            {
                RectangleF intersection = collider.cm.GetCollisionIntersection(collider);
                if (intersection.X >= collider.x)
                {
                    move.X -= intersection.Width;
                }
                if (intersection.Y >= collider.y)
                {
                    move.Y -= intersection.Height;
                }
                if (intersection.X <= collider.x)
                {
                    move.X += intersection.Width;
                }
                if (intersection.Y <= collider.y)
                {
                    move.Y += intersection.Height;
                }

                    move = direction * speed;
                
            }

            if (distanceTo(target) < 2)
            {
                pos = target;
            }
            else
            {
                pos += move;

            }

            collider.x = pos.X - collider.w / 2;
            collider.y = pos.Y - collider.h / 2;
            collider.drawRectangle.Position = new Vector2(collider.x, collider.y);
        }



        private void MoveRandom(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer += delta;
            if (timer >= 1f)
            {
                int i = random.Next(0, 8);
                if (i == 0)
                {
                    moveY = "up";
                    direction.Y = -0.7f;
                }
                else if (i == 1)
                {
                    moveY = "down";
                    direction.Y = 0.7f;
                }
                else if (i == 2)
                {
                    moveX = "left";
                    direction.X = -1f;
                }
                else if (i == 3)
                {
                    moveX = "right";
                    direction.X = 1f;
                }
                else if (i > 3)
                {
                    moveX = "idle";
                    direction = Vector2.Zero;
                }
                timer = 0;
            }

            direction = normalizeDirection(direction);


        }



        public Vector2 getPos()
        {
            return pos;
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Color color = (takingDamage) ? Color.Red : Color.White;
            spriteBatch.Draw(current_animation.sheet, new Rectangle(
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
            takingDamage = false;
            if (DebugInterface.debugViewOn)
            {


            }
        }
    }
}
