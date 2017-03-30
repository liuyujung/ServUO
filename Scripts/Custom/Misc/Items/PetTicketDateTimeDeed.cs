using System;
using Server.Targeting;

namespace Server.Items
{
	public class PetTicketDateTimeDeed : Item
	{
		[Constructable]
		public PetTicketDateTimeDeed(): base(0x14F0)
		{
			Weight = 1.0;
			Movable = true;
			LootType = LootType.Blessed;
			Name = "Pet Claim Ticket Time Changing Deed";
		}

		public PetTicketDateTimeDeed(Serial serial) : base(serial) { }

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			else
			{
				from.SendMessage("Which Pet Claim Ticket would you like to set?");
				from.Target = new PetTicketDateTimeTarget(this);
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

	public class PetTicketDateTimeTarget : Target
	{
		private PetTicketDateTimeDeed m_Deed;

		public PetTicketDateTimeTarget(PetTicketDateTimeDeed deed) : base(1, false, TargetFlags.None)
		{
			m_Deed = deed;
		}

		protected override void OnTarget(Mobile from, object targeted)
		{
			if (m_Deed.Deleted || m_Deed.RootParent != from)
				return;

			if (targeted is PetClaimTicket)
			{
				PetClaimTicket item = (PetClaimTicket)targeted;

				if (item.Time < DateTime.Now)
				{
					from.SendMessage("This Pet Claim Ticket time has passed");
				}
				else
				{
					item.Time = DateTime.Now;
					from.SendMessage("The Pet Claim Ticket time has been set to now");
					m_Deed.Delete();
				}
			}
			else
			{
				from.SendMessage("You must target a Pet Claim Ticket!");
			}
		}
	}
}
