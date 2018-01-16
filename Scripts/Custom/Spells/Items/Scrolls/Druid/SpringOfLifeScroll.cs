using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class SpringOfLifeScroll : SpellScroll
	{
		[Constructable]
		public SpringOfLifeScroll() : this( 1 )
		{
		}
		
		[Constructable]
		public SpringOfLifeScroll( int amount ) : base( 554, 0xE39, amount )
		{
			Name = "Spring Of Life Scroll";
			Hue = 0x58B;
		}
		
		public SpringOfLifeScroll( Serial serial ) : base( serial )
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

