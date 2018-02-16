using Server.Targeting;

namespace Server.Items
{
    public class MageWeaponMalusRemovalDeed : Item
    {
        [Constructable]
        public MageWeaponMalusRemovalDeed() : base(0x14F0)
        {
			Weight = 1.0;
			LootType = LootType.Blessed;
			Name = "Mage Weapon Malus Removal Deed";
        }

        public MageWeaponMalusRemovalDeed(Serial serial) : base(serial) { }

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			else
			{
				from.SendMessage("Please target the weapon you would like to remove the mage weapon Malus.");
				from.Target = new MageWeaponMalusRemovalTarget(this);
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

	public class MageWeaponMalusRemovalTarget : Target
	{
		private MageWeaponMalusRemovalDeed m_Deed;

		public MageWeaponMalusRemovalTarget(MageWeaponMalusRemovalDeed deed) : base(1, false, TargetFlags.None)
		{
			m_Deed = deed;
		}

		protected override void OnTarget(Mobile from, object targeted)
		{
			if (m_Deed.Deleted || m_Deed.RootParent != from)
				return;

			if (targeted is BaseWeapon)
			{
                BaseWeapon weapon = (BaseWeapon)targeted;
                AosWeaponAttributes attributes = weapon.WeaponAttributes;

                if (attributes.MageWeapon != 0)
                {
                    attributes.MageWeapon = 0;
                    from.SendMessage("The mage weapon malus has been removed.");
                    m_Deed.Delete();
                }
                else
                {
                    from.SendMessage("The weapon does not have a mage weapon malus.");
                }
			}
			else
			{
				from.SendMessage("You must target a weapon!");
			}
		}
	}
}
