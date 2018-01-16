using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells.Druid; 


namespace Server.Items
{
	public class EnchantedGroveScroll : SpellScroll
	{
		[Constructable]
		public EnchantedGroveScroll() : this( 1 )
		{
		}
		
		[Constructable]
		public EnchantedGroveScroll( int amount ) : base( 561, 0xE39, amount )
		{
			Name = "Enchanted Grove Scroll";
			Hue = 0x58B;
		}
		
		public EnchantedGroveScroll( Serial serial ) : base( serial )
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
