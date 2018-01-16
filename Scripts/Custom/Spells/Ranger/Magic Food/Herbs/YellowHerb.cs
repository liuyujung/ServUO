using System;
using Server;

namespace Server.Items
{
	public class YellowHerb : BaseHealFood
	{
		public override int MinHeal { get { return (Core.AOS ? 13 : 6); } }
		public override int MaxHeal { get { return (Core.AOS ? 16 : 20); } }
		public override double Delay{ get{ return (Core.AOS ? 8.0 : 10.0); } }

		[Constructable]
		public YellowHerb() : base( MagicFoodEffect.Heal )
		{
			ItemID = 3169;
			this.Weight = 1.0;
			Name = "yellow herb";
			Hue = 56;		
		}

		public YellowHerb( Serial serial ) : base( serial )
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