using Server.Mobiles;

namespace Server.Items
{
    public class BankStorage25Deed : Item
    {
        [Constructable]
        public BankStorage25Deed()
            : base(0x14F0)
        {
            Weight = 1.0;
            Name = "Bank Box Storage Increase - Bonus 25 Items";
            Hue = 1175;
        }

        public BankStorage25Deed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else
            {
              PlayerMobile pm = from as PlayerMobile;
              
				pm.BankBox.MaxItems += 25;
              from.SendMessage( "Your bank storage has been increased by 25.  You must log out and log back in for this to take effect." );
              
              Delete();
            }
        }

    }
    
    public class BankStorage50Deed : Item
    {
        [Constructable]
        public BankStorage50Deed()
            : base(0x14F0)
        {
            Weight = 1.0;
            Name = "Bank Box Storage Increase - Bonus 50 Items";
            Hue = 1175;
        }

        public BankStorage50Deed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else
            {
              PlayerMobile pm = from as PlayerMobile;
              
              pm.BankBox.MaxItems += 50;
              from.SendMessage( "Your bank storage has been increased by 50.  You must log out and log back in for this to take effect." );
              
              Delete();
            }
        }

    }
    
    public class BankStorage100Deed : Item
    {
        [Constructable]
        public BankStorage100Deed()
            : base(0x14F0)
        {
            Weight = 1.0;
            Name = "Bank Box Storage Increase - Bonus 100 Items";
            Hue = 1175;
        }

        public BankStorage100Deed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else
            {
              PlayerMobile pm = from as PlayerMobile;
              
              pm.BankBox.MaxItems += 100;
              from.SendMessage( "Your bank storage has been increased by 100.  You must log out and log back in for this to take effect." );
              
              Delete();
            }
        }

    }
}


