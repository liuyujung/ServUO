/*
Special thanks to Ryan.
With RunUO we now have the ability to become our own Richard Garriott.
All Spells System created by x-SirSly-x, Admin of Land of Obsidian.
All Spells System 4.0 created & supported by Lucid Nagual, Admin of The Conjuring.
All Spells System 5.0 created by A_li_N.
All Spells Optional Restrictive System created by Alien, Daat99 and Lucid Nagual.
    _________________________________
 -=(_)_______________________________)=-
   /   .   .   . ____  . ___      _/
  /~ /    /   / /     / /   )2005 /
 (~ (____(___/ (____ / /___/     (
  \ ----------------------------- \
   \     lucidnagual@gmail.com     \
    \_     ===================      \
     \   -Admin of "The Conjuring"-  \
      \_     ===================     ~\
       )       All Spells System       )
      /~     Version [5].0 & Above   _/
    _/_______________________________/
 -=(_)_______________________________)=-
 */
using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells.Cleric
{
	public class PlayerEvent
	{
		public delegate void OnWeaponHit( Mobile attacker, Mobile defender, int damage, WeaponAbility a );
		public static event OnWeaponHit HitByWeapon;
		
		public static void InvokeHitByWeapon( Mobile attacker, Mobile defender, int damage, WeaponAbility a )
		{
			if ( HitByWeapon != null )
				HitByWeapon( attacker, defender, damage, a );
		}
	}
}
