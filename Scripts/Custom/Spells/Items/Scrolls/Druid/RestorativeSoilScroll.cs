using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class RestorativeSoilScroll : SpellScroll
	{
		[Constructable]
		public RestorativeSoilScroll() : this( 1 )
		{
		}
		
		[Constructable]
		public RestorativeSoilScroll( int amount ) : base( 565, 0xE39, amount )
		{
			Name = "Restorative Soil Scroll";
			Hue = 0x58B;
		}
		
		public RestorativeSoilScroll( Serial serial ) : base( serial )
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
