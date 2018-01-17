using System; 
using System.Collections;
using Server; 
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Multis;
using Server.Misc; 
using Server.Regions;
using Server.Gumps;
using Server.Spells.Druid; 

namespace Server.Spells.Druid
{
	public class GoodBerrySpell : DruidSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Good Berry", "In Mani Oum",
				224,
				9011,
				Reagent.Ginseng,
				Reagent.SpringWater
			);

		public override double CastDelay{ get{ return 2.0; } } 
      		public override double RequiredSkill{ get{ return 2.0; } } 	
      		public override int RequiredMana{ get{ return 10; } } 

		public GoodBerrySpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override SpellCircle Circle
		{
			get
			{
                return SpellCircle.First;
			}
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				Item berry = new GoodBerry();

				Caster.AddToBackpack( berry );
					
				Caster.SendMessage( "you created a good berry." );

				Caster.FixedParticles( 0, 10, 5, 2003, EffectLayer.RightHand );
				Caster.PlaySound( 0x1E2 );
				
			}

			FinishSequence();
		}
	}

}