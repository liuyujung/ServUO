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
using Xanthos.Interfaces;

namespace Xanthos.Evo
{
	public class SavageRidgebackEvoEgg : BaseEvoEgg
	{
		public override IEvoCreature GetEvoCreature()
		{
			return new EvoSavageRidgeback( "a Savage Ridgeback hatchling" );
		}

		[Constructable]
		public SavageRidgebackEvoEgg() : base()
		{
			Name = "a Savage Ridgeback egg";
			HatchDuration = 0.01;		// 15 minutes
		}

		public SavageRidgebackEvoEgg( Serial serial ) : base ( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}