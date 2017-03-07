/*
 created by:
     /\            888                   888     .d8888b.   .d8888b.  
____/_ \____       888                   888    d88P  Y88b d88P  Y88b 
\  ___\ \  /       888                   888    888    888 888    888 
 \/ /  \/ /    .d88888  8888b.   8888b.  888888 Y88b. d888 Y88b. d888 
 / /\__/_/\   d88" 888     "88b     "88b 888     "Y888P888  "Y888P888 
/__\ \_____\  888  888 .d888888 .d888888 888           888        888 
    \  /      Y88b 888 888  888 888  888 Y88b.  Y88b  d88P Y88b  d88P 
     \/        "Y88888 "Y888888 "Y888888  "Y888  "Y8888P"   "Y8888P"  

description: a small mini stand item that can hold music instruments up to 60000 uses each.
Thanx to Sunshine for the idea and TMSTKSBK for deco help ;)
*/
using System;
using System.Collections;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class BardsStand : Item
	{
		private Hashtable htSlayer = new Hashtable();
		public Hashtable Slayer{ get{ return htSlayer; } set{ htSlayer = value; } }
		public int LastChecked=10;

		public static string[] Names = new string[]
		{
			"Normal",			"Exceptional",			"Silver",				"Orc Slaying",		
			"Troll Slaughter",	"Ogre Trashing",		"Repond",				"Dragon Slaying",
			"Terathan",			"Snakes Bane",			"Lizardman Slaughter",	"Reptilian Death",
			"Daemon Dismissal",	"Gargoyles Foe",		"Balron Damnation",		"Exorcism",
			"Ophidian",			"Spiders Death",		"Scorpions Bane",		"Arachnid Doom",
			"Flame Dousing",	"Water Dissipation",	"Vacuum",				"Elemental Health",
			"Earth Shatter",	"Blood Drinking",		"Summer Wind",			"Elemental Ban",
			"Fey"
		};
		
		[Constructable]
		public BardsStand() : base( 3772 )
		{
			Movable = true;
			Weight = 1.0;
			Hue = 1153;
			Name = "Bards Stand";
			//LootType = LootType.Blessed;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if (!(from is PlayerMobile))
				return;
			if ( IsChildOf(from.Backpack) || IsChildOf(from.BankBox) )
				from.SendGump( new BardsStandGump( from, this ) );
			else
				from.SendMessage(32, "The bards stand must be in your backpack or your bank box.");
		}

		public void BeginCombine( Mobile from ) { from.Target = new BardsStandTarget( this ); }		
		
		public void EndCombine( Mobile from, object o )
		{
			if (!(o is BaseInstrument))
			{
				from.SendMessage(32, "That isn't a musical instrument");
				return;
			}
			AddInstrument( (BaseInstrument)o, from );
		}

		public void AddInstrument( BaseInstrument bi, Mobile from)
		{
			if ( htSlayer == null )
				htSlayer = new Hashtable();
			int iHave = 0;
			string sKey = "";
			if ( bi.Slayer2 != SlayerName.None )
			{
				from.SendMessage(32, "This musical instrument is too powerfull to be combined.");
				return;
			}
			if ( bi.Slayer != SlayerName.None )
			{
				sKey = bi.Slayer.ToString();
				string sTemp = bi.Slayer.ToString();
				int iBreak = sTemp.IndexOfAny("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(),1);
				if ( iBreak > -1 )
				{
					sKey = sTemp.Substring( 0, iBreak );
					sTemp = sTemp.Substring( iBreak );
					iBreak = sTemp.IndexOfAny("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(),1);
					while ( iBreak > -1 )
					{
						sKey = sKey + " " + sTemp.Substring(0, iBreak );
						sTemp =  sTemp.Substring( iBreak );
						iBreak = sTemp.IndexOfAny("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(),1);
					}
					if ( sTemp != null && sTemp != sKey )
						sKey += " " + sTemp;
				}
			}
			else if ( bi.Quality == InstrumentQuality.Exceptional )
				sKey = "Exceptional";
			else
				sKey = "Normal";
			
			if ( htSlayer[sKey] != null )
				iHave = (int)htSlayer[sKey];
			if ( iHave > 60000 )
			{
				from.SendGump( new BardsStandGump( from, this ) );
				from.SendMessage(33, "You can't add more charges, the limit is 60,000.");
				return;
			}
			if ( iHave + bi.UsesRemaining > 60000 )
			{
				bi.UsesRemaining = iHave + bi.UsesRemaining - 60000;
				iHave = 60000;
			}
			else
			{
				iHave += bi.UsesRemaining;
				bi.Delete();
			}

			if ( htSlayer.ContainsKey(sKey) )
				htSlayer[sKey] = iHave;
			else
				htSlayer.Add(sKey, iHave);
				
			BeginCombine( from );
			from.SendGump( new BardsStandGump( from, this ) );
			from.SendMessage(88, "You added the Instrument charges.");
		}

		public BardsStand( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 ); // version
			
			writer.Write( htSlayer.Count );
			foreach ( DictionaryEntry de in htSlayer )
			{
				writer.Write( (string)de.Key );
				writer.Write( (int)de.Value );
			}
			writer.Write( (int)LastChecked );
		} 
		
		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt();
			
			switch ( version )
			{
				case 0:
				{
					int len;
					len = reader.ReadInt();
					for (int i=0; i < len; i++)
						htSlayer.Add( (string)reader.ReadString(), reader.ReadInt() );
					LastChecked = reader.ReadInt();
					break;
				}
			}
		} 
	} 

	public class BardsStandGump : Gump
	{
		private Mobile mFrom;
		private BardsStand bsStand;

		public BardsStandGump( Mobile from, BardsStand stand ) : base( 25, 25 )
		{
			mFrom = from;
			bsStand = stand;

			from.CloseGump( typeof( BardsStandGump ) );

			AddPage( 0 );

			AddBackground( 0, 0, 505, 410, 5170 );
			AddPage(0);
			AddLabel( 170, 25, 25, "daat99's Bards Stand" );

			for (int line = 0; line < 15; line++)
			{
				if ( stand.Slayer[BardsStand.Names[line]] != null && (int)stand.Slayer[BardsStand.Names[line]] > 0)
				{
					AddButton( 30, 50+line*20, 4015, 4016, 100+line, GumpButtonType.Reply, 0 );
					AddLabel( 65, 50+line*20, 2118, stand.Slayer[BardsStand.Names[line]].ToString() );
				}
				else
				{
					AddButton( 30, 50+line*20, 4006, 4007, 999, GumpButtonType.Reply, 0 );
					AddLabel( 65, 50+line*20, 32, "0" );
				}
				AddLabel( 115, 50+line*20, 0x486, BardsStand.Names[line] );
				if (line < 14)
				{
					if ( stand.Slayer[BardsStand.Names[line+15]] != null && (int)stand.Slayer[BardsStand.Names[line+15]] > 0)
					{
						AddButton( 260, 50+line*20, 4015, 4016, 100+line+15, GumpButtonType.Reply, 0 );
						AddLabel( 295, 50+line*20, 2118, stand.Slayer[BardsStand.Names[line+15]].ToString() );
					}
					else
					{
						AddButton( 260, 50+line*20, 4006, 4007, 999, GumpButtonType.Reply, 0 );
						AddLabel( 295, 50+line*20, 32, "0" );
					}
					AddLabel( 345, 50+line*20, 0x486, BardsStand.Names[line+15] );
				}
				else
				{
					AddButton( 300, 50+line*20, 4006, 4007, 999, GumpButtonType.Reply, 0 );
					AddLabel( 345, 50+line*20, 32, "Add" );
					AddButton( 380, 50+line*20, 4015, 4016, 999, GumpButtonType.Reply, 0 );
				}
				AddRadio(25, 355, 9727, 9730, (stand.LastChecked == 10), 10 );
				AddLabel(55, 360, 69, "Lap Harp" );
				AddRadio(115, 355, 9727, 9730, (stand.LastChecked == 11), 11 );
				AddLabel(145, 360, 69, "Harp" );
				
				AddRadio(180, 355, 9727, 9730, (stand.LastChecked == 12), 12 );
				AddLabel(210, 360, 69, "Drum" );
				AddRadio(245, 355, 9727, 9730, (stand.LastChecked == 13), 13 );
				AddLabel(275, 360, 69, "Lute" );

				AddRadio(310, 355, 9727, 9730, (stand.LastChecked == 14), 14 );
				AddLabel(340, 360, 69, "Tambourine" );
				if ( Core.SE )
				{
					AddRadio(410, 355, 9727, 9730, (stand.LastChecked == 15), 15 );
					AddLabel(445, 360, 69, "Flute" );
				}
			}
		}

		public override void OnResponse( NetState sender, RelayInfo info)
		{
			if ( bsStand.Deleted )
				return;
			if ( info.ButtonID >= 100 && info.ButtonID < 150 ) //blacksmith
			{
				string sKey = BardsStand.Names[info.ButtonID-100];
				int itemID, well, bad;
				double weight;
				Harp  inst = new Harp();

				if		(info.IsSwitched(11)) {	itemID = 0xEB1;  well = 0x43;  bad = 0x44;  weight = 35.0; bsStand.LastChecked = 11; } //Standing Harp
				else if (info.IsSwitched(12)) { itemID = 0xE9C;  well = 0x38;  bad = 0x39;  weight = 4.0;  bsStand.LastChecked = 12; } //Drum
				else if (info.IsSwitched(13)) { itemID = 0xEB3;  well = 0x4C;  bad = 0x4D;  weight = 5.0;  bsStand.LastChecked = 13; } //Lute
				else if (info.IsSwitched(14)) { itemID = 0xE9D;  well = 0x52;  bad = 0x53;  weight = 1.0;  bsStand.LastChecked = 14; } //Tambourine
				else if (info.IsSwitched(15)) { itemID = 0x2805; well = 0x504; bad = 0x503; weight = 2.0;  bsStand.LastChecked = 15; } //Bamboo Flute
				else						  { itemID = 0xEB2;  well = 0x45;  bad = 0x46;  weight = 10.0; bsStand.LastChecked = 10; } //Lap Harp
				
				inst.ItemID = itemID;
				inst.SuccessSound = well;
				inst.FailureSound = bad;
				inst.Weight = weight;
				switch ( info.ButtonID )
				{
					case 100: inst.Quality = InstrumentQuality.Regular; break;
					case 101: inst.Quality = InstrumentQuality.Exceptional; break;
					case 102: inst.Slayer = SlayerName.Silver; break;
					case 103: inst.Slayer = SlayerName.OrcSlaying; break;
					case 104: inst.Slayer = SlayerName.TrollSlaughter; break;
					case 105: inst.Slayer = SlayerName.OgreTrashing; break;
					case 106: inst.Slayer = SlayerName.Repond; break;
					case 107: inst.Slayer = SlayerName.DragonSlaying; break;
					case 108: inst.Slayer = SlayerName.Terathan; break;
					case 109: inst.Slayer = SlayerName.SnakesBane; break;
					case 110: inst.Slayer = SlayerName.LizardmanSlaughter; break;
					case 111: inst.Slayer = SlayerName.ReptilianDeath; break;
					case 112: inst.Slayer = SlayerName.DaemonDismissal; break;
					case 113: inst.Slayer = SlayerName.GargoylesFoe; break;
					case 114: inst.Slayer = SlayerName.BalronDamnation; break;
					case 115: inst.Slayer = SlayerName.Exorcism; break;
					case 116: inst.Slayer = SlayerName.Ophidian; break;
					case 117: inst.Slayer = SlayerName.SpidersDeath; break;
					case 118: inst.Slayer = SlayerName.ScorpionsBane; break;
					case 119: inst.Slayer = SlayerName.ArachnidDoom; break;
					case 120: inst.Slayer = SlayerName.FlameDousing; break;
					case 121: inst.Slayer = SlayerName.WaterDissipation; break;
					case 122: inst.Slayer = SlayerName.Vacuum; break;
					case 123: inst.Slayer = SlayerName.ElementalHealth; break;
					case 124: inst.Slayer = SlayerName.EarthShatter; break;
					case 125: inst.Slayer = SlayerName.BloodDrinking; break;
					case 126: inst.Slayer = SlayerName.SummerWind; break;
					case 127: inst.Slayer = SlayerName.ElementalBan; break;
					case 128: inst.Slayer = SlayerName.Fey; break;
				}
				inst.UsesRemaining = (int)bsStand.Slayer[sKey];
				bsStand.Slayer[sKey] = 0;
				mFrom.AddToBackpack( inst );
				mFrom.SendMessage( 2118, "You extracted the instrument from the Bards Stand.");
				mFrom.SendGump( new BardsStandGump( mFrom, bsStand ) );
			}
			if ( info.ButtonID == 999 )//add
			{
				bsStand.BeginCombine( mFrom );
				mFrom.SendGump( new BardsStandGump( mFrom, bsStand ) );
			}
		}
	}

	public class BardsStandTarget : Target
	{
		private BardsStand bsStand;

		public BardsStandTarget( BardsStand stand ) : base( 18, false, TargetFlags.None )
		{
			bsStand = stand;
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( bsStand.Deleted )
				return;

			bsStand.EndCombine( from, targeted );
		}
	}
}