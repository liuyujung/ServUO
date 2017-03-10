/*
 created by:
     /\       
____/_ \____  ### ### ### ### #  ### ### # ##  ##  ###
\  ___\ \  /  #   #   # # # # #  # # # # # # # # # #
 \/ /  \/ /   ### ##  ### # # #  ### # # # # # ##  ##
 / /\__/_/\     # #   # # # # #  # # # # # # # # # #
/__\ \_____\  ### ### # # # ###  # # # ### ##  # # ###
    \  /             http://www.wftpradio.net/
     \/       
*/

using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Xanthos.Evo
{
	public class ChargerOfTheFallenEvoDust : BaseEvoDust
	{
		[Constructable]
		public ChargerOfTheFallenEvoDust() : this( 1 )
		{
		}

		[Constructable]
		public ChargerOfTheFallenEvoDust( int amount ) : base( amount )
		{
			Amount = amount;
			Name = "Charger Of The Fallen Dust";
			Hue = 1153;
		}

		public ChargerOfTheFallenEvoDust( Serial serial ) : base ( serial )
		{
		}

		public override BaseEvoDust NewDust()
		{
			return new ChargerOfTheFallenEvoDust();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}