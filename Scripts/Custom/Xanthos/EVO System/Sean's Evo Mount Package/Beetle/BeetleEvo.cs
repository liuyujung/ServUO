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
	[CorpseName( "a Beetle corpse" )]
	public class EvoBeetle : BaseEvoMount, IEvoCreature
	{
		public override BaseEvoSpec GetEvoSpec()
		{
			return BeetleEvoSpec.Instance;
		}

		public override BaseEvoEgg GetEvoEgg()
		{
			return new BeetleEvoEgg();
		}

		public override bool AddPointsOnDamage { get { return true; } }
		public override bool AddPointsOnMelee { get { return false; } }
		public override Type GetEvoDustType() { return typeof( BeetleEvoDust ); }

		public override bool HasBreath{ get{ return false; } }

		public EvoBeetle( string name ) : base( name, 0x317, 0x3EBC )
		{
		}

		public EvoBeetle( Serial serial ) : base( serial )
		{
		}

		public override WeaponAbility GetWeaponAbility()	
		{
			return WeaponAbility.Dismount;
		} 

		public override bool SubdueBeforeTame{ get{ return true; } } // Must be beaten into submission
		
		public override int GetAngerSound()
		{
			return 0x21D;
		}

		public override int GetIdleSound()
		{
			return 0x21D;
		}

		public override int GetAttackSound()
		{
			return 0x162;
		}

		public override int GetHurtSound()
		{
			return 0x163;
		}

		public override int GetDeathSound()
		{
			return 0x21D;
		}
		
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