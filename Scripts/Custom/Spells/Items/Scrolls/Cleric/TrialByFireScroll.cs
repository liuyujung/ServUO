using System;
using Server;
using Server.Network;
using Server.Spells.Cleric; 


namespace Server.Items
{
	public class ClericTrialByFireScroll : SpellScroll
	{
		[Constructable]
		public ClericTrialByFireScroll() : this( 1 )
		{
		}

		[Constructable]
		public ClericTrialByFireScroll( int amount ) : base( 812, 0xE39, amount )
		{
			Name = "Trial By Fire Scroll";
			Hue = 0x1F0;
		}

		public ClericTrialByFireScroll( Serial serial ) : base( serial )
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
