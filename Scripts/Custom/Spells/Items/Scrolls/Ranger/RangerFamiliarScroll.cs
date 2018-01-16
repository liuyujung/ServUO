using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class RangerFamiliarScroll : SpellScroll
	{
		[Constructable]
		public RangerFamiliarScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerFamiliarScroll( int amount ) : base( 902, 3828, amount )
		{
			Name = "Animal Companion Scroll";
			Hue = 2001;
		}

		public RangerFamiliarScroll( Serial serial ) : base( serial )
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
