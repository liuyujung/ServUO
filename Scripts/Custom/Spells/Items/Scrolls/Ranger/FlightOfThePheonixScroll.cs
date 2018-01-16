using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class RangerFlightOfThePheonixScroll : SpellScroll
	{
		[Constructable]
		public RangerFlightOfThePheonixScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerFlightOfThePheonixScroll( int amount ) : base( 901, 3828, amount )
		{
			Name = "Flight of the Pheonix Scroll";
			Hue = 2002;
		}

		public RangerFlightOfThePheonixScroll( Serial serial ) : base( serial )
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
	}
}
