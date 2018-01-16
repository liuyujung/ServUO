using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class RangerHuntersAimScroll : SpellScroll
	{
		[Constructable]
		public RangerHuntersAimScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerHuntersAimScroll( int amount ) : base( 900, 3828, amount )
		{
			Name = "Hunter's Aim Scroll";
			Hue = 2001;
		}

		public RangerHuntersAimScroll( Serial serial ) : base( serial )
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
