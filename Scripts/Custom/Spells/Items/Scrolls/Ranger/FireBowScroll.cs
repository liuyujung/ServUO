using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class RangerFireBowScroll : SpellScroll
	{
		[Constructable]
		public RangerFireBowScroll() : this( 1 )
		{
		}

		[Constructable]
		public RangerFireBowScroll( int amount ) : base( 903, 3828, amount )
		{
			Name = "Fire Bow Scroll";
			Hue = 2001;
		}

		public RangerFireBowScroll( Serial serial ) : base( serial )
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
