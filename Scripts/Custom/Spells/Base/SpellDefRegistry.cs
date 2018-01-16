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

namespace Server.Spells
{
	public class SpellDefRegistry
	{
		private static string[][] m_SDefs = new string[1000][]; //This should match your SpellRegistry.m_Types size

		public static string[][] SDefs{ get{ return m_SDefs; } }

		public static void Register( int spellID, string name, string des, string regs, string inf )
		{
			if( spellID < 0 || spellID >= m_SDefs.Length )
				return;

			string[] def = new string[4];
			def[0] = name;
			def[1] = des;
			def[2] = regs;
			def[3] = inf;
			m_SDefs[spellID] = def;
		}

		public static string[] GetDefFor( int spellID )
		{
			if( spellID < 0 || spellID >= m_SDefs.Length )
				return null;

			string[] s = m_SDefs[spellID];

			return s;
		}
	}
}