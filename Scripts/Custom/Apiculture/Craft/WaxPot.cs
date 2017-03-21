using System;
using Server;
using Server.Items;
using Server.Prompts;
using Server.Targeting;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using System.Collections;

namespace Server.Items 
{
	public class WaxPot : AddonComponent
	{
		[Constructable]
		public WaxPot() : base( 0x142A )
		{
			Name = "Wax pot";
			Movable = false;
		}

		public WaxPot( Serial serial ) : base( serial )
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class WaxPotAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new WaxPotDeed(); } }

		[Constructable]
		public WaxPotAddon()
		{
			AddComponent( new WaxPot(), 0, 0, 0 );
		}

		public WaxPotAddon( Serial serial ) : base( serial )
		{
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

	public class WaxPotDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new WaxPotAddon(); } }
		public override int LabelNumber{ get{ return 1025162; } }

		[Constructable]
		public WaxPotDeed()
		{
		}

		public WaxPotDeed( Serial serial ) : base( serial )
		{
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