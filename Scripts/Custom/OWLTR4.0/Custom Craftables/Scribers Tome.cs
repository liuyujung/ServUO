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

description: a small tome that let you store scrolls.
Idea by Beldr
*/
using System;
using System.Collections;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class ScribersTome : Item
	{
		private ArrayList al_GlobalEntry;
		public ArrayList GlobalEntry{ get{ return al_GlobalEntry; } }

		[Constructable]
		public ScribersTome() : base( 0xEFA )
		{
			Movable = true;
			Weight = 1.0;
			Hue = 88;
			Name = "Scribers Tome";
			//LootType = LootType.Blessed;
			InitArray();
		}

		public void InitArray()
		{
			if ( al_GlobalEntry == null )
				al_GlobalEntry = new ArrayList();
			for ( int i=0; i < 116; i++ )
			{
				if (i==64)
					i=100;
				al_GlobalEntry.Add( new ScrollEntry( i, 1, 0 ) );
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( al_GlobalEntry == null )
				InitArray();

			if (!(from is PlayerMobile))
				return;
			if ( IsChildOf(from.Backpack) || IsChildOf(from.BankBox) )
				from.SendGump( new ScribersTomeGump( from, this ) );
			else
				from.SendMessage("The Scribers Tome must be in your backpack or your bank box.");
		}
		
		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( dropped is SpellScroll && dropped.Amount >= 1 && dropped.Amount <= 60000 )
				AddScroll( (SpellScroll)dropped, from, false );
			return false;
		}
		
		public void BeginCombine( Mobile from ) { from.Target = new ScribersTomeTarget( this ); }		
		
		public void EndCombine( Mobile from, object o )
		{
			if (!(o is SpellScroll))
			{
				from.SendMessage(32, "That isn't a Scroll");
				return;
			}
			AddScroll( (SpellScroll)o, from, true );
		}

		public void AddScroll( SpellScroll sps, Mobile from, bool gump )
		{
			if ( al_GlobalEntry == null )
				InitArray();
			int sid = sps.SpellID;
			if ( sid >= 100 && sid <= 115 )
				sid -= 36;
			else if ( sid > 64 )
			{
				from.SendMessage(33, "You can't add this scroll");
				return;
			}
			if ( ((ScrollEntry)al_GlobalEntry[sid]).Amount >= 60000 )
			{
				from.SendMessage(33, "You can't add more charges, the limit is 60,000.");
				return;
			}
			else if ( ((ScrollEntry)al_GlobalEntry[sid]).Amount + sps.Amount > 60000 )
			{
				sps.Amount = (((ScrollEntry)al_GlobalEntry[sid]).Amount + sps.Amount) - 60000;
				((ScrollEntry)al_GlobalEntry[sid]).Amount = 60000;
			}
			else
			{
				((ScrollEntry)al_GlobalEntry[sid]).Amount += sps.Amount;
				sps.Delete();
			}
			if ( ((ScrollEntry)al_GlobalEntry[sid]).SItemID == 1 )
				((ScrollEntry)al_GlobalEntry[sid]).SItemID = sps.ItemID;
			from.SendMessage(88, "You added the scrolls.");
			if ( gump )
				from.SendGump( new ScribersTomeGump( from, this ) );
		}

		public ScribersTome( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 ); // version
			if (al_GlobalEntry == null)
				writer.Write( (int)0 );
			else
			{
				writer.Write( (int)al_GlobalEntry.Count );
				for (int i=0; i < al_GlobalEntry.Count; i++)
				{
					writer.Write( (int)((ScrollEntry)al_GlobalEntry[i]).SpellID );
					writer.Write( (int)((ScrollEntry)al_GlobalEntry[i]).SItemID );
					writer.Write( (int)((ScrollEntry)al_GlobalEntry[i]).Amount );
				}
			}
		}
		
		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt();
			if ( al_GlobalEntry == null )
				al_GlobalEntry = new ArrayList();
			int l = reader.ReadInt();
			for (int i=0; i < l; i++)
				al_GlobalEntry.Add( new ScrollEntry(reader.ReadInt(), reader.ReadInt(), reader.ReadInt()) );
		}

		public static string[] ScrollsNames = new string[80]
		{
			"Clumsy",			"Create Food",				"Feeblemind",				"Heal",
			"Magic Arrow",		"Night Sight",				"Reactive Armor",			"Weaken",
			"Agility",			"Cunning",					"Cure",						"Harm",
			"Magic Trap",		"Magic Untrap",				"Protection",				"Strength",
			"Bless",			"Fireball",					"Magic Lock",				"Poison",
			"Telekinisis",		"Teleport",					"Unlock",					"Wall Of Stone",
			"Arch Cure",		"Arch Protection",			"Curse",					"Fire Field",
			"Greater Heal",		"Lightning",				"Mana Drain",				"Recall",
			"Blade Spirits",	"Dispel Field",				"Incognito",				"Magic Reflect",
			"Mind Blast",		"Paralyze",					"Poison Field",				"Summon Creature",
			"Dispel",			"Energy Bolt",				"Explosion",				"Invisibility",
			"Mark",				"Mass Curse",				"Paralyze Field",			"Reveal",
			"Chain Lightning",	"Energy Field",				"Flamestrike",				"Gate Travel",
			"Mana Vampire",		"Mass Dispel",				"Meteor Swarm",				"Polymorph",
			"Earthquake",		"Energy Vortex",			"Resurrection",				"Summon Air Elemental",
			"Summon Daemon",	"Summon Earth Elemental",	"Summon Fire Elemental",	"Summon Water Elemental",
			"Animate Dead",		"BloodOath",				"Corpse Skin",				"Curse Weapon",
			"Evil Omen",		"Horrific Beast",			"Lich Form",				"Mind Rot",
			"Pain Spike",		"Poison Strike",			"Strangle",					"Summon Familiar",
			"Vampiric Embrace",	"Vengeful Spirit",			"Wither",					"Wraith Form"
		};
	}

	public class ScribersTomeGump : Gump
	{
		private Mobile m_From;
		private ScribersTome st_Tome;

		public ScribersTomeGump( Mobile from, ScribersTome tome ) : base( 25, 25 )
		{
			if ( tome.GlobalEntry == null )
				tome.InitArray();

			m_From = from;
			st_Tome = tome;

			from.CloseGump( typeof( ScribersTomeGump ) );

			AddPage( 0 );

			AddBackground( 0, 0, 790, 440, 3000 );
			AddPage(0);
			AddLabel( 310, 7, 25, "daat99's Scribers Tome" );

			AddButton( 5, 10, 2462, 2461, 1, GumpButtonType.Reply, 0 );
			AddButton( 730, 10, 2462, 2461, 2, GumpButtonType.Reply, 0 );
			int x = 5, y = 0;
			for ( int i = 0; i < 80; i++ )
			{
				switch (i)
				{
					case 20: x = 175; y = 0; break;
					case 40: x = 355; y = 0; break;
					case 60: x = 560; y = 0; break;
				}
				AddButton( x, 28+y*20, 2443, 2444, i+100, GumpButtonType.Reply, 0 );
				AddHtml( x+10, 30+y*20, 70, 20, ((i%2==0) ? "<basefont color=#f8f8f8>" : "<basefont color=#0000ff>") + ((ScrollEntry)tome.GlobalEntry[i]).Amount.ToString() +"</basefont>", false, false );
				AddHtml( x+70, 30+y*20, 155, 20, "<basefont color=#0000ff>"+ ScribersTome.ScrollsNames[i] +"</basefont>", false, false );
				y++;
			}
		}

		public override void OnResponse( NetState sender, RelayInfo info)
		{
			if ( st_Tome.Deleted || st_Tome.GlobalEntry == null )
				return;
			if ( info.ButtonID == 1 || info.ButtonID == 2 )
			{
				st_Tome.BeginCombine( m_From );
				m_From.SendGump( new ScribersTomeGump( m_From, st_Tome ) );
			}
			else if ( info.ButtonID >= 100 && info.ButtonID < 180 )
			{
				int amount = ((ScrollEntry)st_Tome.GlobalEntry[info.ButtonID-100]).Amount;
				if ( amount == 0 )
					st_Tome.BeginCombine( m_From );
				else
				{
					if ( (int)amount/100 >= 1 )
						amount = 100;
					else if ( (int)amount/10 >= 1 )
						amount = 10;
					((ScrollEntry)st_Tome.GlobalEntry[info.ButtonID-100]).Amount -= amount;
					if (info.ButtonID == 131)
						m_From.AddToBackpack(new RecallScroll( amount ));
					else if (info.ButtonID == 151)
						m_From.AddToBackpack(new GateTravelScroll( amount ));
					else
						m_From.AddToBackpack(new SpellScroll( ((ScrollEntry)st_Tome.GlobalEntry[info.ButtonID-100]).SpellID, ((ScrollEntry)st_Tome.GlobalEntry[info.ButtonID-100]).SItemID, amount ));
				}
				m_From.SendGump( new ScribersTomeGump( m_From, st_Tome ) );
			}
		}
	}

	public class ScribersTomeTarget : Target
	{
		private ScribersTome st_Tome;

		public ScribersTomeTarget( ScribersTome house ) : base( 18, false, TargetFlags.None )
		{
			st_Tome = house;
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( st_Tome.Deleted )
				return;

			st_Tome.EndCombine( from, targeted );
		}
	}

	public class ScrollEntry
	{
		private int i_SpellID, i_SItemID, i_Amount;

		public int SpellID{ get{ return i_SpellID; } set { i_SpellID = value; } }
		public int SItemID{ get{ return i_SItemID; } set{ i_SItemID = value; } }
		public int Amount{ get{ return i_Amount; } set{ i_Amount = value; } }

		public ScrollEntry( int spellid, int sitemid, int amount )
		{
			i_SpellID = spellid;
			i_SItemID = sitemid;
			i_Amount = amount;
		}
	}
}