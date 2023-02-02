using GameProject.Source.Inputs;
using GameProject.Source.Main;
using GameProject.Source.Tiles;
using GameProject.Source.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.Tweening;
using System;
using System.Collections.Generic;
using static GameProject.Source.Tiles.IsometricTilemap;

namespace GameProject
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager _graphics;
        private GameSpace gameSpace;
        private DrawManager drawManager;
        private DebugInterface debugUI;
		public static int screenHeight = 1080;
        public static int screenWidth = 1920;
        private Camera camera;
        private UI ui;
        public static bool DebugOn;
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        protected override void Initialize()
        {

            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
            screenWidth = _graphics.PreferredBackBufferWidth;
            screenHeight = _graphics.PreferredBackBufferHeight;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Globals.content = this.Content;
            Globals.spriteBatch = new SpriteBatch(GraphicsDevice);
            camera = new Camera();
            gameSpace = GameSpace.GetInstance();
            gameSpace.graphicsDevice = GraphicsDevice;
            ui = new UI(this);
            gameSpace.camera = camera;
            debugUI = new DebugInterface(GraphicsDevice, gameSpace, gameSpace.isometricTilemap);
            drawManager = DrawManager.GetInstance();
            Globals.debugUI = this.debugUI;

        }

        protected override void Update(GameTime gameTime)
		{
            
            InputManager.Update(gameTime);
            gameSpace.Update(gameTime);
            drawManager.Update();
            if (DebugOn)
            {
                debugUI.Update(gameTime);
            }
 
            base.Update(gameTime);
		}



		protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            
            Globals.spriteBatch.Begin(sortMode: SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: Camera.Transform);

            drawManager.Draw();
            CollisionManager.GetInstance().Draw(Globals.spriteBatch, gameTime);
            Globals.spriteBatch.End();
            ui.Draw(gameTime);

            if (DebugOn) 
            { 
                debugUI.Draw(gameTime);
                
            }
            base.Draw(gameTime);
        }

    }
}
