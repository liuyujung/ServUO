using System;
using Server;

namespace Server.Items
{
	public class BlueHerb : BaseAgilityFood
	{
		public override int DexOffset{ get{ return 10; } }
		public override TimeSpan Duration{ get{ return TimeSpan.FromMinutes( 2.0 ); } }

		[Constructable]
		public BlueHerb() : base( MagicFoodEffect.Agility )
		{
			ItemID = 0xD39;
			Weight = 1.0;
			Name = "blue herb";
			Hue = 195;
		}

		public BlueHerb( Serial serial ) : base( serial )
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