using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class PackOfBeastScroll : SpellScroll
	{
		[Constructable]
		public PackOfBeastScroll() : this( 1 )
		{
		}
		
		[Constructable]
		public PackOfBeastScroll( int amount ) : base( 553, 0xE39, amount )
		{
			Name = "Pack Of Beast Scroll";
			Hue = 0x58B;
		}
		
		public PackOfBeastScroll( Serial serial ) : base( serial )
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
