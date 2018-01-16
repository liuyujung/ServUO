using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public enum MagicFoodEffect
	{
		Nightsight,
		CureLesser,
		Cure,
		CureGreater,
		Agility,
		AgilityGreater,
		Strength,
		StrengthGreater,
		PoisonLesser,
		Poison,
		PoisonGreater,
		PoisonDeadly,
		Refresh,
		RefreshTotal,
		HealLesser,
		Heal,
		HealGreater,
		ExplosionLesser,
		Explosion,
		ExplosionGreater,
		PetResurrect,
		PetShrink,
		PetHeal,
		PetGreaterHeal,
		PetCure,
		PetGreaterCure,
				Mana,
  				TotalManaRefresh,
				Resurrect,
/*                Invisibility,
                Stealth,
                SuperStealth,
                
                CoronasHealing,
                Repair,
                SuperRepair,
                SuperPotion,
                WaterElemental,
                FireElemental,
                PoisonElemental,
                BloodElemental,
                EarthElemental*/
	}

	public abstract class BaseMagicFood : Item, ICraftable
	{
		private MagicFoodEffect m_MagicFoodEffect;

		public MagicFoodEffect MagicFoodEffect
		{
			get
			{
				return m_MagicFoodEffect;
			}
			set
			{
				m_MagicFoodEffect = value;
				InvalidateProperties();
			}
		}

		public override int LabelNumber{ get{ return 1041314 + (int)m_MagicFoodEffect; } }

		public BaseMagicFood( int itemID, MagicFoodEffect effect ) : base( itemID )
		{
			m_MagicFoodEffect = effect;

			Stackable = false;
			Weight = 1.0;
		}

		public BaseMagicFood( Serial serial ) : base( serial )
		{
		}

		public virtual bool RequireFreeHand{ get{ return true; } }

		public static bool HasFreeHand( Mobile m )
		{
			Item handOne = m.FindItemOnLayer( Layer.OneHanded );
			Item handTwo = m.FindItemOnLayer( Layer.TwoHanded );

			if ( handTwo is BaseWeapon )
				handOne = handTwo;

			return ( handOne == null || handTwo == null );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !Movable )
				return;

			if ( from.InRange( this.GetWorldLocation(), 1 ) )
			{
				if ( !RequireFreeHand || HasFreeHand( from ) )
					Drink( from );
				else
					from.SendMessage( "Your hands are full! You need a free hand to eat this." );
			}
			else
			{
				from.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_MagicFoodEffect );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_MagicFoodEffect = (MagicFoodEffect)reader.ReadInt();
					break;
				}
			}
		}

		public abstract void Drink( Mobile from );

		public static void PlayDrinkEffect( Mobile m )
		{
			m.RevealingAction();

			m.PlaySound( Utility.Random( 0x3A, 3 ) );
			//m.AddToBackpack( new Bottle() );

			if ( m.Body.IsHuman /*&& !m.Mounted*/ )
				m.Animate( 34, 5, 1, true, false, 0 );
		}

		public static TimeSpan Scale( Mobile m, TimeSpan v )
		{
			if ( !Core.AOS )
				return v;

			double scalar = 1.0 + (0.01 * AosAttributes.GetValue( m, AosAttribute.EnhancePotions ));

			return TimeSpan.FromSeconds( v.TotalSeconds * scalar );
		}

		public static double Scale( Mobile m, double v )
		{
			if ( !Core.AOS )
				return v;

			double scalar = 1.0 + (0.01 * AosAttributes.GetValue( m, AosAttribute.EnhancePotions ));

			return v * scalar;
		}

		public static int Scale( Mobile m, int v )
		{
			if ( !Core.AOS )
				return v;

			return AOS.Scale( v, 100 + AosAttributes.GetValue( m, AosAttribute.EnhancePotions ) );
		}
		#region ICraftable Members

		public int OnCraft( int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, ITool tool, CraftItem craftItem, int resHue )
		{
			/*if ( craftSystem is DefAlchemy )
			{
				Container pack = from.Backpack;

				if ( pack != null )
				{
					Item[] kegs = pack.FindItemsByType( typeof( PotionKeg ), true );

					for ( int i = 0; i < kegs.Length; ++i )
					{
						PotionKeg keg = kegs[i] as PotionKeg;

						if ( keg == null )
							continue;

						if ( keg.Held <= 0 || keg.Held >= 100 )
							continue;

						if ( keg.Type != PotionEffect )
							continue;

						++keg.Held;

						Delete();
						from.AddToBackpack( new Bottle() );

						return -1; // signal placed in keg
					}
				}
			}*/

			return 1;
		}

		#endregion
	}
}