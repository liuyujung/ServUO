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
	public class ChargerOfTheFallenEvoEgg : BaseEvoEgg
	{
		public override IEvoCreature GetEvoCreature()
		{
			return new EvoChargerOfTheFallen( "a Charger Of The Fallen hatchling" );
		}

		[Constructable]
		public ChargerOfTheFallenEvoEgg() : base()
		{
			Name = "a Charger Of The Fallen egg";
			HatchDuration = 0.01;		// 15 minutes
		}

		public ChargerOfTheFallenEvoEgg( Serial serial ) : base ( serial )
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