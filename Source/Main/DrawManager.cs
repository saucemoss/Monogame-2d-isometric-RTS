using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProject.Source.Main
{
	class DrawManager
	{
		public List<List<IDraw>> layers;
		public List<IDraw> layerOne;
		public List<IDraw> layerTwo;
		public List<IDraw> layerThree;
		public List<IDraw> layerFour;
		private static DrawManager instance;
		public static DrawManager GetInstance()
		{
			if (instance == null)
			{
				instance = new DrawManager();
			}
			return instance;
		}
		public DrawManager()
		{
			layers = new List<List<IDraw>>();
			layerOne = new List<IDraw>();
			layerTwo = new List<IDraw>();
			layerThree = new List<IDraw>();
			layerFour = new List<IDraw>();
			layers.Add(layerOne);
			layers.Add(layerTwo);
			layers.Add(layerThree);
			layers.Add(layerFour);

		}

		public void Add(IDraw obj, int layer)
		{
			layers[layer].Add(obj);
		}

		public void Remove(IDraw obj, int layer)
		{
			layers[layer].Remove(obj);
		}

		public void Update()
		{
			//layerTwo = layerTwo.OrderBy(o => o.getPosition.Y).ToList();
		}

		public void Draw()
		{
			//ground
			layerOne.ForEach(obj => obj.Draw(Globals.spriteBatch));
			//objects
			var SortedLayerTwo = layerTwo.OrderBy(o => o.getDrawPosition.Y).ToList();
			SortedLayerTwo.ForEach(obj => obj.Draw(Globals.spriteBatch));

			var SortedLayerThree = layerThree.OrderBy(o => o.getDrawPosition.Y).ToList();
			SortedLayerThree.ForEach(obj => obj.Draw(Globals.spriteBatch));

			var SortedLayerFour = layerFour.OrderBy(o => o.getDrawPosition.Y).ToList();
			SortedLayerFour.ForEach(obj => obj.Draw(Globals.spriteBatch));
		}

	}
}
