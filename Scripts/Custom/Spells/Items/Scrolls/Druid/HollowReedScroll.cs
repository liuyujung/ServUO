using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells.Druid; 


namespace Server.Items
{
	public class HollowReedScroll : SpellScroll
	{
		[Constructable]
		public HollowReedScroll() : this( 1 )
		{
		}
		
		[Constructable]
		public HollowReedScroll( int amount ) : base( 552, 0xE39, amount )
		{
			Name = "Hollow Reed Scroll";
			Hue = 0x58B;
		}
		
		public HollowReedScroll( Serial serial ) : base( serial )
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
