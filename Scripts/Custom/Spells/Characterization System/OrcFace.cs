using System;

namespace Server.Items
{

	public class OrcFace : BaseEarrings
	{
		[Constructable]
		public OrcFace() : base( 0x141B )
		{
			Weight = 0.1;
			Name = "an Orcs face";
			Hue = 2212;
			Movable = false;
		}

		public OrcFace( Serial serial ) : base( serial )
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
	}

}