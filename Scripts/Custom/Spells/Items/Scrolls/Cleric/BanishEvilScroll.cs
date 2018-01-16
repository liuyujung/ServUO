using System;
using Server;
using Server.Network;
using Server.Spells.Cleric; 


namespace Server.Items
{
	public class ClericBanishEvilScroll : SpellScroll
	{
		[Constructable]
		public ClericBanishEvilScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericBanishEvilScroll( int amount ) : base( 802, 0xE39, amount )
		{
			Name = "Banish Evil Scroll";
			Hue = 0x1F0;
		}

		public ClericBanishEvilScroll( Serial serial ) : base( serial )
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
