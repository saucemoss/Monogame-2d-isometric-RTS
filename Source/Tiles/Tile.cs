using GameProject.Source.Inputs;
using GameProject.Source.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using MonoGame.Extended.Shapes;
using MonoGame.Extended;
using GameProject.Source.UI;
using tainicom.Aether.Physics2D.Dynamics;

namespace GameProject.Source.Tiles
{
	public partial class IsometricTilemap
	{
		public class Tile : IDraw
		{
			IsometricTilemap isomap;
			public int tilenum;
			public int gid;
			public Vector2 isoPos;
			public Vector2 cartPos;
			Texture2D tilesetTexture;
			public Rectangle tilesetRec;
			public int layer;
			Vector2 drawPos;
			public bool colliding, occupied;
			public bool isWall;
			List<CollisionBox> collisionBoxes;

			public Vector2 getDrawPosition => new Vector2(drawPos.X, drawPos.Y + 70);

			public Tile(IsometricTilemap isomap, Vector2 isoPos, Vector2 cartPos, Rectangle tilesetRec, int layer, List<CollisionBox> collisionBoxes, int tilenum, int gid, bool occupied = false)
			{
				this.gid = gid;
				this.tilenum = tilenum;
				this.occupied = occupied;
				this.isomap = isomap;
				this.isoPos = isoPos;
				this.cartPos = cartPos;
				this.tilesetTexture = isomap.tilesetTexture;
				this.tilesetRec = tilesetRec;
				this.layer = layer;
				
				drawPos = isoPos * Globals.scale;
				drawPos.Y += 0;
				this.collisionBoxes = collisionBoxes;
				CollisionManager cm = CollisionManager.GetInstance();
				for (int i = 0; i < collisionBoxes.Count; i++)
				{
					cm.AssignStaticCollidersToSpatialDivison(collisionBoxes[i]);
				}
				
				DrawManager.GetInstance().Add(this, layer);

			}


			public bool IsOccupied()
			{
				return this.occupied;
			}
			public void RemoveCollider()
			{
				for (int i = 0; i < collisionBoxes.Count; i++)
				{
					collisionBoxes[i].Destroy();
				}
			}



			public void Draw(SpriteBatch batch)
			{
				Color color = (colliding) ? Color.Red : Color.White;
				if (cartPos.X/Globals.originalSize  == isomap.mapTileFromPixel.X && cartPos.Y / Globals.originalSize == isomap.mapTileFromPixel.Y && !InputManager.mouseOverUI)
				{
					batch.Draw(
						tilesetTexture,
						new Rectangle((int)isoPos.X * Globals.scale, ((int)isoPos.Y * Globals.scale) - 8, isomap.tileWidth * Globals.scale, isomap.tileHeight * Globals.scale),
						tilesetRec,
						color);
				}
				else
				{
					batch.Draw(
						tilesetTexture,
						new Rectangle((int)isoPos.X * Globals.scale, (int)isoPos.Y * Globals.scale, isomap.tileWidth * Globals.scale, isomap.tileHeight * Globals.scale),
						tilesetRec,
						color);
				}
				if (DebugInterface.debugViewOn)
				{
					////batch.DrawPolygon(isoPos * Globals.scale, polygon, Color.Red);
				}
			}

		}
	}


}
