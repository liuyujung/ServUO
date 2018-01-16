using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class RangerTreeStrideScroll : SpellScroll
	{
		[Constructable]
		public RangerTreeStrideScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerTreeStrideScroll( int amount ) : base( 910, 3828, amount )
		{
			Name = "Tree Stride Scroll";
			Hue = 2001;
		}

		public RangerTreeStrideScroll( Serial serial ) : base( serial )
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
