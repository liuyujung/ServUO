using System;
using Server;
using Server.Network;
using Server.Spells.Cleric; 


namespace Server.Items
{
	public class ClericTouchOfLifeScroll : SpellScroll
	{
		[Constructable]
		public ClericTouchOfLifeScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericTouchOfLifeScroll( int amount ) : base( 811, 0xE39, amount )
		{
			Name = "Touch Of Life Scroll";
			Hue = 0x1F0;
		}

		public ClericTouchOfLifeScroll( Serial serial ) : base( serial )
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
