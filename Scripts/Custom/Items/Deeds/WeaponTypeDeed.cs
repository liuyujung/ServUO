
using System;
using Server;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Targeting;
using Server.Items;
using Server.Gumps;

namespace Server.Items
{
	public class WeaponTypeDeed : Item
	{

		[Constructable]
		public WeaponTypeDeed() : base( 0x14F0 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
			Hue = 1152;
			Name = "Weapon Type Deed";
		}

        public WeaponTypeDeed(Serial serial): base(serial)
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

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else
			{
				from.SendMessage("Which weapon would you like to use the type deed on?");
				from.Target = new WeaponTarget(this);
			}
		}

		public class WeaponTarget : Target
		{
			private WeaponTypeDeed m_Deed;

			public WeaponTarget(WeaponTypeDeed deed) : base(1, false, TargetFlags.None)
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
                    if (!(o is BaseWeapon))
                    {
                        from.SendMessage("The target item is not a weapon!");
                    }
					else if (!o.IsChildOf(from.Backpack))
					{
						from.SendMessage("The target weapon must be in your backpack!");
					}
					else if (o.Layer != Layer.Invalid)
					{
						from.CloseGump(typeof(WeaponConfirmationGump));
						from.SendGump(new WeaponConfirmationGump(m_Deed, o));
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

		private class WeaponConfirmationGump : Gump
		{
			private WeaponTypeDeed m_Deed;
			private Item m_Targeted;

			public WeaponConfirmationGump(WeaponTypeDeed deed, Item targeted)
				: base(150, 150)
			{
				m_Deed = deed;
				m_Targeted = targeted;

				AddPage(0);

				AddBackground(0, 0, 240, 235, 0x2422);
                AddHtml(15, 15, 210, 175, "You are using this weapon type deed on " + targeted.DefaultName + ". Continue to pick a sample weapon?", true, false);

				AddButton(160, 195, 0xF7, 0xF8, 1, GumpButtonType.Reply, 0);    //Okay
				AddButton(90, 195, 0xF2, 0xF1, 0, GumpButtonType.Reply, 0);     //Cancel
			}

			public override void OnResponse(NetState sender, RelayInfo info)
			{
				if (info.ButtonID != 1)
					return;

				Mobile from = sender.Mobile;
				from.Target = new WeaponSampleTarget(m_Deed, m_Targeted);
			}
		}

		public class WeaponSampleTarget : Target
		{
			private Item m_TargetedItem;
			private WeaponTypeDeed m_Deed;

			public WeaponSampleTarget(WeaponTypeDeed deed, Item targetedItem) : base(1, false, TargetFlags.None)
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
                    if (!(sample is BaseWeapon))
                    {
                        from.SendMessage("The sample is not a weapon!");
                    }
					else if (!sample.IsChildOf(from.Backpack))
					{
						from.SendMessage("The sample weapon must be in your backpack!");
					}
					else if (sample.Layer != Layer.Invalid)
					{
						from.CloseGump(typeof(WeaponFinalConfirmationGump));
						from.SendGump(new WeaponFinalConfirmationGump(m_Deed, m_TargetedItem, sample));
					}
					else
					{
						from.SendMessage("Invalid weapon!");
					}
				}
				else
				{
					from.SendMessage("You must target an item!");
				}
			}
		}

		private class WeaponFinalConfirmationGump : Gump
		{
			private WeaponTypeDeed m_Deed;
			private Item m_Targeted;
			private Item m_Sample;

			public WeaponFinalConfirmationGump(WeaponTypeDeed deed, Item targeted, Item sample)
				: base(150, 150)
			{
				m_Deed = deed;
				m_Targeted = targeted;
				m_Sample = sample;

				AddPage(0);

				AddBackground(0, 0, 240, 235, 0x2422);
				AddHtml(15, 15, 210, 175, "You are going to change the type of " + m_Targeted.Name + " to the type of " + m_Sample.Name + ". Are you sure?", true, false);

				AddButton(160, 195, 0xF7, 0xF8, 1, GumpButtonType.Reply, 0);    //Okay
				AddButton(90, 195, 0xF2, 0xF1, 0, GumpButtonType.Reply, 0);     //Cancel
			}

			public override void OnResponse(NetState sender, RelayInfo info)
			{
				if (info.ButtonID != 1)
					return;

				Mobile from = sender.Mobile;
				m_Targeted.ItemID = m_Sample.ItemID;
				m_Targeted.Layer = m_Sample.Layer;
				m_Deed.Delete();
				from.SendMessage("You have changed the type of " + m_Targeted.Name + " to " + m_Sample.Name + ".");
			}
		}

	}
}
