using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class RangerLandmarkScroll : SpellScroll
	{
		[Constructable]
		public RangerLandmarkScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerLandmarkScroll( int amount ) : base( 909, 3828, amount )
		{
			Name = "Landmark Scroll";
			Hue = 2001;
		}

		public RangerLandmarkScroll( Serial serial ) : base( serial )
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
