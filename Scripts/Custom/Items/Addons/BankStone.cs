using Server.Misc;

namespace Server.Items
{
    public class BankStoneComponent : AddonComponent
    {
        public BankStoneComponent(int hue) : base(0x0ED6)
        {
			Name = "Bank Stone";
			Hue = hue;
        }

        public BankStoneComponent(Serial serial) : base(serial) { }

		public override void OnDoubleClick(Mobile from)
		{
			BankBox box = from.BankBox;
			if (box != null)
				box.Open();
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
    }

    public class BankStoneAddon : BaseAddon
    {
        public BankStoneAddon(int hue)
        {
            AddComponent(new BankStoneComponent(hue), 0, 0, 0);
        }

		public BankStoneAddon(Serial serial) : base(serial) { }

		public override bool RetainDeedHue { get { return true; } }

        public override BaseAddonDeed Deed { get { return new BankStoneDeed(); } }

		public override bool HandlesOnSpeech { get { return true; } }

		public override void OnSpeech(SpeechEventArgs e)
		{
            BankUtility.OnSpeech(e, Location);
			base.OnSpeech(e);
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
    }

	public class BankStoneDeed : BaseAddonDeed
	{
		[Constructable]
		public BankStoneDeed() : base()
		{
            Name = "Bank Stone Deed";
            Hue = 1161;
		}

		public BankStoneDeed(Serial serial) : base(serial)
		{
		}

		public override BaseAddon Addon
		{
			get
			{
				return new BankStoneAddon(Hue);
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.WriteEncodedInt(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadEncodedInt();
		}
	}
}
