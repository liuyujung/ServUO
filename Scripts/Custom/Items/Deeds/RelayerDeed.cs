using Server.Gumps;
using Server.Targeting;
using Server.Network;
using Server.Misc;

namespace Server.Items
{
    public class RelayerDeed : Item
    {
		[Constructable]
		public RelayerDeed(): base(0x14F0)
        {
			Weight = 1.0;
			LootType = LootType.Blessed;
			Name = "Relayer Deed";
            Hue = 1161;
		}

		public RelayerDeed(Serial serial) : base(serial) { }

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			else
			{
				from.SendMessage("Which item would you like to use the relayer deed on?");
				from.Target = new RelayerItemTarget(this);
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

		public class RelayerItemTarget : Target
		{
			private RelayerDeed m_Deed;

			public RelayerItemTarget(RelayerDeed deed) : base(1, false, TargetFlags.None)
			{
				m_Deed = deed;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (m_Deed.Deleted || m_Deed.RootParent != from)
					return;

                if (targeted is Item)
                {
                    Item o = (Item)targeted;
                    if (!o.IsChildOf(from.Backpack))
                    {
                        from.SendMessage("The target item must be in your backpack!");
                    }
                    else if (o.Layer != Layer.Invalid)
                    {
						from.CloseGump(typeof(RelayerConfirmationGump));
                        from.SendGump(new RelayerConfirmationGump(m_Deed, o));
                    }
                    else
                    {
                        from.SendMessage("Invalid item!");
                    }
				}
				else
				{
					from.SendMessage("You must target an item!");
				}
			}
		}

        private class RelayerConfirmationGump : Gump
		{
            private RelayerDeed m_Deed;
			private Item m_Targeted;

			public RelayerConfirmationGump(RelayerDeed deed, Item targeted)
				: base(150, 150)
			{
                m_Deed = deed;
				m_Targeted = targeted;

				AddPage(0);

				AddBackground(0, 0, 240, 235, 0x2422);
                AddHtml(15, 15, 210, 175, "You are using this relayer deed on " + ItemUtility.GetItemName(targeted) + ". Continue to pick a sample?", true, false);

				AddButton(160, 195, 0xF7, 0xF8, 1, GumpButtonType.Reply, 0);    //Okay
				AddButton(90, 195, 0xF2, 0xF1, 0, GumpButtonType.Reply, 0);     //Cancel
			}

			public override void OnResponse(NetState sender, RelayInfo info)
			{
				if (info.ButtonID != 1)
					return;

				Mobile from = sender.Mobile;
                from.Target = new RelayerSampleTarget(m_Deed, m_Targeted);
			}
		}

		public class RelayerSampleTarget : Target
		{
            private Item m_TargetedItem;
			private RelayerDeed m_Deed;

			public RelayerSampleTarget(RelayerDeed deed, Item targetedItem) : base(1, false, TargetFlags.None)
			{
                m_TargetedItem = targetedItem;
				m_Deed = deed;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (m_Deed.Deleted || m_Deed.RootParent != from)
					return;

				if (targeted is Item)
				{
					Item sample = (Item)targeted;
					if (!sample.IsChildOf(from.Backpack))
					{
						from.SendMessage("The sample item must be in your backpack!");
					}
                    else if (sample is BaseWeapon)
                    {
                        from.SendMessage("The sample item can not be a weapon!");
                    }
					else if (sample.Layer != Layer.Invalid)
					{
						from.CloseGump(typeof(RelayerFinalConfirmationGump));
						from.SendGump(new RelayerFinalConfirmationGump(m_Deed, m_TargetedItem, sample));
					}
				}
				else
				{
					from.SendMessage("You must target an item!");
				}
			}
		}

		private class RelayerFinalConfirmationGump : Gump
		{
			private RelayerDeed m_Deed;
			private Item m_Targeted;
            private Item m_Sample;

			public RelayerFinalConfirmationGump(RelayerDeed deed, Item targeted, Item sample)
				: base(150, 150)
			{
				m_Deed = deed;
				m_Targeted = targeted;
                m_Sample = sample;

				AddPage(0);

				AddBackground(0, 0, 240, 235, 0x2422);
                AddHtml(15, 15, 210, 175, "You are going to change the appearance and layer of " + ItemUtility.GetItemName(m_Targeted) + " to " + ItemUtility.GetItemName(m_Sample) + ". Are you sure?", true, false);

				AddButton(160, 195, 0xF7, 0xF8, 1, GumpButtonType.Reply, 0);    //Okay
				AddButton(90, 195, 0xF2, 0xF1, 0, GumpButtonType.Reply, 0);     //Cancel
			}

			public override void OnResponse(NetState sender, RelayInfo info)
			{
				if (info.ButtonID != 1)
					return;

				Mobile from = sender.Mobile;
				m_Targeted.Layer = m_Sample.Layer;
				m_Targeted.ItemID = m_Sample.ItemID;
				m_Deed.Delete();
                from.SendMessage("You have changed the appearance and layer of " + ItemUtility.GetItemName(m_Targeted) + " to " + ItemUtility.GetItemName(m_Sample) + ".");
			}
		}

    }
}
