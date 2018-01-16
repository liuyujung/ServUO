using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells.Druid; 


namespace Server.Items
{
	public class MushroomGatewayScroll : SpellScroll
	{
		[Constructable]
		public MushroomGatewayScroll() : this( 1 )
		{
		}
		
		[Constructable]
		public MushroomGatewayScroll( int amount ) : base( 564, 0xE39, amount )
		{
			Name = "Mushroom Gateway Scroll";
			Hue = 0x58B;
		}
		
		public MushroomGatewayScroll( Serial serial ) : base( serial )
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
