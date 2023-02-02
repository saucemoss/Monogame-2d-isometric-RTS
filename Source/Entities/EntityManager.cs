using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProject.Source.Entities
{
	class EntityManager
	{
		public List<Entity> entities;

		private static EntityManager instance;
		public static EntityManager GetInstance()
		{
			if (instance == null)
			{
				instance = new EntityManager();
			}
			return instance;
		}

		EntityManager()
		{
			entities = new List<Entity>();
		}

		public void Add(Entity entity)
		{
			entities.Add(entity);
		}
		public void Remove(Entity entity)
		{
			entities.Remove(entity);
		}
		public void Uptade(GameTime gameTime)
		{
			foreach (Entity e in entities.ToList()) {
				e.Update(gameTime);
			}

		}


	}
}
