#region AuthorHeader
//
//	Safe Resurrection System version 2.2, by Xanthos
//
//
#endregion AuthorHeader
using System;
using Server;
using Server.Items;

namespace Xanthos.SafeResurrection
{
	public class PetResurrectionStone : Item
	{
		public override string DefaultName
		{
			get { return "Pet Resurrection Stone"; }
		}

		[Constructable]
		public PetResurrectionStone() : base( 0xED4 )
		{
			Movable = false;
			Hue = SafeResConfig.PetResStoneHue;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( SafeResConfig.AllowPetRes && SafeResConfig.UsePetResStone )
				from.Target = new PetResTarget( from );
			else
				from.SendMessage( "That is not allowed at this time." );
		}

		public PetResurrectionStone( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}