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
	[CorpseName( "a golem corpse" )]
	public class Golem : BaseEvo, IEvoCreature
	{
		public override BaseEvoSpec GetEvoSpec()
		{
			return GolemSpec.Instance;
		}

		public override BaseEvoEgg GetEvoEgg()
		{
			return new GolemEgg();
		}

		public override bool AddPointsOnDamage { get { return true; } }
		public override bool AddPointsOnMelee { get { return false; } }
		public override Type GetEvoDustType() { return typeof( GolemDust ); }

		public override bool HasBreath{ get{ return false; } }

		public Golem( string name ) : base( name, AIType.AI_Mage, 0.01 )
		{
		}

		public Golem( Serial serial ) : base( serial )
		{
		}

        public override int GetAngerSound()
        {
            return 541;
        }

        public override int GetIdleSound()
        {
            return 542;
        }

        public override int GetAttackSound()
        {
            return 562;
        }

        public override int GetHurtSound()
        {
            return 320;
        }

        public override int GetDeathSound()
        {
            return 545;
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