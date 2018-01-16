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
using Server.Network;


namespace Server.Spells.Cleric
{
	public abstract class ClericSpell : Spell
	{
		public abstract int RequiredTithing{ get; }
		public abstract double RequiredSkill { get; }
		public abstract SpellCircle Circle { get; }
		public abstract int RequiredMana{ get; }

		public override SkillName CastSkill{ get{ return SkillName.SpiritSpeak; } }
		public override bool ClearHandsOnCast{ get{ return false; } }


		public ClericSpell( Mobile caster, Item scroll, SpellInfo info ) : base( caster, scroll, info )
		{
		}

		public override bool CheckCast()
		{
			if (Server.Spells.AllSpells.CheckRestriction(Caster, 5) == false)
				return true;
			
			if ( !base.CheckCast() )
				return false;

			if ( Caster.Skills[CastSkill].Value < RequiredSkill )
			{
				Caster.SendMessage( "You must have at least " + RequiredSkill + " Spirit Speak to invoke this prayer" );
				return false;
			}
			else if ( Caster.TithingPoints < RequiredTithing )
			{
				Caster.SendMessage( "You must have at least " + RequiredTithing + " Tithe to invoke this prayer." );
				return false;
			}
			else if ( Caster.Mana < ScaleMana( GetMana() ) )
			{
				Caster.SendMessage( "You must have at least " + GetMana() + " Mana to invoke this praryer." );
				return false;
			}

			return true;
		}

		public override bool CheckFizzle()
		{
			if ( !base.CheckFizzle() )
				return false;

			int tithing = RequiredTithing;
			double min, max;

			GetCastSkills( out min, out max );

			if ( AosAttributes.GetValue( Caster, AosAttribute.LowerRegCost ) > Utility.Random( 100 ) )
				tithing = 0;

			int mana = ScaleMana( GetMana() );

			if ( Caster.Skills[CastSkill].Value < RequiredSkill )
			{
				Caster.SendMessage( "You must have at least " + RequiredSkill + " Spirit Speak to invoke this prayer." );
				return false;
			}
			else if ( Caster.TithingPoints < tithing )
			{
				Caster.SendMessage( "You must have at least " + tithing + " Tithe to invoke this prayer." );
				return false;
			}
			else if ( Caster.Mana < mana )
			{
				Caster.SendMessage( "You must have at least " + mana + " Mana to invoke this prayer." );
				return false;
			}

			Caster.TithingPoints -= tithing;

			return true;
		}

		public override void SayMantra()
		{
			Caster.PublicOverheadMessage( MessageType.Regular, 0x3B2, false, Info.Mantra );
			Caster.PlaySound( 0x24A );
		}

		public override void DoFizzle()
		{
			Caster.PlaySound( 0x1D6 );
            Caster.NextSpellTime = DateTime.Now.Ticks;
		}

		public override void DoHurtFizzle()
		{
			Caster.PlaySound( 0x1D6 );
		}

		public override void OnDisturb( DisturbType type, bool message )
		{
			base.OnDisturb( type, message );

			if ( message )
				Caster.PlaySound( 0x1D6 );
		}

		public override void OnBeginCast()
		{
			base.OnBeginCast();

			Caster.FixedEffect( 0x37C4, 10, 42, 4, 3 );
		}

		public override void GetCastSkills( out double min, out double max )
		{
			min = RequiredSkill;
			max = RequiredSkill + 40.0;
		}

		public override int GetMana()
		{
            return RequiredMana;
		}

		public override TimeSpan CastDelayBase
		{
			get
			{
                return TimeSpan.Zero;
			}
		}
	}
}
