using System;
using Server;

namespace Server.Items
{
	public class FoodCureLevelInfo
	{
		private Poison m_Poison;
		private double m_Chance;

		public Poison Poison
		{
			get{ return m_Poison; }
		}

		public double Chance
		{
			get{ return m_Chance; }
		}

		public FoodCureLevelInfo( Poison poison, double chance )
		{
			m_Poison = poison;
			m_Chance = chance;
		}
	}

	public abstract class BaseCureFood : BaseMagicFood
	{
		public abstract FoodCureLevelInfo[] LevelInfo{ get; }

		public BaseCureFood( MagicFoodEffect effect ) : base( 0xF07, effect )
		{
		}

		public BaseCureFood( Serial serial ) : base( serial )
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

		public void DoCure( Mobile from )
		{
			bool cure = false;

			FoodCureLevelInfo[] info = LevelInfo;

			for ( int i = 0; i < info.Length; ++i )
			{
				FoodCureLevelInfo li = info[i];

				if ( li.Poison == from.Poison && Scale( from, li.Chance ) > Utility.RandomDouble() )
				{
					cure = true;
					break;
				}
			}

			if ( cure && from.CurePoison( from ) )
			{
				from.SendLocalizedMessage( 500231 ); // You feel cured of poison!

				from.FixedEffect( 0x373A, 10, 15 );
				from.PlaySound( 0x1E0 );
			}
			else if ( !cure )
			{
				from.SendMessage( "That was not strong enough to cure your ailment." ); // That potion was not strong enough to cure your ailment!
			}
		}

		public override void Drink( Mobile from )
		{
			if ( from.Poisoned )
			{
				DoCure( from );

				BaseMagicFood.PlayDrinkEffect( from );

				from.FixedParticles( 0x373A, 10, 15, 5012, EffectLayer.Waist );
				from.PlaySound( 0x1E0 );

				this.Delete();
			}
			else
			{
				from.SendLocalizedMessage( 1042000 ); // You are not poisoned.
			}
		}
	}
}