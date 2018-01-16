using System;
using Server;

namespace Server.Items
{
	public abstract class BaseRefreshFood : BaseMagicFood
	{
		public abstract double Refresh{ get; }

		public BaseRefreshFood( MagicFoodEffect effect ) : base( 0xF0B, effect )
		{
		}

		public BaseRefreshFood( Serial serial ) : base( serial )
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

		public override void Drink( Mobile from )
		{
			if ( from.Stam < from.StamMax )
			{
				from.Stam += Scale( from, (int)(Refresh * from.StamMax) );

				BaseMagicFood.PlayDrinkEffect( from );

				this.Delete();
			}
			else
			{
				from.SendMessage( "You decide against using this, as you are already at full stamina." );
			}
		}
	}
}