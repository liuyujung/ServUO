using System;
using Server;

namespace Server.Items
{
	public class OrangeHerb : BaseCureFood
	{
		private static FoodCureLevelInfo[] m_OldLevelInfo = new FoodCureLevelInfo[]
			{
				new FoodCureLevelInfo( Poison.Lesser,  1.00 ), // 100% chance to cure lesser poison
				new FoodCureLevelInfo( Poison.Regular, 0.75 ), //  75% chance to cure regular poison
				new FoodCureLevelInfo( Poison.Greater, 0.50 ), //  50% chance to cure greater poison
				new FoodCureLevelInfo( Poison.Deadly,  0.15 )  //  15% chance to cure deadly poison
			};

		private static FoodCureLevelInfo[] m_AosLevelInfo = new FoodCureLevelInfo[]
			{
				new FoodCureLevelInfo( Poison.Lesser,  1.00 ),
				new FoodCureLevelInfo( Poison.Regular, 0.95 ),
				new FoodCureLevelInfo( Poison.Greater, 0.75 ),
				new FoodCureLevelInfo( Poison.Deadly,  0.50 ),
				new FoodCureLevelInfo( Poison.Lethal,  0.25 )
			};

		public override FoodCureLevelInfo[] LevelInfo{ get{ return Core.AOS ? m_AosLevelInfo : m_OldLevelInfo; } }

		[Constructable]
		public OrangeHerb() : base( MagicFoodEffect.Cure )
		{
			ItemID = 3181;
			this.Weight = 1.0;
			Name = "orange herb";
			Hue = 43;		
		}

		public OrangeHerb( Serial serial ) : base( serial )
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