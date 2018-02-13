using Server.Misc;

namespace Server.Items {
    public class BankBell : Item {

        [Constructable]
        public BankBell() : base(0x1C12) {
            Name = "Bank Bell";
            Hue = Utility.RandomMinMax(0, 3000);
            Weight = 3;
            LootType = LootType.Blessed;
        }

        public override void AddNameProperties(ObjectPropertyList list) {
            base.AddNameProperties(list);
            list.Add(1070722, "Double Click To Open Bank Box");
        } 

        public override void OnDoubleClick(Mobile from) {
			BankBox box = from.BankBox;
			if (box != null)
				box.Open();
        }

        public override bool HandlesOnSpeech { get { return true; } }

        public override void OnSpeech(SpeechEventArgs e)
        {
            BankUtility.OnSpeech(e, Location);
            base.OnSpeech(e);
        }

        public BankBell( Serial serial ) : base( serial ) {
		}

		public override void Serialize( GenericWriter writer ) {
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader ) {
			base.Deserialize( reader );
			int version = reader.ReadInt();
	    }
    }
}