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
description: a small mini kit item that can hold music instruments up to 60000 uses each.
Thanx to beldr for the idea and deco help ;)
*/
using System;
using System.Collections;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class AlchemistKit : Item
	{
		private Hashtable htPot = new Hashtable();
		public Hashtable Pot{ get{ return htPot; } set{ htPot = value; } }
		public bool ToKeg=false;

		public static string[] Names = new string[]
		{
			"Empty Bottles",	"Empty Potion Kegs",	"Nightsight",		"Cure Lesser",
			"Cure",				"Cure Greater",			"Agility",			"Agility Greater",
			"Strength",			"Strength Greater",		"Poison Lesser",	"Poison",
			"Poison Greater",	"Poison Deadly",		"Refresh",			"Refresh Total",
			"Heal Lesser",		"Heal",					"Heal Greater",		"Explosion Lesser",
			"Explosion",		"Explosion Greater"
		};

		[Constructable]
		public AlchemistKit() : base( 6464 )
		{
			Movable = true;
			Weight = 1.0;
			Hue = 45;
			Name = "Alchemist Kit";
			//LootType = LootType.Blessed;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if (!(from is PlayerMobile))
				return;
			if ( IsChildOf(from.Backpack) || IsChildOf(from.BankBox) )
				from.SendGump( new AlchemistKitGump( from, this ) );
			else
				from.SendMessage(32, "The {0} must be in your backpack or your bank box.", Name);
		}

		public void BeginCombine( Mobile from ) { from.Target = new AlchemistKitTarget( this ); }		
		
		public void EndCombine( Mobile from, object o )
		{
			if (o is BaseContainer)
				AddContainer( (BaseContainer)o, from);
			else if (o is Bottle)
				AddBottle( (Bottle)o, from );
			else if (o is PotionKeg)
				AddKeg( (PotionKeg)o, from );
			else if (o is BasePotion)
				AddPot( (BasePotion)o, from );
			else
			{
				from.SendMessage(32, "You can't add this to the Alchemist Kit.");
				return;
			}
			from.SendGump( new AlchemistKitGump( from, this ) );
		}

		public void AddContainer( BaseContainer bc, Mobile from )
		{
			Item[] bottles = bc.FindItemsByType( typeof( Bottle ) );
			foreach ( Bottle b in bottles )
				AddBottle( b, from );

			Item[] kegs = bc.FindItemsByType( typeof( PotionKeg ) );
			foreach ( PotionKeg pk in kegs )
				AddKeg( pk, from );
			
			Item[] potions = bc.FindItemsByType( typeof( BasePotion ) );
			foreach ( BasePotion bp in potions )
				AddPot( bp, from );
		}

		public void AddBottle( Bottle b, Mobile from )
		{
			if ( htPot.ContainsKey("Empty Bottles") )
				htPot["Empty Bottles"] = (int)htPot["Empty Bottles"] + b.Amount;
			else
				htPot.Add("Empty Bottles", b.Amount);
			b.Delete();
			from.SendMessage(88, "You added the empty bottles");
		}

		public void AddKeg( PotionKeg pk, Mobile from)
		{
			int iHave = 0;
			if ( pk.Held == 0 )
			{
				if ( htPot.ContainsKey("Empty Potion Kegs") )
					htPot["Empty Potion Kegs"] = (int)htPot["Empty Potion Kegs"] + 1;
				else
					htPot.Add("Empty Potion Kegs", 1);
				pk.Delete();
				return;
			}

			string sKey = pk.Type.ToString(), sTemp = pk.Type.ToString();
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
				if ( htPot[sKey] != null )
					iHave = (int)htPot[sKey];
			}
			
			if ( htPot[sKey] != null )
				iHave = (int)htPot[sKey];
			if ( iHave >= 60000 )
			{
				from.SendGump( new AlchemistKitGump( from, this ) );
				from.SendMessage(33, "You can't add more, the limit is 60,000.");
				return;
			}
			
			if ( iHave + pk.Held > 60000 )
			{
				if ( htPot[sKey] != null )
				{
					pk.Held = 60000-(int)htPot[sKey];
					htPot[sKey] = 60000;
				}
				else
				{
					htPot.Add(sKey, 60000);
					pk.Held -= 60000;
				}
			}
			else
			{
				if ( htPot[sKey] != null )
					htPot[sKey] = (int)htPot[sKey]+pk.Held;
				else
					htPot.Add(sKey, pk.Held);
				
				if ( htPot.ContainsKey("Empty Potion Kegs") )
					htPot["Empty Potion Kegs"] = (int)htPot["Empty Potion Kegs"] + 1;
				else
					htPot.Add("Empty Potion Kegs", 1);
				pk.Delete();
			}
			from.SendMessage(88, "You added the {0} from the keg", sKey);
		}

		public void AddPot( BasePotion bp, Mobile from)
		{
			if ( htPot == null )
				htPot = new Hashtable();
			int iHave = 0;
			
			string sKey = bp.PotionEffect.ToString(), sTemp = bp.PotionEffect.ToString();
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
				if ( htPot[sKey] != null )
					iHave = (int)htPot[sKey];
			}
			if ( htPot[sKey] != null )
				iHave = (int)htPot[sKey];
			if ( iHave >= 60000 )
			{
				from.SendGump( new AlchemistKitGump( from, this ) );
				from.SendMessage(33, "You can't add more, the limit is 60,000.");
				return;
			}
			if ( htPot[sKey] != null )
				htPot[sKey] = (int)htPot[sKey]+1;
			else
				htPot.Add(sKey, 1);
			if ( htPot["Empty Bottles"] != null )
				htPot["Empty Bottles"] = (int)htPot["Empty Bottles"] + 1;
			else
				htPot.Add("Empty Bottles", 1);
			bp.Delete();
			from.SendMessage(88, "You added the {0}.", sKey);
		}

		public AlchemistKit( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 ); // version
			
			writer.Write( htPot.Count );
			foreach ( DictionaryEntry de in htPot )
			{
				writer.Write( (string)de.Key );
				writer.Write( (int)de.Value );
			}
			writer.Write( (bool)ToKeg );
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
						htPot.Add( (string)reader.ReadString(), reader.ReadInt() );
					ToKeg = reader.ReadBool();
					break;
				}
			}
		} 
	} 

	public class AlchemistKitGump : Gump
	{
		private Mobile mFrom;
		private AlchemistKit akKit;

		public AlchemistKitGump( Mobile from, AlchemistKit kit ) : base( 25, 25 )
		{
			mFrom = from;
			akKit = kit;

			from.CloseGump( typeof( AlchemistKitGump ) );

			AddPage( 0 );

			AddBackground( 0, 0, 505, 330, 5170 );
			AddPage(0);
			AddLabel( 170, 25, 25, "daat99's Alchemist Kit" );

			for (int line = 0; line < 11; line++)
			{
				if ( kit.Pot[AlchemistKit.Names[line]] != null && (int)kit.Pot[AlchemistKit.Names[line]] > 0)
				{
					AddButton( 30, 50+line*20, 4015, 4016, 100+line, GumpButtonType.Reply, 0 );
					AddLabel( 65, 50+line*20, 2118, ((int)kit.Pot[AlchemistKit.Names[line]]).ToString() );
				}
				else
				{
					AddButton( 30, 50+line*20, 4006, 4007, 999, GumpButtonType.Reply, 0 );
					AddLabel( 65, 50+line*20, 32, "0" );
				}
				AddLabel( 115, 50+line*20, 0x486, AlchemistKit.Names[line] );

				if ( kit.Pot[AlchemistKit.Names[line+11]] != null && (int)kit.Pot[AlchemistKit.Names[line+11]] > 0)
				{
					AddButton( 260, 50+line*20, 4015, 4016, 100+line+11, GumpButtonType.Reply, 0 );
					AddLabel( 295, 50+line*20, 2118, ((int)kit.Pot[AlchemistKit.Names[line+11]]).ToString() );
				}
				else
				{
					AddButton( 260, 50+line*20, 4006, 4007, 999, GumpButtonType.Reply, 0 );
					AddLabel( 295, 50+line*20, 32, "0" );
				}
				AddLabel( 345, 50+line*20, 0x486, AlchemistKit.Names[line+11] );
			}
			AddButton( 360, 280, 4006, 4007, 999, GumpButtonType.Reply, 0 );
			AddLabel( 400, 280, 32, "Add" );
			AddButton( 430, 280, 4015, 4016, 999, GumpButtonType.Reply, 0 );

			AddRadio(30, 275, 9727, 9730, !(akKit.ToKeg), 10 );
			AddLabel(65, 280, 69, "Extract Potion Bottle" );
			AddRadio(200, 275, 9727, 9730, (akKit.ToKeg), 11 );
			AddLabel(235, 280, 69, "Extract Potion Keg" );
		}

		public override void OnResponse( NetState sender, RelayInfo info)
		{
			if ( akKit.Deleted )
				return;
			int iEx;
			akKit.ToKeg = info.IsSwitched(11);
			if ( info.ButtonID == 100 )
			{
				if ((int)akKit.Pot["Empty Bottles"] > 100)
					iEx = 100;
				else
					iEx = (int)akKit.Pot["Empty Bottles"];
				mFrom.AddToBackpack( new Bottle(iEx) );
				akKit.Pot["Empty Bottles"] = (int)akKit.Pot["Empty Bottles"]-iEx;
				mFrom.SendMessage(88, "You extracted {0} empty Bottles from the {1}.", iEx, akKit.Name);
			}
			else if ( info.ButtonID == 101 )
			{
				mFrom.AddToBackpack( new PotionKeg() );
				akKit.Pot["Empty Potion Kegs"] = (int)akKit.Pot["Empty Potion Kegs"] - 1;
				mFrom.SendMessage(88, "You extracted an empty Potion Keg from the {0}.", akKit.Name);
			}
			else if ( info.ButtonID >= 102 && info.ButtonID <= 122 )
			{
				int iHave = (int)akKit.Pot[AlchemistKit.Names[info.ButtonID-100]];
				if ( iHave > 100 )
					iHave = 100;
				if ( akKit.ToKeg )
				{
					if ( akKit.Pot["Empty Potion Kegs"] == null || (int)akKit.Pot["Empty Potion Kegs"] == 0 )
					{
						mFrom.SendMessage(32, "You need atleast 1 empty potion keg to extract any potions in a keg.");
						mFrom.SendGump( new AlchemistKitGump( mFrom, akKit ) );
						return;
					}
					PotionKeg pk = new PotionKeg();
					pk.Held = iHave;
					switch (info.ButtonID)
					{
						case 102: pk.Type = PotionEffect.Nightsight;		break;
						case 103: pk.Type = PotionEffect.CureLesser;		break;
						case 104: pk.Type = PotionEffect.Cure;				break;
						case 105: pk.Type = PotionEffect.CureGreater;		break;
						case 106: pk.Type = PotionEffect.Agility;			break;
						case 107: pk.Type = PotionEffect.AgilityGreater;	break;
						case 108: pk.Type = PotionEffect.Strength;			break;
						case 109: pk.Type = PotionEffect.StrengthGreater;	break;
						case 110: pk.Type = PotionEffect.PoisonLesser;		break;
						case 111: pk.Type = PotionEffect.Poison;			break;
						case 112: pk.Type = PotionEffect.PoisonGreater;		break;
						case 113: pk.Type = PotionEffect.PoisonDeadly;		break;
						case 114: pk.Type = PotionEffect.Refresh;			break;
						case 115: pk.Type = PotionEffect.RefreshTotal;		break;
						case 116: pk.Type = PotionEffect.HealLesser;		break;
						case 117: pk.Type = PotionEffect.Heal;				break;
						case 118: pk.Type = PotionEffect.HealGreater;		break;
						case 119: pk.Type = PotionEffect.ExplosionLesser;	break;
						case 120: pk.Type = PotionEffect.Explosion;			break;
						case 121: pk.Type = PotionEffect.ExplosionGreater;	break;
						case 122: pk.Type = PotionEffect.Nightsight;		break;
					}
					akKit.Pot[AlchemistKit.Names[info.ButtonID-100]] = (int)akKit.Pot[AlchemistKit.Names[info.ButtonID-100]] - iHave;
					akKit.Pot["Empty Potion Kegs"] = (int)akKit.Pot["Empty Potion Kegs"]-1;
					mFrom.AddToBackpack( pk );
				}
				else
				{
					if ( akKit.Pot["Empty Bottles"] == null || (int)akKit.Pot["Empty Bottles"] == 0 )
					{
						mFrom.SendMessage(32, "You need atleast 1 empty bottle to extract any potions in a bottle.");
						mFrom.SendGump( new AlchemistKitGump( mFrom, akKit ) );
						return;
					}
					int iUsed = (int)akKit.Pot["Empty Bottles"], i=0;
					if ( iUsed > 10 )
						iUsed = 10;
					if ( iUsed > (int)akKit.Pot[AlchemistKit.Names[info.ButtonID-100]] )
						 iUsed = (int)akKit.Pot[AlchemistKit.Names[info.ButtonID-100]];
					for (i=0; i<iUsed;i++)
						switch ( info.ButtonID )
						{
							case 102: mFrom.AddToBackpack( new NightSightPotion() );		break;
							case 103: mFrom.AddToBackpack( new LesserCurePotion() );		break;
							case 104: mFrom.AddToBackpack( new CurePotion() );				break;
							case 105: mFrom.AddToBackpack( new GreaterCurePotion() );		break;
							case 106: mFrom.AddToBackpack( new AgilityPotion() );			break;
							case 107: mFrom.AddToBackpack( new GreaterAgilityPotion() );	break;
							case 108: mFrom.AddToBackpack( new StrengthPotion() );			break;
							case 109: mFrom.AddToBackpack( new GreaterStrengthPotion() );	break;
							case 110: mFrom.AddToBackpack( new LesserPoisonPotion() );		break;
							case 111: mFrom.AddToBackpack( new PoisonPotion() );			break;
							case 112: mFrom.AddToBackpack( new GreaterPoisonPotion() );		break;
							case 113: mFrom.AddToBackpack( new DeadlyPoisonPotion() );		break;
							case 114: mFrom.AddToBackpack( new RefreshPotion() );			break;
							case 115: mFrom.AddToBackpack( new TotalRefreshPotion() );		break;
							case 116: mFrom.AddToBackpack( new LesserHealPotion() );		break;
							case 117: mFrom.AddToBackpack( new HealPotion() );				break;
							case 118: mFrom.AddToBackpack( new GreaterHealPotion() );		break;
							case 119: mFrom.AddToBackpack( new LesserExplosionPotion() );	break;
							case 120: mFrom.AddToBackpack( new ExplosionPotion() );			break;
							case 121: mFrom.AddToBackpack( new GreaterExplosionPotion() );	break;
							case 122: mFrom.AddToBackpack( new NightSightPotion() );		break;
						}
					akKit.Pot[AlchemistKit.Names[info.ButtonID-100]] = (int)akKit.Pot[AlchemistKit.Names[info.ButtonID-100]]-iUsed;
					akKit.Pot["Empty Bottles"] = (int)akKit.Pot["Empty Bottles"]-iUsed;
				}
				mFrom.SendMessage( 2118, "You extracted the potion from the Alchemist Kit.");
				mFrom.SendGump( new AlchemistKitGump( mFrom, akKit ) );
			}
			if ( info.ButtonID == 999 )//add
			{
				akKit.ToKeg = info.IsSwitched(11);
				akKit.BeginCombine( mFrom );
			}
			if ( info.ButtonID != 0 )
				mFrom.SendGump( new AlchemistKitGump( mFrom, akKit ) );
		}
	}

	public class AlchemistKitTarget : Target
	{
		private AlchemistKit akKit;

		public AlchemistKitTarget( AlchemistKit kit ) : base( 18, false, TargetFlags.None )
		{
			akKit = kit;
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( akKit.Deleted )
				return;

			akKit.EndCombine( from, targeted );
		}
	}
}