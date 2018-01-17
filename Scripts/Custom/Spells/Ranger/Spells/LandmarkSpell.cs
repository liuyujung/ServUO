using System;
using Server.Items;
using Server.Targeting;
using Server.Network;
using Server.Regions;
using Server.Spells.Ranger;

namespace Server.Spells.Ranger
{
	public class RangerLandmarkSpell : RangerSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Landmark", "Sinta Nat'",
		                                                218,
		                                                9002,
		                                                Reagent.SulfurousAsh,
		                                                Reagent.BlackPearl
		                                               );
		
		public override double CastDelay{ get{ return 3.0; } }
		public override int RequiredMana{ get{ return 25; } }
		public override double RequiredSkill{ get{ return 50; } }
		
		public RangerLandmarkSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override SpellCircle Circle
		{
			get
			{
				return SpellCircle.Sixth;
			}
		}
		
		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}
		
		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;
			
			return SpellHelper.CheckTravel( Caster, TravelCheckType.Mark );
		}
		
		public void Target( RecallRune rune )
		{
			if ( !Caster.CanSee( rune ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( !SpellHelper.CheckTravel( Caster, TravelCheckType.Mark ) )
			{
			}
			else if ( SpellHelper.CheckMulti( Caster.Location, Caster.Map, !Core.AOS ) )
			{
				Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
			}
			else if ( !rune.IsChildOf( Caster.Backpack ) )
			{
				Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1062422 ); // You must have this rune in your backpack in order to mark it.
			}
			else if ( CheckSequence() )
			{
				rune.Mark( Caster );
				Caster.PlaySound( 578 );
			}
			
			FinishSequence();
		}
		
		private class InternalTarget : Target
		{
			private RangerLandmarkSpell m_Owner;
			
			public InternalTarget( RangerLandmarkSpell owner ) : base( 12, false, TargetFlags.None )
			{
				m_Owner = owner;
			}
			
			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is RecallRune )
				{
					m_Owner.Target( (RecallRune) o );
				}
				else
				{
					from.Send( new MessageLocalized( from.Serial, from.Body, MessageType.Regular, 0x3B2, 3, 501797, from.Name, "" ) ); // I cannot mark that object.
				}
			}
			
			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
