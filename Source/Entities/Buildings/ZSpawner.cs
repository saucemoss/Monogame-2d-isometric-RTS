using GameProject.Source.Entities.Enemies;
using GameProject.Source.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProject.Source.Entities.Buildings
{
	class ZSpawner : Entity
	{

		AnimationManager animMgr;
		Animation current_animation;
        float spawnCounter;
        float spawnCooldown;
        Random random;
        Vector2 target;

        public ZSpawner(Vector2 POS, Vector2 DIMS, float spawnCooldown = 1f) : base(POS, DIMS)
		{
            animMgr = AnimationManager.GetInstance();
            current_animation = animMgr.PlayOnce("spawner_building");
            DrawManager.GetInstance().Add(this, 1);
            this.spawnCooldown = spawnCooldown;
            random = new Random();
            target = new Vector2(150, 0);
            
        }

        public override void Update(GameTime gameTime)
        {
            if (current_animation.AnimEnded())
            {
                current_animation = animMgr.GetAnimation("spawner_idle");
            }
            //target = new Vector2(random.Next(0, 2000), random.Next(0, 2000));
            spawnCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (spawnCounter > spawnCooldown)
            {
                for (int i = 0; i < 3; i++)
                {
                    int radius = 30;
                   
                    int X = random.Next((int)pos.X - radius, (int)pos.X + radius);
                    int Y = random.Next((int)pos.Y - radius, (int)pos.Y + radius);
                    Zombie zombie = new Zombie(new Vector2(X, Y), new Vector2(10, 10), target);
                }
				if (random.Next(0, 20) == 0)
				{
					//BigZombie bigZombie = new BigZombie(pos, new Vector2(Globals.tileSize/2, Globals.tileSize/2), target);
				}
				spawnCounter = 0;
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
                                                            0f,
                                                            new Vector2(current_animation.currentFrame.Width / 2, current_animation.currentFrame.Height / 2),
                                                            new SpriteEffects(),
                                                            0f);

        }
    }
}
