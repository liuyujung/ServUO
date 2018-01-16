using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells.Druid; 


namespace Server.Items
{
	public class BlendWithForestScroll : SpellScroll
	{
		[Constructable]
		public BlendWithForestScroll() : this( 1 )
		{
		}
		
		[Constructable]
		public BlendWithForestScroll( int amount ) : base( 556, 0xE39, amount )
		{
			Name = "Blend With Forest Scroll";
			Hue = 0x58B;
		}
		
		public BlendWithForestScroll( Serial serial ) : base( serial )
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
