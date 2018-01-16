using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class RangerWallOfAirScroll : SpellScroll
	{
		[Constructable]
		public RangerWallOfAirScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerWallOfAirScroll( int amount ) : base( 911, 3828, amount )
		{
			Name = "Wall Of Air Scroll";
			Hue = 2001;
		}

		public RangerWallOfAirScroll( Serial serial ) : base( serial )
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
