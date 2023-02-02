using GameProject.Source.Main;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace GameProject.Source.Entities.Objects.Projectiles
{
	abstract class Projectile : Entity
	{
		public Projectile(Vector2 POS, Vector2 DIMS, string PATH = "placeholder") : base(POS, DIMS, PATH)
		{
			DrawManager.GetInstance().Remove(this, 1);
			DrawManager.GetInstance().Add(this, 2);
		}
		public override bool OnCollision(Fixture fixture, Fixture other, Contact contact)
		{
			return true;
		}
	}
}
