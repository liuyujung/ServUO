using System;
using Server;
using Server.Network;
using Server.Spells.Cleric; 


namespace Server.Items
{
	public class ClericSacredBoonScroll : SpellScroll
	{
		[Constructable]
		public ClericSacredBoonScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericSacredBoonScroll( int amount ) : base( 808, 0xE39, amount )
		{
			Name = "Sacred Boon Scroll";
			Hue = 0x1F0;
		}

		public ClericSacredBoonScroll( Serial serial ) : base( serial )
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
