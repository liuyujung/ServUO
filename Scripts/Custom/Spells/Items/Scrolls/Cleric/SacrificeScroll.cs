using System;
using Server;
using Server.Network;
using Server.Spells.Cleric; 


namespace Server.Items
{
	public class ClericSacrificeScroll : SpellScroll
	{
		[Constructable]
		public ClericSacrificeScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericSacrificeScroll( int amount ) : base( 809, 0xE39, amount )
		{
			Name = "Sacrifice Scroll";
			Hue = 0x1F0;
		}

		public ClericSacrificeScroll( Serial serial ) : base( serial )
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
