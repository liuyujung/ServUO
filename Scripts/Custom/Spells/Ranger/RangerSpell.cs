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

namespace Server.Spells.Ranger
{
	public abstract class RangerSpell : Spell
	{
		public abstract double CastDelay{ get; }
		public abstract double RequiredSkill{ get; }
		public abstract int RequiredMana { get; }
		public abstract SpellCircle Circle { get; }
		
		public override SkillName CastSkill{ get{ return SkillName.Archery; } }
		public override SkillName DamageSkill{ get{ return SkillName.Veterinary; } }
		
		public override bool ClearHandsOnCast{ get{ return false; } }
		
		public RangerSpell( Mobile caster, Item scroll, SpellInfo info ) : base( caster, scroll, info )
		{
		}
		
		public override void GetCastSkills( out double min, out double max )
		{
			min = RequiredSkill;
			max = RequiredSkill;
		}
		
		
		public override int GetMana()
		{
			return RequiredMana;
		}
		public override TimeSpan GetCastDelay()
		{
			return TimeSpan.FromSeconds( CastDelay );
		}

		public override TimeSpan CastDelayBase
		{
			get
			{
				return TimeSpan.FromSeconds(CastDelay);
			}
		}
	}
}

