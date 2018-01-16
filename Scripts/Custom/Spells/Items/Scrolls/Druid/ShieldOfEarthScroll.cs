using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ShieldOfEarthScroll : SpellScroll
	{
		[Constructable]
		public ShieldOfEarthScroll() : this( 1 )
		{
		}
		
		[Constructable]
		public ShieldOfEarthScroll( int amount ) : base( 566, 0xE39, amount )
		{
			Name = "Shield Of Earth Scroll";
			Hue = 0x58B;
		}
		
		public ShieldOfEarthScroll( Serial serial ) : base( serial )
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
