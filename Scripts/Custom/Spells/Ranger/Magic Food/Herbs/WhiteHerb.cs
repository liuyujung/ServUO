using System;
using Server;

namespace Server.Items
{
	public class WhiteHerb : BaseStrengthFood
	{
		public override int StrOffset{ get{ return 10; } }
		public override TimeSpan Duration{ get{ return TimeSpan.FromMinutes( 2.0 ); } }

		[Constructable]
		public WhiteHerb() : base( MagicFoodEffect.Strength )
		{
			ItemID = 6810;
			this.Weight = 1.0;
			Name = "white herb";
			Hue = 1150;		
		}

		public WhiteHerb( Serial serial ) : base( serial )
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