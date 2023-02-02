using GameProject.Source.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using GameProject.Source.UI;

namespace GameProject.Source.Entities
{
    public abstract class Entity : IDraw
    {
        public Texture2D img;
        public Vector2 pos, dims;
        protected CollisionBox collider;
        protected Vector2 target;
        public bool alive = true;
        public bool isEnemy = false;
        public bool takingDamage = false;
        public int HP;
        public int maxHP;
        public bool colliding;



        public Entity(Vector2 POS, Vector2 DIMS, string PATH = "placeholder")
        {
            this.pos = POS;
            this.dims = DIMS;
            if (!PATH.Equals("placeholder"))
            {
                img = Globals.content.Load<Texture2D>(PATH);
            }
            DrawManager.GetInstance().Add(this, 1);
            EntityManager.GetInstance().Add(this);
        }
        public virtual bool OnCollision(Fixture fixture, Fixture fixture1, Contact contact)
        {
            colliding = true;
            return true;
        }

        public virtual void OnSeparation(Fixture fixture, Fixture fixture1, Contact contact)
        {
            colliding = false;
        }


        public virtual Vector2 getDrawPosition => isoPos();

        public virtual void setTarget(Vector2 target)
        {
            this.target = target;
        }


		private Vector2 isoPos()
        {
            return Globals.ToIsometric(pos);
        }

        public int distanceTo(Vector2 target)
        {
            
            int distance = 0;
            int xDiff = Math.Abs((int)pos.X - (int)target.X);
            int yDiff = Math.Abs((int)pos.Y - (int)target.Y);
            return distance = (int)Math.Sqrt(xDiff + yDiff);
        }

        public bool isWithinEllipse(Vector2 target, EllipseF range)
        {
            return range.Contains(target);
        }

        public Vector2 normalizeDirection(Vector2 direction)
        {
            if (!direction.Equals(Vector2.Zero) && !direction.X.Equals(0))
            {
                if (direction.Y>0)
                {
                    direction.Y = 0.5f;
                }
                else if (direction.Y<0)
                {
                    direction.Y = -0.5f;
                }
                direction.Normalize();
            }
            return direction;
        }

        public void receiveDamage(int i)
        {
            HP -= i;
            takingDamage = true;
            if (HP < 0)
            {
                alive = false;
            }
        }


        public virtual void Update(GameTime gameTime)
        {

        }
        public void Destroy()
        {
            alive = false;
            EntityManager.GetInstance().Remove(this);
            DrawManager.GetInstance().Remove(this, 1);
            DrawManager.GetInstance().Remove(this, 2);
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

            if (DebugInterface.debugViewOn)
            {
                Globals.spriteBatch.DrawRectangle(new RectangleF(pos.X-dims.X/2, pos.Y - dims.Y / 2, dims.X, dims.Y), Color.Red);
            }
        }


	}
}
