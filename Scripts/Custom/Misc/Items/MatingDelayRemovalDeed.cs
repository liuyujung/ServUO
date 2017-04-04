using System;
using Server.Targeting;

namespace Server.Items
{
	public class MatingDelayRemovalDeed : Item
	{
		[Constructable]
		public MatingDelayRemovalDeed(): base(0x14F0)
		{
			Weight = 1.0;
			Movable = true;
			LootType = LootType.Blessed;
			Name = "Mating Delay Removal Deed";
		}

		public MatingDelayRemovalDeed(Serial serial) : base(serial) { }

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			else
			{
				from.SendMessage("Which shrunken pet would you like to remove mating delay?");
				from.Target = new MatingDelayRemovalTarget(this);
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class MatingDelayRemovalTarget : Target
	{
		private MatingDelayRemovalDeed m_Deed;

		public MatingDelayRemovalTarget(MatingDelayRemovalDeed deed) : base(1, false, TargetFlags.None)
		{
			m_Deed = deed;
		}

		protected override void OnTarget(Mobile from, object targeted)
		{
			if (m_Deed.Deleted || m_Deed.RootParent != from)
				return;

			if (targeted is ShrinkItem)
			{
				ShrinkItem item = (ShrinkItem)targeted;

				if (item.MatingDelay < DateTime.Now)
				{
					from.SendMessage("This shrunken pet does not have mating delay");
				}
				else
				{
					item.MatingDelay = DateTime.MinValue;
					item.InvalidateProperties();
					from.SendMessage("The mating delay has been removed");
					m_Deed.Delete();
				}
			}
			else
			{
				from.SendMessage("You must target a shrunken pet!");
			}
		}
	}
}
