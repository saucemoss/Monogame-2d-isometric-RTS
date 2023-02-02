using System;
using System.Collections.Generic;
using System.Text;

namespace GameProject.Source.Entities.Objects.Weapons
{
	enum Weapon
	{
		unequipped, fireaxe, flamethrower
	}

	static class Weapons
	{
	
		public static Weapon weapon = Weapon.unequipped;

		public static void setWeapon(Weapon next_weapon) => weapon = next_weapon;

		public static void nextWeapon()
		{
			switch (weapon)
			{
				case Weapon.unequipped:
					weapon = Weapon.fireaxe;
					break;
				case Weapon.fireaxe:
					weapon = Weapon.flamethrower;
					break;
				case Weapon.flamethrower:
					weapon = Weapon.unequipped;
					break;
			}

		}


	}


}
