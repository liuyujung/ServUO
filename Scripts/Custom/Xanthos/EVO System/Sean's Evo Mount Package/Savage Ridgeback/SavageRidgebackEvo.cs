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
	[CorpseName( "a Savage Ridgeback corpse" )]
	public class EvoSavageRidgeback : BaseEvoMount, IEvoCreature
	{
		public override BaseEvoSpec GetEvoSpec()
		{
			return SavageRidgebackEvoSpec.Instance;
		}

		public override BaseEvoEgg GetEvoEgg()
		{
			return new SavageRidgebackEvoEgg();
		}

		public override bool AddPointsOnDamage { get { return true; } }
		public override bool AddPointsOnMelee { get { return false; } }
		public override Type GetEvoDustType() { return typeof( SavageRidgebackEvoDust ); }

		public override bool HasBreath{ get{ return false; } }

		public EvoSavageRidgeback( string name ) : base( name, 0xBC, 0x3EB8 )
		{
		}

		public EvoSavageRidgeback( Serial serial ) : base( serial )
		{
		}

		public override WeaponAbility GetWeaponAbility()	
		{
			return WeaponAbility.Dismount;
		} 

		public override bool SubdueBeforeTame{ get{ return true; } } // Must be beaten into submission
		
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write( (int)0 );			
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}