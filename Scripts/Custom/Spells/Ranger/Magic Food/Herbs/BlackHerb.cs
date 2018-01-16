using System;
using Server;

namespace Server.Items
{
	public class BlackHerb : BaseMagicFood
	{
		[Constructable]
		public BlackHerb() : base( 0xC7B, MagicFoodEffect.Nightsight )
		{
			Weight = 1.0;
			Name = "black herb";
			Hue = 902;
		}

		public BlackHerb( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void Drink( Mobile from )
		{
			if ( from.BeginAction( typeof( LightCycle ) ) )
			{
				new LightCycle.NightSightTimer( from ).Start();
				from.LightLevel = LightCycle.DungeonLevel / 2;

				from.FixedParticles( 0x376A, 9, 32, 5007, EffectLayer.Waist );
				from.PlaySound( 0x1E3 );

				BaseMagicFood.PlayDrinkEffect( from );

				this.Delete();
			}
			else
			{
				from.SendMessage( "You already have nightsight." );
			}
		}
	}
}