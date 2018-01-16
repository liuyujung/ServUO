using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Ranger
{
	public class RangerSummonMountSpell : RangerSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		                                                "Call Mount", "*Blows Decreative Flute*",
		                                                266,
		                                                9040,
		                                                Reagent.SpringWater,
		                                                Reagent.BlackPearl,
		                                                Reagent.SulfurousAsh
		                                               );

		public override double CastDelay{ get{ return 2.0; } }
		public override int RequiredMana{ get{ return 15; } }
		public override double RequiredSkill{ get{ return 30; } }

		public RangerSummonMountSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override SpellCircle Circle
		{
			get
			{
                return SpellCircle.Fifth;
			}
		}

		private static Type[] m_Types = new Type[]
		{
			typeof( ForestOstard ),
			typeof( DesertOstard ),
			typeof( Ridgeback ),
			typeof( ForestOstard ),
			typeof( Horse ),
			typeof( DesertOstard ),
			typeof( Horse ),
			typeof( Ridgeback ),
			typeof( SwampDragon ),
			typeof( RidableLlama ),
			typeof( RidableLlama ),
			typeof( Horse )
		};

		public override bool CheckCast()
		{
			if (Server.Spells.AllSpells.CheckRestriction(Caster, 6) == false) 
				return true;

			if ( !base.CheckCast() )
				return false;

			if ( (Caster.Followers + 1) > Caster.FollowersMax )
			{
				Caster.SendLocalizedMessage( 1049645 ); // You have too many followers to summon that creature.
				return false;
			}
			Caster.PlaySound( 0x3D );
			return true;
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				try
				{
					BaseCreature creature = (BaseCreature)Activator.CreateInstance( m_Types[Utility.Random( m_Types.Length )] );

					creature.ControlSlots = 1;

					TimeSpan duration;

					if ( Core.AOS )
						duration = TimeSpan.FromSeconds( (2 * Caster.Skills.AnimalLore.Fixed) / 5 );
					else
						duration = TimeSpan.FromSeconds( 4.0 * Caster.Skills[SkillName.AnimalLore].Value );

					SpellHelper.Summon( creature, Caster, 0x215, duration, false, false );
				}
				catch
				{
				}
			}

			FinishSequence();
		}


	}
}
