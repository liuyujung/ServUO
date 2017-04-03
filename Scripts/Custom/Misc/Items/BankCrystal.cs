using Server.Items;
using Server.Gumps; 
using Server.Network; 
using Server.Menus; 
using Server.Mobiles;
using Server.Menus.Questions;

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
            if (!e.Handled && (this.IsChildOf(e.Mobile.Backpack) || e.Mobile.InRange(this.Location, 12)))
            {
				if (e.Keywords != null && e.Keywords.Length > 0)
				{
					int keyword = e.Keywords[0];
					if (keyword == 0x0002)
					{
						e.Handled = true;
						if (e.Mobile.Criminal)
						{
							e.Mobile.SendMessage("Thou art a criminal and cannot access thy bank box.");
						}
						else
						{
							BankBox box = e.Mobile.BankBox;
							if (box != null)
								box.Open();
						}
					}
				}
            }
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