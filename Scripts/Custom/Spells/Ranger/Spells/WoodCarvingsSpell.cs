using System;
using Server.Items;

namespace Server.Spells.Ranger
{
	public class WoodCarvingsSpell : RangerSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Wood Carvings", "Kurwa Taur Nat'",
				224,
				9011,
				Reagent.Kindling
			);
		public override double CastDelay{ get{ return 2.0; } }
		public override double RequiredSkill{ get{ return 50.0; } }
		public override int RequiredMana{ get{ return 10; } }			

		public WoodCarvingsSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override SpellCircle Circle
		{
			get
			{
				return SpellCircle.First;
			}
		}

		private static WoodItemInfo[] m_WoodItem = new WoodItemInfo[]
			{
				new WoodItemInfo( typeof( RecallRune ), "a recall rune" ),
				//TO DO: I got more ideas for items but Need to think up on what I want to do.
				//new WoodItemInfo( typeof( BlackHerb ), "" ),
				//new WoodItemInfo( typeof( BlackHerb ), "" ),
				//new WoodItemInfo( typeof( BlackHerb ), "" ),

			};

		public override void OnCast()
		{
            if ( CheckSequence() )
			{
				WoodItemInfo wooditemInfo = m_WoodItem[Utility.Random( m_WoodItem.Length )];
				Item wooditem = wooditemInfo.Create();

				if ( wooditem != null )
				{
					Caster.AddToBackpack( wooditem );

					
					Caster.SendMessage( "You carve an item from the wood.", true, " " + wooditemInfo.Name );

					Caster.PlaySound( 578 );//TO DO: if someone knows the hex number for this be my guest to replace it :)
				}
			}

			FinishSequence();
		}
	}

	public class WoodItemInfo
	{
		private Type m_Type;
		private string m_Name;

		public Type Type{ get{ return m_Type; } set{ m_Type = value; } }
		public string Name{ get{ return m_Name; } set{ m_Name = value; } }

		public WoodItemInfo( Type type, string name )
		{
			m_Type = type;
			m_Name = name;
		}

		public Item Create()
		{
			Item item;

			try
			{
				item = (Item)Activator.CreateInstance( m_Type );
			}
			catch
			{
				item = null;
			}

			return item;
		}
	}
}