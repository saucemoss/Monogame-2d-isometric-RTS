using GameProject.Source.Entities.Buildings;
using GameProject.Source.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProject.Source.UI
{
	class UI
	{
		Texture2D whiteRectangle;
		Texture2D fifty, flameT, howitzerA, howitzerB;
		private SpriteFont font;
		private SpriteBatch spriteBatch;
		private Game1 game;
		public GraphicsDevice GraphicsDevice;
		private int screenWidht, screenHeight, screenSixthH, screenSixthW;
		private Vector2 screenCenter;
		private AnimationManager animMgr;
		private BuilderManager buildMgr;

		private int margin = 8;
		private int lineMargin = 14;

		public SpriteFont Font { get => font; set => font = value; }
		public UI(Game1 game)
		{
			this.game = game;
			this.GraphicsDevice = game.GraphicsDevice;
			spriteBatch = new SpriteBatch(GraphicsDevice);
			whiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
			whiteRectangle.SetData(new[] { Color.White });
			font = Globals.content.Load<SpriteFont>("font");
			initSizes();
			animMgr = AnimationManager.GetInstance();
			fifty = animMgr.GetAnimation("fifty_idle").sheet;
			flameT = animMgr.GetAnimation("flamethrower_idle").sheet;
			howitzerA = animMgr.GetAnimation("howitzer_base").sheet;
			buildMgr = BuilderManager.GetInstance();


		}

		public void initSizes()
		{
			screenWidht = Game1.screenWidth;
			screenHeight = Game1.screenHeight;
			screenCenter = new Vector2(Game1.screenWidth / 2, Game1.screenHeight / 2);
			screenSixthW = Game1.screenWidth / 6;
			screenSixthH = Game1.screenHeight / 6;
		}

		public void Update(GameTime gameTime)
		{
		}

		public void Draw(GameTime gameTime)
		{
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
			
			spriteBatch.Draw(whiteRectangle, new Rectangle(screenSixthW * 4, screenSixthH * 5, screenSixthW * 2 - 30, screenSixthH - 50),
					Color.Black * 0.75f);

			spriteBatch.Draw(whiteRectangle, new Rectangle(screenSixthW * 2, screenSixthH * 5, screenSixthW * 2 - 30, screenSixthH - 50),
			Color.Black * 0.75f);

			//fifty icon
			spriteBatch.Draw(fifty, new Rectangle(screenSixthW * 4 + 120, screenSixthH * 5 + 100, 50, 50), new Rectangle(0, 0, 16, 16),
			Color.White, 0f, new Vector2(fifty.Width / 2, fifty.Height / 2),new SpriteEffects(),0.1f);
			//flame icon
			spriteBatch.Draw(flameT, new Rectangle(screenSixthW * 4 + 180, screenSixthH * 5 + 100 , 50, 50), new Rectangle(0, 0, 16, 16),
			Color.White, 0f, new Vector2(fifty.Width / 2, fifty.Height / 2), new SpriteEffects(), 0.1f);
			//howitzerA icon
			spriteBatch.Draw(howitzerA, new Rectangle(screenSixthW * 4 + 420, screenSixthH * 5 + 250, 100, 100), new Rectangle(0, 0, 32, 32),
			Color.White, 0f, new Vector2(howitzerA.Width / 2, howitzerA.Height / 2), new SpriteEffects(), 0.1f);
			//howitzerB icon
			spriteBatch.Draw(howitzerA, new Rectangle(screenSixthW * 4+ 420, screenSixthH * 5 + 250, 100, 100), new Rectangle(0, 32, 32, 32),
			Color.White, 0f, new Vector2(howitzerA.Width / 2, howitzerA.Height / 2), new SpriteEffects(), 0.1f);
			buildMgr.Draw(spriteBatch);

			spriteBatch.End();
		}
	}
}
