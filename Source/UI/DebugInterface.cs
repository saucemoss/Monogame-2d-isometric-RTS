using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using MonoGame.Extended.Input;
using GameProject.Source.Main;
using GameProject.Source.Tiles;
using GameProject.Source.Entities;

namespace GameProject.Source.UI
{

	public class DebugInterface
	{
		private static SpriteBatch spriteBatch;

		Texture2D whiteRectangle;
		Rectangle rect;
		public static bool debugViewOn;
		public static SpriteFont font;
		private int margin = 8;
		private int lineMargin = 14;
		private IsometricTilemap isomap;
		private int intToDebug;
		private string intname;
		private Vector2 vectorToDebug;
		private string vecname;
		double frameRate;
		public Vector2 mousePos;
		Vector2 orthoTilePos;
		Vector2 isoTilePos;
		Vector2 mouseInWorld;
		Vector2 mouseInTileOffset;
		Vector2 mapTileFromPixel;
		List<String> valuesToDebugNames;
		List<Object> valuesToDebug;


		public DebugInterface(GraphicsDevice graphicsDevice, GameSpace world, IsometricTilemap isomap) 
		{
			this.isomap = isomap;
			spriteBatch = new SpriteBatch(graphicsDevice);
			whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
			whiteRectangle.SetData(new[] { Color.White });
			font = Globals.content.Load<SpriteFont>("font");
			valuesToDebug = new List<object>();
			valuesToDebugNames = new List<String>();

		}

		public void DebugInt(string valname, int value)
		{
			intname = valname;
			intToDebug = value;
		}

		public void DebugVector(string valname, Vector2 vector)
		{
			vecname = valname;
			vectorToDebug = vector;
		}
		public void AddDebugValue(String name, Object obj)
		{
			valuesToDebug.Add(obj);
			valuesToDebugNames.Add(name);
		}

		public void Update(GameTime gameTime)
		{
			mousePos.X = MouseExtended.GetState().X;
			mousePos.Y = MouseExtended.GetState().Y;

			//mouseInWorld = Vector2.Transform(new Vector2(mousePos.X, mousePos.Y), Camera.InverseTransform());

			orthoTilePos.X = (float)(Math.Floor(mouseInWorld.X / Globals.tileSize/2)*2);
			orthoTilePos.Y = (float)(Math.Floor(mouseInWorld.Y / Globals.tileSize));
			mouseInTileOffset.X = mouseInWorld.X % (Globals.tileSize*2);
			mouseInTileOffset.Y = mouseInWorld.Y % Globals.tileSize;
			isoTilePos = Globals.ToCartesian(orthoTilePos);

			mapTileFromPixel.X = (int)((mouseInWorld.X + (2 * mouseInWorld.Y) - (48)) / 96)-1;
			mapTileFromPixel.Y = (int)((mouseInWorld.X - (2 * mouseInWorld.Y) - (48)) / -96)-1;

		}

		public void Draw(GameTime gameTime)
		{

		//UI render
		spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

		frameRate = 1 / gameTime.ElapsedGameTime.TotalSeconds;

		spriteBatch.Draw(whiteRectangle, new Rectangle(margin, margin, 300, 300),
								Color.Black * 0.75f);

		spriteBatch.DrawString(font, "FPS "+((decimal)frameRate), new Vector2(margin*2, lineMargin), Color.White);
		spriteBatch.DrawString(font, "Screen Mouse " + mousePos, new Vector2(margin * 2, lineMargin * 2), Color.White);
		spriteBatch.DrawString(font, "World Mouse " + mouseInWorld, new Vector2(margin * 2, lineMargin * 3), Color.White);
		spriteBatch.DrawString(font, "Ortho Tile " + orthoTilePos, new Vector2(margin * 2, lineMargin * 4), Color.White);
		spriteBatch.DrawString(font, "Iso Tile " + isoTilePos, new Vector2(margin * 2, lineMargin * 5), Color.White);
		spriteBatch.DrawString(font, "mapTileFromPixel " + mapTileFromPixel, new Vector2(margin * 2, lineMargin * 7), Color.White);
		spriteBatch.DrawString(font, "tile selected " + isomap.tileSelect, new Vector2(margin * 2, lineMargin * 8), Color.White);
		spriteBatch.DrawString(font, "tile layer " + isomap.layerSelect, new Vector2(margin * 2, lineMargin * 9), Color.White);

		spriteBatch.DrawString(font, "Entities " + EntityManager.GetInstance().entities.Count, new Vector2(margin * 2, lineMargin * 10), Color.White);
		spriteBatch.DrawString(font, "Draws l1 " + DrawManager.GetInstance().layerOne.Count, new Vector2(margin * 2, lineMargin * 12), Color.White);
		spriteBatch.DrawString(font, "Draws l2 " + DrawManager.GetInstance().layerTwo.Count, new Vector2(margin * 2, lineMargin * 13), Color.White);
		spriteBatch.DrawString(font, "Draws l3 " + DrawManager.GetInstance().layerThree.Count, new Vector2(margin * 2, lineMargin * 14), Color.White);
		spriteBatch.DrawString(font, "Anims " + AnimationManager.Animations.Count, new Vector2(margin * 2, lineMargin * 15), Color.White);
		//spriteBatch.DrawString(font, "Collisions " + TestCollisionWorldManager.GetInstance().world., new Vector2(margin * 2, lineMargin * 16), Color.White);

			spriteBatch.End();


		//Camera render
		Globals.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: Camera.Transform);
		Globals.spriteBatch.Draw(whiteRectangle, new Rectangle((int)(orthoTilePos.X * Globals.tileSize), (int)(orthoTilePos.Y * Globals.tileSize), 2 * Globals.tileSize, Globals.tileSize),
								Color.Black * 0.75f);

			//////////////////////
			///

			for (int i = 0; i < isomap.map.Layers.Length; i++)
			{
				for (int j = 0; j < isomap.map.Layers[i].data.Length; j++)
				{

					int gid = isomap.map.Layers[i].data[j];
					// Empty tile, do nothing
					if (gid == 0)
					{

					}
					else
					{

						var tile = isomap.map.GetTiledTile(isomap.map.Tilesets[0], isomap.tileset, gid);
						if (tile != null)
						{
							//System.Diagnostics.Debug.WriteLine(tile.id);
						}

						int tileFrame = gid - 1;
						int column = tileFrame % isomap.tilesetTilesWide;
						int row = (int)Math.Floor((double)tileFrame / (double)isomap.tilesetTilesWide);
						Rectangle tilesetRec = new Rectangle(isomap.tileWidth * 0, isomap.tileHeight * 1, isomap.tileWidth, isomap.tileHeight);
						float x = (j % isomap.map.Width) * isomap.map.TileHeight;
						float y = (float)Math.Floor(j / (double)isomap.map.Width) * isomap.map.TileHeight;
						Vector2 cartesian = new Vector2(x,y);
						Vector2 isometric = Globals.ToIsometric(cartesian);
						Globals.spriteBatch.DrawString(font, ""+ (int)(cartesian.X / 16)+","+(int)(cartesian.Y / 16) + ",", new Vector2((isometric.X + 6)*Globals.scale, (isometric.Y + 6)*Globals.scale+48), Color.Black);
						//Globals.spriteBatch.DrawString(font, "" + x + "," + y + ",", new Vector2((isometric.X + 6) * Globals.scale, (isometric.Y + 6) * Globals.scale), Color.Black);
						//Globals.spriteBatch.DrawString(font, "" + isometric.X * Globals.scale + "," + isometric.Y * Globals.scale + ",", new Vector2((isometric.X + 6) * Globals.scale, (isometric.Y + 6) * Globals.scale), Color.Black);

					}
				}
			}
			
			Globals.spriteBatch.End();

		}

	}
}
