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

namespace Server.Spells.Druid
{
	public class DruidInitializer
	{
		public static void Initialize()
		{
			if( Core.AOS )
			{
				Register( 551, typeof( LeafWhirlwindSpell ) );
				Register( 552, typeof( HollowReedSpell ) );
				Register( 553, typeof( PackOfBeastSpell ) );
				Register( 554, typeof( SpringOfLifeSpell ) );
				Register( 555, typeof( GraspingRootsSpell ) );
				Register( 556, typeof( BlendWithForestSpell ) );
				Register( 557, typeof( SwarmOfInsectsSpell ) );
				Register( 558, typeof( VolcanicEruptionSpell ) );
			 	Register( 559, typeof( DruidFamiliarSpell ) );
			 	Register( 560, typeof( StoneCircleSpell ) );
				Register( 561, typeof( EnchantedGroveSpell ) );
				Register( 562, typeof( LureStoneSpell ) );
				Register( 563, typeof( NaturesPassageSpell ) );
				Register( 564, typeof( MushroomGatewaySpell ) );
				Register( 565, typeof( RestorativeSoilSpell ) );
				Register( 566, typeof( ShieldOfEarthSpell ) );
				Register( 567, typeof( BarkSkinSpell ) );
				Register( 568, typeof( DelayedPoisonSpell ) );
				Register( 569, typeof( GoodBerrySpell ) );

				//RegDef( spellID, "Name", "Description", "Reagent1; Reagent2; Reagentn", "Skill; Mana; Tithe; Etc" );
				RegDef( 551, "Leaf Whirlwind", "A gust of wind blows picking up magic leaves that memorize where they have come from, marking a rune for the caster.", "Spring Water; Petrafied Wood; Destroying Angel", "Skill: 1; Mana: 10" );
				RegDef( 552, "Hollow Reed", "Increases both the strength and the intelligence of the Druid.", "Bloodmoss; Mandrake Root; Nightshade", "Skill: 30; Mana: 30" );
				RegDef( 553, "Pack Of Beasts", "Summons a pack of beasts to fight for the Druid. Spell length increases with skill.", "Bloodmoss; Black Pearl; Petrified Wood", "Skill: 40; Mana: 45" );
				RegDef( 554, "Spring Of Life", "Creates a magical spring that heals the Druid and their party.", "Spring Water", "Skill: 40; Mana: 40" );
				RegDef( 555, "Grasping Roots", "Summons roots from the ground to entangle a single target.", "Bloodmoss; Spring Water; Spiders Silk", "Skill: 40; Mana: 40" );
				RegDef( 556, "Blend With Forest", "The Druid blends seamlessly with the background, becoming invisible to their foes.", "Bloodmoss; Nightshade", "Skill: 75; Mana: 60" );
				RegDef( 557, "Swarm Of Insects", "Summons a swam of insects that bite and sting the Druid's enemies.", "Garlic; Nightshade; Destroying Angel", "Skill: 85; Mana: 70" );
				RegDef( 558, "Volcanic Eruption", "A blast of molten lava bursts from the ground, hitting every enemy nearby.", "Sulfurous Ash; Destroying Angel", "Skill: 98; Mana: 85" );
				RegDef( 559, "Summon Familiar", "Summons a choice of diffrent familiars that can aid the druid.", "Mandrake Root; Spring Water", "Skill: 80; Mana: 50" );
				RegDef( 560, "Stone Circle", "Forms an impassable circle of stones, ideal for trapping enemies.", "Black Pearl; Ginseng; Spring Water", "Skill: 60; Mana: 45" );
				RegDef( 561, "Enchanted Grove", "Causes a grove of magical trees to grow.", "Petrified Wood; Mandrake Root; Spring Water", "Skill: 95; Mana: 60" );
				RegDef( 562, "Lure Stone", "Creates a magical stone that calls all nearby animals to it.", "Black Pearl; Spring Water", "Skill: 15; Mana: 30" );
				RegDef( 563, "Nature's Passage", "The Druid is turned into flower petals and carried on the wind to a recall rune location.", "Black Pearl; Bloodmoss; Mandrake Root", "Skill: 15; Mana: 10" );
				RegDef( 564, "Mushroom Gateway", "A magical circle of mushrooms opens, allowing the Druid to step through it to another location.", "Black Pearl; Spring Water; Mandrake Root", "Skill: 70; Mana: 40" );
				RegDef( 565, "Restorative Soil", "Saturates a patch of land with power, causing healing mud to seep through . The mud can restore the dead to life.", "Garlic; Ginseng; Spring Water", "Skill: 89; Mana: 55" );
				RegDef( 566, "Shield Of Earth", "A quick-growing wall of foliage springs up at the bidding of the Druid.", "Ginseng; Spring Water", "Skill: 20; Mana: 15" );
				//--<< New Spells >>-------------------*
				RegDef( 567, "Bark Skin", "Description.", "Garlic; Petrafied Wood; Mandrake Root", "Skill: 2; Mana: 10" );
				RegDef( 568, "Delayed Poison", "Description.", "Bloodmoss; Mandrake Root", "Skill: 2; Mana: 10" );
				RegDef( 569, "Good Berry", "Description.", "Ginseng; Spring Water", "Skill: 2; Mana: 10" );
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