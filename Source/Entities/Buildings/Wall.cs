using GameProject.Source.Main;
using GameProject.Source.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static GameProject.Source.Tiles.IsometricTilemap;

namespace GameProject.Source.Entities.Buildings
{
	class Wall : Tile
	{
		
		public Wall(IsometricTilemap isomap, Vector2 isoPos, Vector2 cartPos, Rectangle tilesetRec, int layer, List<CollisionBox> colliders, int tilenum, int gid, bool occupied = false) : base(isomap, isoPos, cartPos, tilesetRec, layer, colliders, tilenum, gid, occupied)
		{
			isWall = true;
		}
	}
}
