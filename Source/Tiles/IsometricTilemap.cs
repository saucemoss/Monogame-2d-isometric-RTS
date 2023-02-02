using GameProject.Source.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using System;
using System.Collections.Generic;
using TiledCS;

using System.Linq;
using GameProject.Source.Entities.Buildings;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using MonoGame.Extended;

namespace GameProject.Source.Tiles
{
	public partial class IsometricTilemap
	{
		public TiledMap map;
		public TiledTileset tileset;
		public Texture2D tilesetTexture;
		public Tile[] tilesArray;
		public Rectangle[] tileRects;
		private Vector2 mousePos;
		Vector2 mouseInWorld;
		public Vector2 mapTileFromPixel;

		public int tileWidth;
		public int tileHeight;
		public int tilesetTilesWide;
		public int tilesetTilesHeight;
		public int layerSelect;
		public int tileSelect;
		

		public IsometricTilemap()
		{
			// Set the "Copy to Output Directory" property of these two files to `Copy if newer`
			// by clicking them in the solution explorer.
			
			map = new TiledMap(Globals.content.RootDirectory + "\\TileMaps\\isoMapTest\\map3.tmx");
			tileset = new TiledTileset(Globals.content.RootDirectory + "\\TileMaps\\isoMapTest\\tileset7.tsx");
			Globals.mapWidth = map.Width;
			Globals.mapHeight = map.Height;
			
			// Not the best way to do this but it works. It looks for "exampleTileset.xnb" file
			// which is the result of building the image file with "Content.mgcb".
			tilesetTexture = Globals.content.Load<Texture2D>("TileMaps\\isoMapTest\\tileset7");

			tileWidth = tileset.TileWidth;
			tileHeight = tileset.TileHeight;

			// Amount of tiles on each row (left right)
			tilesetTilesWide = tileset.Columns;
			// Amount of tiels on each column (up down)
			tilesetTilesHeight = tileset.TileCount / tileset.Columns;
			tileRects = new Rectangle[tileset.TileCount];


			int tcount = 0;
			for (int j = 0; j < tilesetTilesHeight; j++)
			{
				for (int i = 0; i < tilesetTilesWide; i++)
				{ 
					tileRects[tcount] = new Rectangle(tileWidth * i, tileHeight * j, tileWidth, tileHeight);
					tcount++;
				}
			}

			initTilesArray();

		}

		private void initTilesArray()
		{
			
			Rectangle tilesetRec = new Rectangle(0, 0, 0, 0);
			tilesArray = new Tile[map.Layers[0].data.Length];
			for (int i = 0; i < map.Layers.Length; i++)
			{
				for (int j = 0; j < map.Layers[0].data.Length; j++)
				{
					float x = (j % map.Width) * map.TileHeight;
					float y = (float)Math.Floor(j / (double)map.Width) * map.TileHeight;

					Vector2 cartesian = new Vector2(x, y);
					Vector2 isometric = Globals.ToIsometric(cartesian);

					int gid = map.Layers[i].data[j];
					// Empty tile, do nothing
					if (gid == 0)
					{

					}
					else
					{
						//List<Vector2> v = CreateVertices(i, gid);
						List<CollisionBox> collisionBoxes = CreateColliders(i, gid, isometric);
						int tileFrame = gid - 1;
						int column = tileFrame % tilesetTilesWide;
						int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);
						tilesetRec = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);
						bool occupied = (i==1) ? true : false; 
						Tile tile = new Tile(this, isometric, cartesian, tilesetRec, i, collisionBoxes, j, gid, occupied: occupied); ;
						tilesArray[j] = tile;
					}
				}
			}
		}

		private List<CollisionBox> CreateColliders(int layer, int gid, Vector2 pos)
		{
			int i = layer;
			List <CollisionBox> collisionBoxes = new List<CollisionBox>();
			collisionBoxes.Add(new CollisionBox(0, 0, 0, 0));
			var t = map.GetTiledTile(map.Tilesets[0], tileset, gid);
			if (t != null && i > 0)
			{
				collisionBoxes.Clear();

				for (int j = 0; j < t.objects.Length; j++)
				{
					collisionBoxes.Add(new CollisionBox(
						pos.X * Globals.scale + t.objects[j].x * Globals.scale, 
						pos.Y * Globals.scale + t.objects[j].y * Globals.scale, 
						t.objects[j].width * Globals.scale, 
						t.objects[j].height * Globals.scale, ignoreOffset: true));
					//System.Diagnostics.Debug.WriteLine(t.objects[j].x + "," + t.objects[j].y + "," + t.objects[j].width + "," + t.objects[j].height);
				}
			}

			return collisionBoxes;

		}

		public bool IsTileOccupied(Tile t)
		{
			return t.occupied;
		}
		public int GetTileIDFromIndex(int tilenum)
		{
			return map.Layers[1].data[tilenum];
		}

		public Tile GetTileFromIndex(int tilenum)
		{
			return tilesArray[tilenum];
		}

		public Tile GetTileFromMousePos()
		{
			return GetTileFromIndex(GetTilePosNum(GetTileIsoVectorFromMousePos()));
		}

		public Vector2 GetTileIsoVectorFromMousePos()
		{
			mousePos.X = MouseExtended.GetState().X;
			mousePos.Y = MouseExtended.GetState().Y;
			mouseInWorld = Vector2.Transform(new Vector2(mousePos.X, mousePos.Y), Camera.InverseTransform());
			mapTileFromPixel.X = (int)(mouseInWorld.X + (2 * mouseInWorld.Y) - (48)) / 96;
			mapTileFromPixel.Y = (int)(mouseInWorld.X - (2 * mouseInWorld.Y) - (48)) / -96;
			mapTileFromPixel.X -= 1;
			mapTileFromPixel.Y -= 1;
			return mapTileFromPixel;
		}

		public int GetTilePosNum(Vector2 tile) 
		{
			return (int)((tile.Y * map.Width) + (int)tile.X);
		}

		public Dictionary<string, int> GetNeighboursNums(Vector2 pos)
		{
			Dictionary<string, int> neighbours = new Dictionary<string, int>();

			neighbours.Add("top_left", GetTilePosNum(new Vector2(pos.X - 1, pos.Y - 1)));
			neighbours.Add("top", GetTilePosNum(new Vector2(pos.X, pos.Y - 1)));
			neighbours.Add("top_right", GetTilePosNum(new Vector2(pos.X + 1, pos.Y - 1)));
			neighbours.Add("left", GetTilePosNum(new Vector2(pos.X - 1, pos.Y)));
			neighbours.Add("center", GetTilePosNum(pos));
			neighbours.Add("right", GetTilePosNum(new Vector2(pos.X + 1, pos.Y)));
			neighbours.Add("bottom_left", GetTilePosNum(new Vector2(pos.X - 1, pos.Y + 1)));
			neighbours.Add("bottom", GetTilePosNum(new Vector2(pos.X, pos.Y + 1)));
			neighbours.Add("bottom_right", GetTilePosNum(new Vector2(pos.X + 1, pos.Y + 1)));

			return neighbours;
		}

		public Dictionary<int, Tile> GetNeighboursTiles(Tile t)
		{
			Dictionary<int, Tile> neighbours = new Dictionary<int, Tile>();
			Vector2 pos = GetSimpleIsoVector(t.tilenum);
			return GetNeighboursTiles(pos);
		}

		public Dictionary<int, Tile> GetNeighboursTiles(Vector2 pos)
		{
			Dictionary<int, Tile> neighbours = new Dictionary<int, Tile>();

			neighbours.Add(1, GetTileFromIndex(GetTilePosNum(new Vector2(pos.X - 1, pos.Y - 1))));
			neighbours.Add(2, GetTileFromIndex(GetTilePosNum(new Vector2(pos.X, pos.Y - 1))));
			neighbours.Add(3, GetTileFromIndex(GetTilePosNum(new Vector2(pos.X + 1, pos.Y - 1))));
			neighbours.Add(4, GetTileFromIndex(GetTilePosNum(new Vector2(pos.X - 1, pos.Y))));
			neighbours.Add(5, GetTileFromIndex(GetTilePosNum(pos)));
			neighbours.Add(6, GetTileFromIndex(GetTilePosNum(new Vector2(pos.X + 1, pos.Y))));
			neighbours.Add(7, GetTileFromIndex(GetTilePosNum(new Vector2(pos.X - 1, pos.Y + 1))));
			neighbours.Add(8, GetTileFromIndex(GetTilePosNum(new Vector2(pos.X, pos.Y + 1))));
			neighbours.Add(9, GetTileFromIndex(GetTilePosNum(new Vector2(pos.X + 1, pos.Y + 1))));

			return neighbours;
		}

		public void Update()
		{
			mapTileFromPixel = GetTileIsoVectorFromMousePos();

		}


		public Tile SetWallTile(int tileSelect, int layer, bool replace = true, int tilenum = 9999, bool occupied = false)
		{
			
			if (tilenum == 9999)
			{
				tilenum = GetTilePosNum(GetTileIsoVectorFromMousePos());
			}

			
			if (replace)
			{
				Tile oldTile = GetTileFromIndex(tilenum);
				DrawManager.GetInstance().Remove(oldTile, layer);
				oldTile.RemoveCollider();
			}

			Vector2 cartesian = GetScreenCartesianFromTilenum(tilenum);
			Vector2 isometric = GetScreenIsoFromTilenum(tilenum);
			List<CollisionBox> colliders = CreateColliders(1, tileSelect + 1, isometric);
			Wall t = new Wall(this, isometric, cartesian, tileRects[tileSelect], layer, colliders, tilenum, tileSelect, occupied);
			map.Layers[t.layer].data[tilenum] = tileSelect;
			tilesArray[tilenum] = t;
			return t;

		}

		public Tile SetTile(int tileSelect, int layer, bool replace = true, int tilenum = 9999, bool occupied = false)
		{
			
			if (tilenum == 9999)
			{
				tilenum = GetTilePosNum(GetTileIsoVectorFromMousePos());
			}

			
			if (replace)
			{
				Tile oldTile = GetTileFromMousePos();
				DrawManager.GetInstance().Remove(oldTile, layer);
				oldTile.RemoveCollider();
			}

			Vector2 cartesian = GetScreenCartesianFromTilenum(tilenum);
			Vector2 isometric = GetScreenIsoFromTilenum(tilenum);
			List<CollisionBox> colliders = CreateColliders(1, tileSelect, isometric);
			Tile t = new Tile(this, isometric, cartesian, tileRects[tileSelect], layer, colliders, tilenum, tileSelect, occupied);
			map.Layers[t.layer].data[tilenum] = tileSelect;
			tilesArray[tilenum] = t;
			return t;

		}

		public Vector2 GetSimpleIsoVector(int tilenum)
		{
			float x = (tilenum % map.Width);
			float y = (float)Math.Floor(tilenum / (double)map.Width);
			Vector2 cartesian = new Vector2(x, y);
			return cartesian;
		}

		public Vector2 GetScreenIsoFromTilenum(int tilenum)
		{
			float x = (tilenum % map.Width) * map.TileHeight;
			float y = (float)Math.Floor(tilenum / (double)map.Width) * map.TileHeight;
			Vector2 cartesian = new Vector2(x, y);
			return Globals.ToIsometric(cartesian);
		}
		
		public Vector2 GetScreenCartesianFromTilenum(int tilenum)
		{
			float x = (tilenum % map.Width) * map.TileHeight;
			float y = (float)Math.Floor(tilenum / (double)map.Width) * map.TileHeight;
			Vector2 cartesian = new Vector2(x, y);
			return cartesian;
		}
	}


}
