using GameProject.Source.Inputs;
using GameProject.Source.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
namespace GameProject.Source.Main
{
	public class CollisionManager
	{
		public List<ICollidable> collidables;
		public List<List<ICollidable>> collidablesArranged;
		private static CollisionManager Instance;
		private List<RectangleF> cells;
		public int gridHeight = 2500;
		public int gridWidth = 3000;
		public int gridWidthOffset = 1000;
		static public int divider = 50;
		public float factor = 0.1f;


		public static CollisionManager GetInstance()
		{
			if (Instance == null)
			{
				Instance = new CollisionManager();
			}
			return Instance;
		}

		private CollisionManager()
		{
			initSpatialDivision();
			collidables = new List<ICollidable>();
		}
		public void initSpatialDivision()
		{
			collidablesArranged = new List<List<ICollidable>>();
			cells = new List<RectangleF>();
			int dimx = gridWidth / divider;
			int dimy = gridHeight / divider;
			//Debug.WriteLine(dimx + ", " + dimy);
			for (int y = 0; y < dimy; y++)
			{
				for (int x = 0; x < dimx; x++)
				{
					collidablesArranged.Add(new List<ICollidable>());
					cells.Add(new RectangleF(x * divider - gridWidthOffset, y * divider, divider, divider));
				}
			}
		}

		public List<int> GetNeighbouringCellIndexes(int cell)
		{
			List<int> nCells = new List<int>();
			nCells.Add(cell - (gridWidth / divider) - 1);
			nCells.Add(cell - (gridWidth / divider));
			nCells.Add(cell - (gridWidth / divider) + 1 );
			nCells.Add(cell - 1);
			nCells.Add(cell);
			nCells.Add(cell + 1);
			nCells.Add(cell + (gridWidth / divider) - 1);
			nCells.Add(cell + (gridWidth / divider));
			nCells.Add(cell + (gridWidth / divider) + 1);

			return nCells;
		}

		public int GetCellIndex(ICollidable b)
		{
			int tx = (int)((b.x + b.w / 2 + gridWidthOffset) / divider);
			int ty = (int)((b.y + b.h / 2) / divider);
			int tw = gridWidth / divider;
			int cell = (int)(ty * tw + tx);

			return cell;
		}

		public void AssignStaticCollidersToSpatialDivison(ICollidable b)
		{
			for (int i = 0; i < cells.Count; i++)
			{
				if (b.drawRectangle.Intersects(cells[i]))
				{
					collidablesArranged[i].Add(b);
				}
			}

		}
		public void AssignToSpatialDivision(ICollidable b)
		{
			List<int> neighbourCellsIndexes = GetNeighbouringCellIndexes(GetCellIndex(b));


			for (int i = 0; i < neighbourCellsIndexes.Count; i++)
			{
				if (b.drawRectangle.Intersects(cells[neighbourCellsIndexes[i]]) && !collidablesArranged[neighbourCellsIndexes[i]].Contains(b))
				{
					collidablesArranged[neighbourCellsIndexes[i]].Add(b);
				}
				else if (!b.drawRectangle.Intersects(cells[neighbourCellsIndexes[i]]) && collidablesArranged[neighbourCellsIndexes[i]].Contains(b))
				{
					collidablesArranged[neighbourCellsIndexes[i]].RemoveAll(box => box.Equals(b));
				}
			}


		}
		public void Add(ICollidable c)
		{
			collidables.Add(c);
		}
		public void Remove(ICollidable c)
		{
			collidables.Remove(c);
			for (int i = 0; i < collidablesArranged.Count; i++)
			{
				collidablesArranged[i].Remove(c);
			}
		}

		public void Update(GameTime gameTime)
		{

			if (InputManager.WasKeyPressed(Keys.D1))
			{
				Debug.WriteLine(collidables.Count);

				//Debug.WriteLine(GetCellIndex(collidables[collidables.Count - 1]));
				//Debug.WriteLine(collidables[collidables.Count - 1].x);

			}

			if (InputManager.IsKeyPressed(Keys.Q))
			{
				for (int i = 0; i < collidables.Count; i++)
				{
					if (collidables[i].colliding)
					{
						collidables.RemoveAt(i);
					}
				}
				initSpatialDivision();
			}


		}
		public bool WillAABBSpatialCollide(ICollidable b, Vector2 m)
		{
			foreach (int n in GetNeighbouringCellIndexes(GetCellIndex(b)))
			{
				foreach (ICollidable o in collidablesArranged[n])
				{
					if (!b.Equals(o) &&
						(b.x + m.X < o.x + o.w &&
						b.x + m.X + b.w > o.x &&
						b.y + m.Y < o.y + o.h &&
						b.y + m.Y + b.h > o.y))
					{
						return true;
					}
				}
			}

			return false;
		}

		public bool WillAABBSimpleCollide(ICollidable b, Vector2 m)
		{
			foreach (ICollidable o in collidables)
			{
				if (!o.Equals(b))
				{
					if (b.x + m.X < o.x + o.w &&
						b.x + m.X + b.w > o.x &&
						b.y + m.Y < o.y + o.h &&
						b.y + m.Y + b.h > o.y)
					{
						return true;
					}
				}
			}
			return false;
		}

		public RectangleF GetCollisionIntersection(ICollidable b)
		{

			foreach (int n in GetNeighbouringCellIndexes(GetCellIndex(b)))
			{
				foreach (ICollidable o in collidablesArranged[n])
				{
					if (!o.Equals(b) && b.drawRectangle.Intersects(o.drawRectangle))
					{
						return b.drawRectangle.Intersection(o.drawRectangle);
					}
				}
			}
			return new RectangleF(0,0,0,0);
		}


		public void Draw(SpriteBatch spritebatch, GameTime gameTime)
		{


/*			for (int i = 0; i < cells.Count; i++)
			{
				Color c = new Color(collidablesArranged[i].Count * factor, collidablesArranged[i].Count * factor, collidablesArranged[i].Count * factor, 0.1f);
				spritebatch.DrawRectangle(cells[i], c);
				spritebatch.DrawString(DebugInterface.font, "" + i, new Vector2(cells[i].X, cells[i].Y), Color.Red);
			}*/

/*			foreach (ICollidable c in collidables)
			{
				c.Draw(spritebatch, gameTime);
			}*/

		}
	}
}
