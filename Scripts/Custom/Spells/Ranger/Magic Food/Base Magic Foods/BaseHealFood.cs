using System;
using Server;
using Server.Network;

namespace Server.Items
{
	public abstract class BaseHealFood : BaseMagicFood
	{
		public abstract int MinHeal { get; }
		public abstract int MaxHeal { get; }
		public abstract double Delay { get; }

		public BaseHealFood( MagicFoodEffect effect ) : base( 0xF0C, effect )
		{
		}

		public BaseHealFood( Serial serial ) : base( serial )
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

		public void DoHeal( Mobile from )
		{
			int min = Scale( from, MinHeal );
			int max = Scale( from, MaxHeal );

			from.Heal( Utility.RandomMinMax( min, max ) );
		}

		public override void Drink( Mobile from )
		{
			if ( from.Hits < from.HitsMax )
			{
				if ( from.Poisoned || MortalStrike.IsWounded( from ) )
				{
					from.LocalOverheadMessage( MessageType.Regular, 0x22, 1005000 ); // You can not heal yourself in your current state.
				}
				else
				{
					if ( from.BeginAction( typeof( BaseHealFood ) ) )
					{
						DoHeal( from );

						BaseMagicFood.PlayDrinkEffect( from );

						this.Delete();

						Timer.DelayCall( TimeSpan.FromSeconds( Delay ), new TimerStateCallback( ReleaseHealLock ), from );
					}
					else
					{
						from.SendMessage( "You must wait 10 seconds before eating that again." ); 
					}
				}
			}
			else
			{
				from.SendMessage( "You are at full health. Eating this would not help you any." ); 
			}
		}

		private static void ReleaseHealLock( object state )
		{
			((Mobile)state).EndAction( typeof( BaseHealFood ) );
		}
	}
}