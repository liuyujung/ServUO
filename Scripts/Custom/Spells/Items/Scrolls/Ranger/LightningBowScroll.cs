using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class RangerLightningBowScroll : SpellScroll
	{
		[Constructable]
		public RangerLightningBowScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerLightningBowScroll( int amount ) : base( 905, 3828, amount )
		{
			Name = "Lightning Bow Scroll";
			Hue = 2001;
		}

		public RangerLightningBowScroll( Serial serial ) : base( serial )
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
