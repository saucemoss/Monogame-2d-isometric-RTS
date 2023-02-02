
using GameProject.Source.Entities;
using GameProject.Source.Entities.Buildings;
using GameProject.Source.Entities.Enemies;
using GameProject.Source.Entities.Objects.Projectiles;
using GameProject.Source.Inputs;
using GameProject.Source.Main;
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

namespace GameProject.Source.Entities.Buildings
{
	public class BuilderManager
    {
		public IsometricTilemap isometricTilemap;
        Animation current_animation;
        AnimationManager animMgr;
        bool IsBuildingSelected;
        public Dictionary<int, Rectangle> TextureRects;
        private static BuilderManager instance;
        bool IsWallsConnected;
        public static BuilderManager GetInstance()
        {
            if (instance == null)
            {
                instance = new BuilderManager();
            }
            return instance;
        }
        private BuilderManager()
        {
            TextureRects = new Dictionary<int, Rectangle>();
            animMgr = AnimationManager.GetInstance();
            current_animation = animMgr.GetAnimation("buildings_wall");
            
        }

        public void setIsoMap(IsometricTilemap isometricTilemap)
        {
            this.isometricTilemap = isometricTilemap;
        }

        public static Building building = Building.wall;
        public static void setBuilding(Building b) => building = b;


		public void Update(GameTime gameTime)
		{

            SelectBuilding();
            PlaceSelectedBuilding();

		}


        private void PlaceSelectedBuilding()
        {
            if (InputManager.IsMousePressed(MouseButton.Left) && IsBuildingSelected)
            {
                Vector2 towerPos;
                setIsoMap(GameSpace.GetInstance().isometricTilemap);
                Tile t = isometricTilemap.GetTileFromMousePos();
                //System.Diagnostics.Debug.WriteLine(isometricTilemap.tilesArray.Length);
                if (!t.IsOccupied())
                {
                    switch (building)
                    {
                        case Building.wall:
                            var list = isometricTilemap.GetNeighboursTiles(isometricTilemap.GetTileFromMousePos());
                            BuildWall(list);
                            
                            for (int i = 0; i < isometricTilemap.map.Layers[1].data.Length; i++)
							{
								if (isometricTilemap.tilesArray[i].isWall)
								{
									var allWalllist = isometricTilemap.GetNeighboursTiles(isometricTilemap.GetSimpleIsoVector(i));
									FormatWall(allWalllist);
								}
							}

							break;
                        case Building.house:
                            towerPos = GameSpace.GetInstance().isometricTilemap.SetTile(42, 1, true, occupied: true).isoPos * Globals.scale;
                            break;
                        case Building.fiftycal:
                            towerPos = GameSpace.GetInstance().isometricTilemap.SetTile(6, 1, false, occupied: true).isoPos * Globals.scale;
                            towerPos.X = towerPos.X + 48;
                            towerPos.Y = towerPos.Y + 15;
                            FiftyCalTurret fifty = new FiftyCalTurret(towerPos, new Vector2(Globals.tileSize, Globals.tileSize));
                            break;
                        case Building.flamethrower:
                            towerPos = GameSpace.GetInstance().isometricTilemap.SetTile(6, 1, false, occupied: true).isoPos * Globals.scale;
                            towerPos.X = towerPos.X + 48;
                            towerPos.Y = towerPos.Y + 15;
                            Flamethrower flamethrower = new Flamethrower(towerPos, new Vector2(Globals.tileSize, Globals.tileSize));
                            break;
                        case Building.howitzer:
                            towerPos = GameSpace.GetInstance().isometricTilemap.SetTile(13, 1, false, occupied: true).isoPos * Globals.scale;
                            towerPos.X = towerPos.X + 48;
                            towerPos.Y = towerPos.Y + 48;
                            Howitzer howitzer = new Howitzer(towerPos, new Vector2(Globals.tileSize, Globals.tileSize));
                            break;
                        case Building.spawner:
                            towerPos = GameSpace.GetInstance().isometricTilemap.SetTile(13, 0, false, occupied: true).isoPos * Globals.scale;
                            towerPos.X = towerPos.X + 48;
                            towerPos.Y = towerPos.Y + 48;
                            ZSpawner spawner = new ZSpawner(towerPos, new Vector2(Globals.tileSize, Globals.tileSize));
                            break;
                        default:
                            break;
                    }
                }
            }
        
            if (InputManager.WasMousePressed(MouseButton.Right))
            {
                IsBuildingSelected = false;
            }
        }

		private void BuildWall(Dictionary<int, Tile> list)
        {
            
            var t = isometricTilemap.SetWallTile(6, 1, true, list[5].tilenum, occupied: true);
            if (list[2].isWall)
            {
                t = isometricTilemap.SetWallTile(0, 1, true, list[2].tilenum, occupied: true);
                t = isometricTilemap.SetWallTile(0, 1, true, list[5].tilenum, occupied: true);
            }
            if (list[8].isWall)
            {
                t = isometricTilemap.SetWallTile(0, 1, true, list[8].tilenum, occupied: true);
                t = isometricTilemap.SetWallTile(0, 1, true, list[5].tilenum, occupied: true);
            }
            if (list[4].isWall)
			{
                t = isometricTilemap.SetWallTile(1, 1, true, list[4].tilenum, occupied: true);
                t = isometricTilemap.SetWallTile(1, 1, true, list[5].tilenum, occupied: true);
            }
			if (list[6].isWall)
			{
                t = isometricTilemap.SetWallTile(1, 1, true, list[6].tilenum, occupied: true);
                t = isometricTilemap.SetWallTile(1, 1, true, list[5].tilenum, occupied: true);
            }
            
        }
        private void FormatWall(Dictionary<int, Tile> list)
        {
			//bottom corner
			if (list[4].isWall && list[2].isWall && list[5].tilenum != 2)
			{
				var t = isometricTilemap.SetWallTile(2, 1, true, list[5].tilenum, occupied: true);
			}
            // left corner
            if (list[2].isWall && list[6].isWall && list[5].tilenum != 3)
            {
                var t = isometricTilemap.SetWallTile(3, 1, true, list[5].tilenum, occupied: true);
            }
            // right corner
            if (list[4].isWall && list[8].isWall && list[5].tilenum != 4)
            {
                var t = isometricTilemap.SetWallTile(4, 1, true, list[5].tilenum, occupied: true);
            }
            // top corner
            if (list[6].isWall && list[8].isWall && list[5].tilenum != 5)
            {
                var t = isometricTilemap.SetWallTile(5, 1, true, list[5].tilenum, occupied: true);
            }

            //right t-section
            if (list[2].isWall && list[6].isWall && list[8].isWall && list[5].tilenum != 7)
            {
                var t = isometricTilemap.SetWallTile(7, 1, true, list[5].tilenum, occupied: true);
            }
            //down t-section
            if (list[4].isWall && list[6].isWall && list[8].isWall && list[5].tilenum != 8)
            {
                var t = isometricTilemap.SetWallTile(8, 1, true, list[5].tilenum, occupied: true);
            }
            //top t-section
            if (list[2].isWall && list[4].isWall && list[6].isWall && list[5].tilenum != 9)
            {
                var t = isometricTilemap.SetWallTile(9, 1, true, list[5].tilenum, occupied: true);
            }
            //left t-section
            if (list[2].isWall && list[4].isWall && list[8].isWall && list[5].tilenum != 10)
            {
                var t = isometricTilemap.SetWallTile(10, 1, true, list[5].tilenum, occupied: true);
            }
            //cross
            if (list[2].isWall && list[4].isWall && list[6].isWall && list[8].isWall && list[5].tilenum != 11)
            {
                var t = isometricTilemap.SetWallTile(11, 1, true, list[5].tilenum, occupied: true);
            }

        }

        private void SelectBuilding()
		{
            if (!InputManager.mouseOverUI)
            {
                if (InputManager.WasKeyPressed(Keys.D1))
                {
                    IsBuildingSelected = true;
                    setBuilding(Building.wall);
                    current_animation = animMgr.GetAnimation("buildings_wall");
                }
                if (InputManager.WasKeyPressed(Keys.D2))
                {
                    IsBuildingSelected = true;
                    setBuilding(Building.house);
                    current_animation = animMgr.GetAnimation("buildings_house_1");
                }
                if (InputManager.WasKeyPressed(Keys.D3))
                {
                    IsBuildingSelected = true;
                    setBuilding(Building.fiftycal);
                    current_animation = animMgr.GetAnimation("buildings_fifty");
                }
                if (InputManager.WasKeyPressed(Keys.D4))
                {
                    IsBuildingSelected = true;
                    setBuilding(Building.flamethrower);
                    current_animation = animMgr.GetAnimation("buildings_flamethrower");
                }
                if (InputManager.WasKeyPressed(Keys.D5))
                {
                    IsBuildingSelected = true;
                    setBuilding(Building.howitzer);
                    current_animation = animMgr.GetAnimation("buildings_howitzer");
                }
                if (InputManager.WasKeyPressed(Keys.D6))
                {
                    IsBuildingSelected = true;
                    setBuilding(Building.spawner);
                }
            }
	}
        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsBuildingSelected)
            {
                Vector2 mouse = InputManager.GetMouseState().Position.ToVector2();
                spriteBatch.Draw(current_animation.sheet,
                            new Rectangle((int)mouse.X, (int)mouse.Y, 32 * Globals.scale, 32 * Globals.scale),
                            current_animation.currentFrame,
                            Color.White);
            }
		}
    }
}
