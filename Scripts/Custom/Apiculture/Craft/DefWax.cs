using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Craft
{
	public class DefWax : CraftSystem
	{
		public override SkillName MainSkill
		{
			get { return SkillName.Focus; }
		}

		public override string GumpTitleString
		{
			get { return "<BASEFONT COLOR=#FFFFFF><CENTER>WAX CRAFT MENU</CENTER></BASEFONT>"; }
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefWax();

				return m_CraftSystem;
			}
		}

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.0; 
		}

		private DefWax() : base( 1, 1, 1.25 )
		{
		}

		private static Type typeofWaxPot = typeof( WaxPotAttribute );

		public static void CheckWaxPot( Mobile from, int range, out bool waxpot )
		{
			waxpot = false;

			Map map = from.Map;

			if ( map == null )
				return;

			IPooledEnumerable eable = map.GetItemsInRange( from.Location, range );

			foreach ( Item item in eable )
			{
				Type type = item.GetType();

				if ( type.IsDefined( typeofWaxPot, false ) )
					waxpot = true;
				else if ( item.ItemID == 5162 )
					waxpot = true;

				if ( waxpot )
					break;
			}

			eable.Free();

			for ( int x = -range; ( !waxpot ) && x <= range; ++x )
			{
				for ( int y = -range; ( !waxpot ) && y <= range; ++y )
				{
					StaticTile[] tiles = map.Tiles.GetStaticTiles( from.X+x, from.Y+y, true );

					for ( int i = 0; ( !waxpot ) && i < tiles.Length; ++i )
					{
						int id = tiles[i].ID & 0x3FFF;

						if ( id == 5162 )
							waxpot = true;
					}
				}
			}
		}

		public override int CanCraft( Mobile from, ITool tool, Type typeItem )
		{
			if ( tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( tool is Item && !BaseTool.CheckAccessible( (Item)tool, from ) )
				return 1044263; // The tool must be on your person to use.

			bool waxpot;
			CheckWaxPot( from, 2, out waxpot );

			if ( waxpot )
				return 0;

			return 1005650;
		}

		public override void PlayCraftEffect( Mobile from )
		{
		}

		public override int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item )
		{
			Container pack = from.Backpack;

			if ( toolBroken )
				from.SendMessage( "You have worn out your tool"); // You have worn out your tool

			if ( failed )
			{
				if ( lostMaterial )
				{
					return 1044043; // You failed to create the item, and some of your materials are lost.
				}
				else
				{
					return 1044157; // You failed to create the item, but no materials were lost.
				}
			}
			else
			{
				return 1044154; // You create the item.
			}
		}

		public override void InitCraftList()
		{
			int index = -1;

			//Candles
			AddCraft( typeof( Candle ), "Candles", "Candle", 41.0, 61.0, typeof( Beeswax ), "Beeswax",  3 );
			AddCraft( typeof( CandleLong ), "Candles", "Long Candle", 32.0, 52.0, typeof( Beeswax ), "Beeswax",  2 );
			AddCraft( typeof( CandleLarge ), "Candles", "Large Candle", 30.0, 50.0, typeof( Beeswax ), "Beeswax",  2 );
			AddCraft( typeof( CandleShort ), "Candles", "Short Candle", 22.0, 42.0, typeof( Beeswax ), "Beeswax",  1 );
			index = AddCraft( typeof( CandleSkull ), "Candles", "Skull Candle", 49.0, 79.0, typeof( Beeswax ), "Beeswax",  4 );
			AddRes( index, typeof( BoneHelm ), "Bone Helmet", 1 );

			//Masks
			index = AddCraft( typeof( WaxMask ), "Mask", "Wax Mask", 37.0, 57.0, typeof( Beeswax ), "Beeswax",  6 );
			AddRes( index, typeof( Dyes ), "Dyes", 1 );
		}
	}

	public class WaxPotAttribute : Attribute
	{
		public WaxPotAttribute()
		{
		}
	}
}