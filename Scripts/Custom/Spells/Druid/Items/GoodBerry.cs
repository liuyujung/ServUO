using System;
using Server;

namespace Server.Items
{
	public class GoodBerry : BaseHealFood
	{
		public override int MinHeal { get { return 15; } }
		public override int MaxHeal { get { return 30; } }
		public override double Delay { get { return 7; } }

		 [Constructable]
		public GoodBerry() : base( 0x9D0 )
		{
			Name = "a good berry";
			Hue = 1378;
			Weight = 2.0;
			//FillFactor = 5;
			Stackable = true;
		}

		public GoodBerry( Serial serial ) : base( serial )
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