using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server;

namespace Server.Items
{
    public class RelayerTalismanTarget : Target
    {
        private RelayerTalismanDeed m_Deed;

        public RelayerTalismanTarget(RelayerTalismanDeed deed)
            : base(1, false, TargetFlags.None)
        {
            m_Deed = deed;
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (target is BaseArmor)
            {
                Item item = (Item)target;

                if (item.Layer == Layer.Talisman)
                {
                    from.SendMessage("That item is already the Talisman Layer!");
                }
                else
                {
                    if (item.RootParent != from) // Make sure its in their pack or they are wearing it
                    {
                        from.SendMessage("You cannot Relayer that!");
                    }
                    else
                    {
                        (item).Layer = Layer.Talisman;
                        from.SendMessage("You magically change the layer!");

                        m_Deed.Delete(); // Delete the deed
                    }
                }
            }
            if (target is BaseClothing)
            {
                Item item = (Item)target;

                if (item.Layer == Layer.Talisman)
                {
                    from.SendMessage("That item is already the Talisman Layer!");
                }
                else
                {
                    if (item.RootParent != from) // Make sure its in their pack or they are wearing it
                    {
                        from.SendMessage("You cannot Relayer that!");
                    }
                    else
                    {
                        (item).Layer = Layer.Talisman;
                        from.SendMessage("You magically change the layer!");

                        m_Deed.Delete(); // Delete the deed
                    }
                }
            }
            if (target is BaseJewel)
            {
                Item item = (Item)target;

                if (item.Layer == Layer.Talisman)
                {
                    from.SendMessage("That item is already the Talisman Layer!");
                }
                else
                {
                    if (item.RootParent != from) // Make sure its in their pack or they are wearing it
                    {
                        from.SendMessage("You cannot Relayer that!");
                    }
                    else
                    {
                        (item).Layer = Layer.Talisman;
                        from.SendMessage("You magically change the layer!");

                        m_Deed.Delete(); // Delete the deed
                    }
                }
            }
            if (target is BaseShield)
            {
                Item item = (Item)target;

                if (item.Layer == Layer.Talisman)
                {
                    from.SendMessage("That item is already the Talisman Layer!");
                }
                else
                {
                    if (item.RootParent != from)
                    {
                        from.SendMessage("You cannot Relayer that!");
                    }
                    else
                    {
                        (item).Layer = Layer.Talisman;
                        from.SendMessage("You magically relayer your shield....");

                        m_Deed.Delete();
                    }
                }
            }
        }
    }
    public class RelayerTalismanDeed : Item
    {
        [Constructable]
        public RelayerTalismanDeed()
            : base(0x14F0)
        {
            Weight = 1.0;
            Name = "Relayer Deed (Talisman)";
            LootType = LootType.Blessed;
            Hue = 1161;
        }

        public RelayerTalismanDeed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            LootType = LootType.Blessed;

            int version = reader.ReadInt();
        }

        public override bool DisplayLootType { get { return false; } }

        public override void OnDoubleClick(Mobile from) // Override double click of the deed to call our target
        {
            if (!IsChildOf(from.Backpack)) // Make sure its in their pack
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else
            {
                from.SendMessage("What would you like to Relayer?"); // What would you like to Relayer?
                from.Target = new RelayerTalismanTarget(this); // Call our target
            }
        }
    }
}