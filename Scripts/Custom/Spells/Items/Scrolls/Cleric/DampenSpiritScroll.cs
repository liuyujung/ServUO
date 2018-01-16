using System;
using Server;
using Server.Network;
using Server.Spells.Cleric; 


namespace Server.Items
{
	public class ClericDampenSpiritScroll : SpellScroll
	{
		[Constructable]
		public ClericDampenSpiritScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericDampenSpiritScroll( int amount ) : base( 803, 0xE39, amount )
		{
			Name = "Dampen Spirit Scroll";
			Hue = 0x1F0;
		}

		public ClericDampenSpiritScroll( Serial serial ) : base( serial )
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
