using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells.Druid; 


namespace Server.Items
{
	public class GraspingRootsScroll : SpellScroll
	{
		[Constructable]
		public GraspingRootsScroll() : this( 1 )
		{
		}
		
		[Constructable]
		public GraspingRootsScroll( int amount ) : base( 555, 0xE39, amount )
		{
			Name = "Grasping Roots Scroll";
			Hue = 0x58B;
		}
		
		public GraspingRootsScroll( Serial serial ) : base( serial )
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
