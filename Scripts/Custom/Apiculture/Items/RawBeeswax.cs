using System;

namespace Server.Items
{
	public class RawBeeswax : Item
	{
		[Constructable]
		public RawBeeswax() : this( 1 )
		{
		}

		[Constructable]
		public RawBeeswax( int amount ) : base( 0x1422 )
		{
			Weight = 1.0;
			Stackable = true;
			Amount = amount;
			Hue = 1126;
			Name = "Raw Beeswax";
		}

		public RawBeeswax( Serial serial ) : base( serial )
		{
		}

		public override void OnAfterDuped(Item newItem)
		{

			base.OnAfterDuped(newItem);

			RawBeeswax s = newItem as RawBeeswax;
			if (s != null)
			{
				s.Amount = Amount;
			}

		}

		/*public override Item Dupe( int amount )
		{
			return base.Dupe( new RawBeeswax( amount ), amount );
		}*/

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