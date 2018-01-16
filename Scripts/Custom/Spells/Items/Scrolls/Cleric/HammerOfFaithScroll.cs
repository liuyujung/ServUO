using System;
using Server;
using Server.Network;
using Server.Spells.Cleric; 


namespace Server.Items
{
	public class ClericHammerOfFaithScroll : SpellScroll
	{
		[Constructable]
		public ClericHammerOfFaithScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericHammerOfFaithScroll( int amount ) : base( 805, 0xE39, amount )
		{
			Name = "Hammer Of Faith Scroll";
			Hue = 0x1F0;
		}

		public ClericHammerOfFaithScroll( Serial serial ) : base( serial )
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
