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


namespace Server.Spells.Ranger
{
	public class RangerInitializer
	{
		public static void Initialize()
		{
			if( Core.AOS )
			{
				Register( 900, typeof( HuntersAimSpell ) );
				Register( 901, typeof( FlightOfThePheonixSpell ) );
				Register( 902, typeof( FamiliarSpell ) );
				Register( 903, typeof( FireBowSpell ) );
				Register( 904, typeof( IceBowSpell ) );
				Register( 905, typeof( LightningBowSpell ) );
				Register( 906, typeof( NoxBowSpell ) );
				Register( 907, typeof( SummonMountSpell ) );
				Register( 908, typeof( NaturalHerbSpell ) );
				Register( 909, typeof( LandmarkSpell ) );
				Register( 910, typeof( TreeStrideSpell ) );
				Register( 911, typeof( WallOfAirSpell ) );
				Register( 912, typeof( WoodCarvingsSpell ) );

				//RegDef( spellID, "Name", "Description", "Reagent1; Reagent2; Reagentn", "Skill; Mana; Tithe; Etc" );
				RegDef( 900, "Hunter's Aim", "Increases the Rangers archery, and tactics for a short period of time.", "Nightshade; Spring Water; Bloodmoss", "Skill: 50; Mana: 25" );
				RegDef( 901, "Flight of the Pheonix", "Calls Forth a Phoenix who will carry you to the location of your choice.", "Sulfuous Ash; Petrafied Wood", "Skill: 30; Mana: 30" );
				RegDef( 902, "Animal Companion", "The Ranger summons an animal companion (baised on skill level) to aid him in his quests.", "Destroying Angel; Spring Water; Petrafied Wood", "Skill: 30; Mana: 17" );
				RegDef( 903, "Fire Bow", "The Ranger uses his knowlage of archery and hunting, to craft a temparary fire elemental bow, that last for a short duration.", "Kindling; Sulfurous Ash", "Skill: 85; Mana: 30" );
				RegDef( 904, "Ice Bow", "The Ranger uses his knowlage of archery and hunting, to craft a temparary ice elemental bow, that last for a short duration.", "Kindling; Spring Water", "Skill: 85; Mana 30" );
				RegDef( 905, "Lightning Bow", "The Ranger uses his knowlage of archery and hunting, to craft a temparary lightning elemental bow, that last for a short duration.", "Kindling; Black Pearl", "Skill: 75; Mana: 60" );
				RegDef( 906, "Nox Bow", "The Ranger uses his knowlage of archery and hunting, to craft a temparary poison elemental bow, that last for a short duration.", "Kindling; Nightshade", "Skill: 85; Mana: 70" );
				RegDef( 907, "Call Mount", "The Ranger calls to the Wilds, summoning a speedy mount to his side.", "Spring Water; Black Pearl; Sulfurous Ash", "Skill: 30; Mana: 15" );
				RegDef( 908, "Natural Herb", "The Ranger searches the area around him looking for any herbs. These herbs can be anything from night sight to herbs of life which may bring one person back from the dead.", "Spring Water; Garlic; Destroying Angel", "Skill: 60; Mana: 15" );
				RegDef( 909, "Landmark", "Studying the area, the ranger is able to mark a rune with his location, for later a later return.", "Black Pearl; Sulfurous Ash", "Skill: 50; Mana: 25" );
				RegDef( 910, "Tree Stride", "This will open a woodland gate of the rangers choosing, to another location.", "Kindling; Genseng, Destroying Angel", "Skill: 80; Mana: 40" );
				RegDef( 911, "Wall of Air", "The ranger creates a wall of electrical air that damages anyone who passes through it.", "Black Pearl; Nightshade; Destroying Angel; Black Pearl", "Skill: 50; Mana: 25" );
				RegDef( 912, "Wood Carvings", "Using his blade, the Ranger carves an item from wood.", "Kindling", "Skill: 50; Mana: 10" );
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
