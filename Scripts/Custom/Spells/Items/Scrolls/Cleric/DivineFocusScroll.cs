using System;
using Server;
using Server.Network;
using Server.Spells.Cleric; 


namespace Server.Items
{
	public class ClericDivineFocusScroll : SpellScroll
	{
		[Constructable]
		public ClericDivineFocusScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericDivineFocusScroll( int amount ) : base( 804, 0xE39, amount )
		{
			Name = "Divine Focus Scroll";
			Hue = 0x1F0;
		}

		public ClericDivineFocusScroll( Serial serial ) : base( serial )
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
