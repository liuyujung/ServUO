using System;
using Server;
using Server.Network;
using Server.Spells.Cleric; 


namespace Server.Items
{
	public class ClericAngelicFaithScroll : SpellScroll
	{
		[Constructable]
		public ClericAngelicFaithScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericAngelicFaithScroll( int amount ) : base( 801, 0xE39, amount )
		{
			Name = "Angelic Faith Scroll";
			Hue = 0x1F0;
		}

		public ClericAngelicFaithScroll( Serial serial ) : base( serial )
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
