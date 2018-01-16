using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class RangerNaturalHerbScroll : SpellScroll
	{
		[Constructable]
		public RangerNaturalHerbScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerNaturalHerbScroll( int amount ) : base( 908, 3828, amount )
		{
			Name = "Natural Herb Scroll";
			Hue = 2003;
		}

		public RangerNaturalHerbScroll( Serial serial ) : base( serial )
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
