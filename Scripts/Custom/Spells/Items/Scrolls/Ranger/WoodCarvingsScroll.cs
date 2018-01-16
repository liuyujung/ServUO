using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class RangerWoodCarvingsScroll : SpellScroll
	{
		[Constructable]
		public RangerWoodCarvingsScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerWoodCarvingsScroll( int amount ) : base( 912, 3828, amount )
		{
			Name = "Wood Carvings Scroll";
			Hue = 2001;
		}

		public RangerWoodCarvingsScroll( Serial serial ) : base( serial )
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
