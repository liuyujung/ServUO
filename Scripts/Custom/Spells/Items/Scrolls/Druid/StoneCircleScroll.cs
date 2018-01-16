using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class StoneCircleScroll : SpellScroll
	{
		[Constructable]
		public StoneCircleScroll() : this( 1 )
		{
		}
		
		[Constructable]
		public StoneCircleScroll( int amount ) : base( 560, 0xE39, amount )
		{
			Name = "Stone Circle Scroll";
			Hue = 0x58B;
		}
		
		public StoneCircleScroll( Serial serial ) : base( serial )
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
