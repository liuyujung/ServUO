using System;
using Server.Items;

namespace Server.Spells.Ranger
{
	public class NaturalHerbSpell : RangerSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Nautral Herb", "Kurwa Poika",
				224,
				9011,
				Reagent.Garlic,
				Reagent.SpringWater,
				Reagent.DestroyingAngel
			);
		public override double CastDelay{ get{ return 2.0; } }
		public override double RequiredSkill{ get{ return 60.0; } }
		public override int RequiredMana{ get{ return 15; } }			

		public NaturalHerbSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override SpellCircle Circle
		{
			get
			{
				return SpellCircle.First;
			}
		}

		private static HerbInfo[] m_Herb = new HerbInfo[]
			{
				new HerbInfo( typeof( BlackHerb ), "a black herb" ),
				new HerbInfo( typeof( BlackHerb ), "a black herb" ),
				new HerbInfo( typeof( BlackHerb ), "a black herb" ),
				new HerbInfo( typeof( BlackHerb ), "a black herb" ),
				new HerbInfo( typeof( BlueHerb ), "a blue herb" ),
				new HerbInfo( typeof( BlueHerb ), "a blue herb" ),
				new HerbInfo( typeof( BlueHerb ), "a blue herb" ),
				new HerbInfo( typeof( GreenHerb ), "a green herb" ),
				new HerbInfo( typeof( GreenHerb ), "a green herb" ),
				new HerbInfo( typeof( HerbOfLife ), "an herb of life" ),
				new HerbInfo( typeof( OrangeHerb ), "an orange herb" ),
				new HerbInfo( typeof( OrangeHerb ), "an orange herb" ),
				new HerbInfo( typeof( OrangeHerb ), "an orange herb" ),
				new HerbInfo( typeof( RedHerb ), "a red herb" ),
				new HerbInfo( typeof( RedHerb ), "a red herb" ),
				new HerbInfo( typeof( RedHerb ), "a red herb" ),
				new HerbInfo( typeof( RedHerb ), "a red herb" ),
				new HerbInfo( typeof( WhiteHerb ), "a white herb" ),
				new HerbInfo( typeof( WhiteHerb ), "a white herb" ),
				new HerbInfo( typeof( YellowHerb ), "a yellow herb" ),
				new HerbInfo( typeof( YellowHerb ), "a yellow herb" ),
				new HerbInfo( typeof( YellowHerb ), "a yellow herb" ),
			};

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				HerbInfo herbInfo = m_Herb[Utility.Random( m_Herb.Length )];
				Item herb = herbInfo.Create();

				if ( herb != null )
				{
					Caster.AddToBackpack( herb );

					// You magically create food in your backpack:
					Caster.SendMessage( "You search the area and find an herb.", true, " " + herbInfo.Name );

					Caster.FixedParticles( 0, 10, 5, 2003, EffectLayer.RightHand );
					Caster.PlaySound( 0x1E2 );
				}
			}

			FinishSequence();
		}
	}

	public class HerbInfo
	{
		private Type m_Type;
		private string m_Name;

		public Type Type{ get{ return m_Type; } set{ m_Type = value; } }
		public string Name{ get{ return m_Name; } set{ m_Name = value; } }

		public HerbInfo( Type type, string name )
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