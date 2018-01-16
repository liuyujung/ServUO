using System;
using Server;
using Server.Network;
using Server.Spells.Cleric; 


namespace Server.Items
{
	public class ClericSmiteScroll : SpellScroll
	{
		[Constructable]
		public ClericSmiteScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericSmiteScroll( int amount ) : base( 810, 0xE39, amount )
		{
			Name = "Smite Scroll";
			Hue = 0x1F0;
		}

		public ClericSmiteScroll( Serial serial ) : base( serial )
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
