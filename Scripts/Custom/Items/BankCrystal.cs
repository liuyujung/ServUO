using Server.Misc;

namespace Server.Items
{ 
	public class BankCrystal : Item 
	{ 

		[Constructable] 
		public BankCrystal() : base( 7964 ) 
		{ 
			Movable = true; 
			Hue = 0x480; 
			Name = "Bank Crystal";
			LootType = LootType.Blessed;
		} 

		public override void OnDoubleClick( Mobile from ) 
		{ 
			//from.Handled = true;

			BankBox box = from.BankBox;

			if ( box != null )
				box.Open();

     	 } 

     	public override bool HandlesOnSpeech { get { return true; } }

        public override void OnSpeech(SpeechEventArgs e)
        {
            BankUtility.OnSpeech(e, Location);
            base.OnSpeech(e);
        }
 
		public BankCrystal( Serial serial ) : base( serial ) 
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