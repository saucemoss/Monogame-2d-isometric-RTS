using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProject.Source.Main
{
	public class AnimationManager
	{
		private static AnimationManager instance;
		public static Dictionary<string, Animation> Animations;

		public static AnimationManager GetInstance()
		{
			if (instance == null)
			{
				instance = new AnimationManager();
			}
			return instance;
		}

		static AnimationManager()
		{
			Animations = new Dictionary<string, Animation>();
		}

		public Animation GetAnimation(string a)
		{
			return Animations[a];
		}

		private Animation CopyAnimation(string a)
		{
			Animation anim = GetAnimation(a);
			Animation copy = new Animation(anim.texturePath, anim.spriteSheetRow, anim.framesCount, anim.frameSize, anim.frameTime);
			return copy;
		}

		public Animation PlayOnce(string a)
		{
			Animation anim = CopyAnimation(a);
			anim.playOnce = true;
			Animations.Add(a+anim.GetHashCode().ToString()+System.DateTime.Now.ToString(), anim);
			return anim;
		}

		public void Update(GameTime gameTime)
		{
			foreach (KeyValuePair<string, Animation> a in Animations)
			{
				if (a.Value.playOnce && a.Value.AnimEnded())
				{
					Animations.Remove(a.Key);
				}
				else
				{
					a.Value.Play(gameTime);
				}
			}
		}

		public void initAnimations()
		{

			//player
			Animations["player_up_unequipped"] = new Animation("Sprites\\player_moves_sprites16px", 0, 2, 16, 0.1f);
			Animations["player_down_unequipped"] = new Animation("Sprites\\player_moves_sprites16px", 1, 2, 16, 0.1f);
			Animations["player_left_unequipped"] = new Animation("Sprites\\player_moves_sprites16px", 2, 2, 16, 0.1f);
			Animations["player_right_unequipped"] = new Animation("Sprites\\player_moves_sprites16px", 3, 2, 16, 0.1f);
			Animations["player_idle_unequipped"] = new Animation("Sprites\\player_moves_sprites16px", 4, 2, 16, 0.1f);

			Animations["player_up_flamethrower"] = new Animation("Sprites\\player_moves_sprites16px", 0, 2, 16, 0.1f);
			Animations["player_down_flamethrower"] = new Animation("Sprites\\player_moves_sprites16px", 6, 2, 16, 0.1f);
			Animations["player_left_flamethrower"] = new Animation("Sprites\\player_moves_sprites16px", 10, 2, 16, 0.1f);
			Animations["player_right_flamethrower"] = new Animation("Sprites\\player_moves_sprites16px", 12, 2, 16, 0.1f);
			Animations["player_idle_flamethrower"] = new Animation("Sprites\\player_moves_sprites16px", 11, 2, 16, 0.1f);

			Animations["player_up_fireaxe"] = new Animation("Sprites\\player_moves_sprites16px", 0, 2, 16, 0.1f);
			Animations["player_down_fireaxe"] = new Animation("Sprites\\player_moves_sprites16px", 14, 2, 16, 0.1f);
			Animations["player_left_fireaxe"] = new Animation("Sprites\\player_moves_sprites16px", 16, 2, 16, 0.1f);
			Animations["player_right_fireaxe"] = new Animation("Sprites\\player_moves_sprites16px", 15, 2, 16, 0.1f);
			Animations["player_idle_fireaxe"] = new Animation("Sprites\\player_moves_sprites16px", 13, 2, 16, 0.1f);

			Animation down_attack = new Animation("Sprites\\player_fireaxe_attacks_sprites", 0, 6, 32, 0.03f);
			down_attack.setSpecificFrameTime(0, 0.08f);
			down_attack.setSpecificFrameTime(1, 0.04f);
			down_attack.setSpecificFrameTime(4, 0.04f);
			down_attack.setSpecificFrameTime(5, 0.08f);
			Animations["player_axe_down_attack"] = down_attack;

			Animations["player_axe_right_attack"] = new Animation("Sprites\\player_fireaxe_attacks_sprites", 1, 6, 32, 0.1f);
			Animations["player_axe_left_attack"] = new Animation("Sprites\\player_fireaxe_attacks_sprites", 2, 6, 32, 0.1f);
			Animations["player_axe_up_attack"] = new Animation("Sprites\\player_fireaxe_attacks_sprites", 3, 6, 32, 0.1f);

			//zombies
			//0
			Animations["zombie_down_0"] = new Animation("Sprites\\zombie_0", 1, 2, 8, 0.1f);
			Animations["zombie_idle_0"] = new Animation("Sprites\\zombie_0", 0, 5, 8, 0.1f);
			Animations["zombie_left_0"] = new Animation("Sprites\\zombie_0", 3, 5, 8, 0.1f);
			Animations["zombie_right_0"] = new Animation("Sprites\\zombie_0", 2, 5, 8, 0.1f);
			Animations["zombie_up_0"] = new Animation("Sprites\\zombie_0", 4, 2, 8, 0.1f);
			//1
			Animations["zombie_down_1"] = new Animation("Sprites\\zombie_1", 1, 2, 8, 0.1f);
			Animations["zombie_idle_1"] = new Animation("Sprites\\zombie_1", 0, 5, 8, 0.1f);
			Animations["zombie_left_1"] = new Animation("Sprites\\zombie_1", 3, 5, 8, 0.1f);
			Animations["zombie_right_1"] = new Animation("Sprites\\zombie_1", 2, 5, 8, 0.1f);
			Animations["zombie_up_1"] = new Animation("Sprites\\zombie_1", 4, 2, 8, 0.1f);
			//2
			Animations["zombie_down_2"] = new Animation("Sprites\\zombie_2", 1, 2, 8, 0.1f);
			Animations["zombie_idle_2"] = new Animation("Sprites\\zombie_2", 0, 5, 8, 0.1f);
			Animations["zombie_left_2"] = new Animation("Sprites\\zombie_2", 3, 5, 8, 0.1f);
			Animations["zombie_right_2"] = new Animation("Sprites\\zombie_2", 2, 5, 8, 0.1f);
			Animations["zombie_up_2"] = new Animation("Sprites\\zombie_2", 4, 2, 8, 0.1f);
			//3
			Animations["zombie_down_3"] = new Animation("Sprites\\zombie_3", 1, 2, 8, 0.1f);
			Animations["zombie_idle_3"] = new Animation("Sprites\\zombie_3", 0, 5, 8, 0.1f);
			Animations["zombie_left_3"] = new Animation("Sprites\\zombie_3", 3, 5, 8, 0.1f);
			Animations["zombie_right_3"] = new Animation("Sprites\\zombie_3", 2, 5, 8, 0.1f);
			Animations["zombie_up_3"] = new Animation("Sprites\\zombie_3", 4, 2, 8, 0.1f);
			//4
			Animations["zombie_down_4"] = new Animation("Sprites\\zombie_4", 1, 2, 8, 0.1f);
			Animations["zombie_idle_4"] = new Animation("Sprites\\zombie_4", 0, 5, 8, 0.1f);
			Animations["zombie_left_4"] = new Animation("Sprites\\zombie_4", 3, 5, 8, 0.1f);
			Animations["zombie_right_4"] = new Animation("Sprites\\zombie_4", 2, 5, 8, 0.1f);
			Animations["zombie_up_4"] = new Animation("Sprites\\zombie_4", 4, 2, 8, 0.1f);
			//5
			Animations["zombie_down_5"] = new Animation("Sprites\\zombie_5", 1, 2, 8, 0.1f);
			Animations["zombie_idle_5"] = new Animation("Sprites\\zombie_5", 0, 5, 8, 0.1f);
			Animations["zombie_left_5"] = new Animation("Sprites\\zombie_5", 3, 5, 8, 0.1f);
			Animations["zombie_right_5"] = new Animation("Sprites\\zombie_5", 2, 5, 8, 0.1f);
			Animations["zombie_up_5"] = new Animation("Sprites\\zombie_5", 4, 2, 8, 0.1f);

			//big zombie
			Animations["big_zombie_idle"] = new Animation("Sprites\\bigzombie", 0, 10, 32, 0.15f);
			Animations["big_zombie_down"] = new Animation("Sprites\\bigzombie", 1, 8, 32, 0.15f);
			Animations["big_zombie_up"] = new Animation("Sprites\\bigzombie", 2, 8, 32, 0.15f);
			Animations["big_zombie_right"] = new Animation("Sprites\\bigzombie", 3, 9, 32, 0.15f);
			Animations["big_zombie_left"] = new Animation("Sprites\\bigzombie", 4, 9, 32, 0.15f);
			
			//50cal turret
			Animations["fifty_idle"] = new Animation("Sprites\\fiftycalturret", 0, 1, 16, 1f);
			Animations["fifty_firing"] = new Animation("Sprites\\fiftycalturret", 1, 4, 16, 0.03f);

			//bullet
			Animations["bullet"] = new Animation("Sprites\\bullet", 0, 1, 1, 1f);

			//zSpawner
			Animations["spawner_building"] = new Animation("Sprites\\zSpawner", 1, 10, 32, 0.1f);
			Animations["spawner_idle"] = new Animation("Sprites\\zSpawner", 0, 10, 32, 0.1f);

			//howitzer
			Animations["howitzer_base"] = new Animation("Sprites\\howitzer", 0, 1, 32, 1f);
			Animations["howitzer_idle"] = new Animation("Sprites\\howitzer", 1, 1, 32, 1f);
			Animation howitzer_fire = new Animation("Sprites\\howitzer", 2, 6, 32, 0.1f);
			howitzer_fire.setSpecificFrameTime(2, 0.1f);
			howitzer_fire.setSpecificFrameTime(3, 0.2f); 
			howitzer_fire.setSpecificFrameTime(4, 0.2f);
			howitzer_fire.setSpecificFrameTime(5, 0.2f);
			Animations["howitzer_fire"] = howitzer_fire;
			//howitzer bullet
			Animations["howitzer_bullet_idle"] = new Animation("Sprites\\howitzer", 3, 1, 32, 1f);
			Animations["howitzer_bullet_boom"] = new Animation("Sprites\\howitzer", 4, 4, 32, 0.02f);

			//flames
			Animations["flame_1"] = new Animation("Sprites\\flames", 0, 4, 8, 0.05f);
			Animations["flame_2"] = new Animation("Sprites\\flames", 1, 4, 8, 0.05f);
			Animations["flame_3"] = new Animation("Sprites\\flames", 2, 4, 8, 0.05f); 
			Animations["flame_4"] = new Animation("Sprites\\flames", 3, 4, 8, 0.05f);

			//flamethrower
			Animations["flamethrower_idle"] = new Animation("Sprites\\flamethrower", 0, 1, 16, 1f);

			//buildings
			Animations["buildings_house_1"] = new Animation("Sprites\\buildings", 0, 1, 32, 1f);
			Animations["buildings_house_2"] = new Animation("Sprites\\buildings", 1, 1, 32, 1f);
			Animations["buildings_wall"] = new Animation("Sprites\\buildings", 2, 1, 32, 1f);
			Animations["buildings_fifty"] = new Animation("Sprites\\buildings", 3, 1, 32, 1f);
			Animations["buildings_flamethrower"] = new Animation("Sprites\\buildings", 4, 1, 32, 1f);
			Animations["buildings_howitzer"] = new Animation("Sprites\\buildings", 5, 1, 32, 1f);
		}
	}


	public class Animation
	{
		Rectangle[] frames;
		float[] frameTimes;
		public Texture2D sheet;
		public string texturePath;
		public int spriteSheetRow;
		public int framesCount;
		public int frameSize;
		public Rectangle currentFrame;
		private int frameCounter = 0;
		public float frameTime;
		private float timer = 0;
		public Boolean end, playOnce, ended;


		public Animation(string texturePath, int spriteSheetRow, int framesCount, int frameSize, float frameTime)
		{
			this.texturePath = texturePath;
			this.frameSize = frameSize;
			this.frameTime = frameTime;
			this.framesCount = framesCount;
			this.spriteSheetRow = spriteSheetRow;
			frames = new Rectangle[framesCount];
			frameTimes = new float[framesCount];
			sheet = Globals.content.Load<Texture2D>(texturePath);
			setStandardFrameTimes();
			addRectangles(spriteSheetRow);
		}

		private void setStandardFrameTimes()
		{
			for (int i = 0; i < frameTimes.Length; i++)
			{
				frameTimes[i] = frameTime;
			}
		}

		private void addRectangles(int spriteSheetRow)
		{
			for (int i = 0; i < frames.Length; i++)
			{
				frames[i] = new Rectangle(i * frameSize, spriteSheetRow * frameSize, frameSize, frameSize);
			}
		}

		public void setSpecificFrameTime(int frame, float time) 
		{
			frameTimes[frame] = time;
		}

		public void switchFrame(GameTime gameTime) 
		{
			var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			timer += delta;
			currentFrame = frames[frameCounter];
			if (timer >= frameTimes[frameCounter])
			{
				frameCounter++;
				timer = 0;
				if (frameCounter == frames.Length)
				{
					frameCounter = 0;
					end = true;
					ended = true;
				}
			}
		}
		public void Loop(GameTime gameTime) 
		{
			switchFrame(gameTime);
		}

		public void Play(GameTime gameTime)
		{
			if (!end)
			{
				switchFrame(gameTime);
			}
			else
			{
				frameCounter = 0;
				timer = 0;
				currentFrame = frames[frameCounter];
				end = false;
			}
		}

		public bool AnimEnded()
		{
			if (ended)
			{
				return true;
			}
			return false;
		}

	}
}
