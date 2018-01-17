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
using Server.Spells;

namespace Server.Spells.Cleric
{
	public class ClericInitializer
	{
		public static void Initialize()
		{
			if( Core.AOS )
			{
				Register( 801, typeof( AngelicFaithSpell ) );
				Register( 802, typeof( BanishEvilSpell ) );
				Register( 803, typeof( DampenSpiritSpell ) );
				Register( 804, typeof( DivineFocusSpell ) );
				Register( 805, typeof( HammerOfFaithSpell ) );
				Register( 806, typeof( PurgeSpell ) );
				Register( 807, typeof( RestorationSpell ) );
				Register( 808, typeof( SacredBoonSpell ) );
				Register( 809, typeof( SacrificeSpell ) );
				Register( 810, typeof( SmiteSpell ) );
				Register( 811, typeof( TouchOfLifeSpell ) );
				Register( 812, typeof( TrialByFireSpell ) );
				
				//RegDef( spellID, "Name", "Description", "Reagent1; Reagent2; Reagentn", "Skill; Mana; Tithe; Etc" );
				RegDef( 801, "Angelic Faith",   "The caster calls upon the divine powers of the heavens to transform himself into a holy angel.  The caster gains better regeneration rates and increased stats and skills.", null, "Skill: 80; Tithing: 100" );
				RegDef( 802, "Banish Evil",     "The caster calls forth a divine fire to banish his undead or demonic foe from the earth.", null, "Skill: 60; Tithing: 30" );
				RegDef( 803, "Dampen Spirit",   "The caster's enemy is slowly drained of his stamina, greatly hindering their ability to fight in combat or flee.", null, "Skill: 35; Tithing: 15" );
				RegDef( 804, "Divine Focus",    "The caster's mind focuses on his divine faith increasing the effect of his prayers.  However, the caster becomes mentally fatigued much faster.", null, "Skill: 35; Tithing: 15" );
				RegDef( 805, "Hammer Of Faith", "Summons forth a divine hammer of pure energy blessed with the ability to vanquish undead foes with greater efficiency.", null, "Skill: 40; Tithing: 20" );
				RegDef( 806, "Purge",           "The target is cured of all poisons and has all negative stat curses removed.", null, "Skill: 10; Tithing: 5" );
				RegDef( 807, "Restoration",     "The caster's target is resurrected and fully healed and refreshed.", null, "Skill: 50; Tithing: 40" );
				RegDef( 808, "Sacred Boon",     "The caster's target is surrounded by a divine wind that heals small amounts of lost life over time.", null, "Skill: 25; Tithing: 15" );
				RegDef( 809, "Sacrifice",       "The caster sacrifices himself for his allies. Whenever damaged, all party members are healed a small percent of the damage dealt. The caster still takes damage.", null, "Skill: 5; Tithing: 5" );
				RegDef( 810, "Smite",           "The caster calls to the heavens to send a deadly bolt of lightning to strike down his opponent.", null, "Skill: 80; Tithing: 60" );
				RegDef( 811, "Touch Of Life",   "The caster's target is healed by the heavens for a significant amount.", null, "Skill: 30; Tithing: 10" );
				RegDef( 812, "Trial By Fire",   "The caster is surrounded by a divine flame that damages the caster's enemy when hit by a weapon.", null, "Skill: 45; Tithing: 25" );
			}		
		}
		
		public static void Register( int spellID, Type type )
		{
			SpellRegistry.Register( spellID, type );
		}
		
		public static void RegDef( int spellID, string name, string des, string regs, string inf )
		{
			SpellDefRegistry.Register( spellID, name, des, regs, inf );
		}
	}
}
