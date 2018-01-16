using System;
using Server;

namespace Server.Items
{
	public class RedHerb : BaseRefreshFood
	{
		public override double Refresh{ get{ return 0.25; } }

		[Constructable]
		public RedHerb() : base( MagicFoodEffect.Refresh )
		{
			ItemID = 3248;
			this.Weight = 1.0;
			Name = "red herb";
			Hue = 34;			
		}

		public RedHerb( Serial serial ) : base( serial )
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