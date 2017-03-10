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
	public class RidgebackEvoDust : BaseEvoDust
	{
		[Constructable]
		public RidgebackEvoDust() : this( 1 )
		{
		}

		[Constructable]
		public RidgebackEvoDust( int amount ) : base( amount )
		{
			Amount = amount;
			Name = "Ridgeback Dust";
			Hue = 1153;
		}

		public RidgebackEvoDust( Serial serial ) : base ( serial )
		{
		}

		public override BaseEvoDust NewDust()
		{
			return new RidgebackEvoDust();
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