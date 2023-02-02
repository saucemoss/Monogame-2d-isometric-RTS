
using GameProject.Source.Entities;
using GameProject.Source.Entities.Buildings;
using GameProject.Source.Entities.Enemies;
using GameProject.Source.Entities.Objects.Projectiles;
using GameProject.Source.Inputs;
using GameProject.Source.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using MonoGame.Extended.Tweening;
using System;
using System.Collections.Generic;

using static GameProject.Source.Tiles.IsometricTilemap;

namespace GameProject.Source.Main
{
    public class GameSpace
    {
        public Player player;
        public View view;
        public readonly IsometricTilemap isometricTilemap;
        AnimationManager animMgr;
        public BuilderManager builder;
        public Camera camera;
        public GraphicsDevice graphicsDevice;
        public readonly List<Tweener> tweeners;

        private static GameSpace instance;
        public static GameSpace GetInstance()
        {
            if (instance == null)
            {
                instance = new GameSpace();
            }
            return instance;
        }

        public GameSpace()
        {

            animMgr = AnimationManager.GetInstance();
            animMgr.initAnimations();
            view = new View(new Vector2(0, 1000));
            isometricTilemap = new IsometricTilemap();
            builder = BuilderManager.GetInstance();
            tweeners = new List<Tweener>();

        }

        public void Update(GameTime gameTime)
        {
            
            isometricTilemap.Update();
            EntityManager.GetInstance().Uptade(gameTime);
            animMgr.Update(gameTime);
            camera.Follow(view);
            view.Update(gameTime);
            CollisionManager.GetInstance().Update(gameTime);
            builder.Update(gameTime);


            if (InputManager.IsMousePressed(MouseButton.Right))
            {
                foreach (Entity e in EntityManager.GetInstance().entities)
				{
                    
                    Vector2 t = Vector2.Transform(new Vector2(InputManager.GetMouseState().X, InputManager.GetMouseState().Y), Camera.InverseTransform());
                    e.setTarget(t);

				}
			}

            if (InputManager.WasMousePressed(MouseButton.Left) && InputManager.IsKeyPressed(Keys.LeftControl))
            {
                Vector2 v = Vector2.Transform(new Vector2(InputManager.GetMouseState().X, InputManager.GetMouseState().Y), Camera.InverseTransform());
                Zombie zombie = new Zombie(v, new Vector2(12, 12), v);
            }


            //tweener test            
            /*			if (InputManager.WasMousePressed(MouseButton.Left))
                        {
                            Tweener tweener = new Tweener();

                            Vector2 test = isometricTilemap.GetTileIsoVectorFromMousePos();
                            Tile tile = isometricTilemap.tilesArray[isometricTilemap.GetTilePosNum(test)];

                            tweeners.Add(tweener);
                            tweeners.Add(tweener);
                            tweener.TweenTo(target: tile, expression: i => i.isoPos, toValue: new Vector2(tile.isoPos.X, tile.isoPos.Y - 16), duration: 1f, delay: 0)
                            .Easing(EasingFunctions.BounceOut)
                            .AutoReverse()
                            .OnEnd(t =>
                            {
                                tweeners.Remove(tweener);

                            });

                        }*/

            /*			for (int i = 0; i < tweeners.Count; i++)
                        {
                            Tweener t = tweeners[i];
                            t.Update(gameTime.GetElapsedSeconds());
                        }*/



        }

    }
}
