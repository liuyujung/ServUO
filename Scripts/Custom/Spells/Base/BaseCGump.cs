/*
Special thanks to Ryan.
With RunUO we now have the ability to become our own Richard Garriott.
All Spells System created by x-SirSly-x, Admin of Land of Obsidian.
All Spells System 4.0 created & supported by Lucid Nagual, Admin of The Conjuring.
All Spells System 5.0 created by A_li_N.
All Spells Optional Restrictive System created by Alien, Daat99 and Lucid Nagual.
    _________________________________
 -=(_)_______________________________)=-
   /   .   .   . ____  . ___      _/
  /~ /    /   / /     / /   )2005 /
 (~ (____(___/ (____ / /___/     (
  \ ----------------------------- \
   \     lucidnagual@gmail.com     \
    \_     ===================      \
     \   -Admin of "The Conjuring"-  \
      \_     ===================     ~\
       )       All Spells System       )
      /~     Version [5].0 & Above   _/
    _/_______________________________/
 -=(_)_______________________________)=-
 */
using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Gumps
{
	public abstract class BaseCGump : Gump
	{
		public Spellbook m_Book;
		public string[]  m_Defs;
		
		public abstract int TextHue{ get; }
		public abstract int BGImage{ get; }
		public abstract int SpellBtn{ get; }
		public abstract int SpellBtnP{ get; }
		public abstract string MLab{ get; }
		public abstract int Horz{ get; }
		public abstract int Vert{ get; }
		
		public BaseCGump( Mobile from, Spellbook book ) : base( 150, 200 )
		{
			m_Book = book;
			
			AddBackground();
			AddPage(1);
			
			AddLabel( 150, 17, TextHue, MLab );
			AddSpells();
		}
		
		private void AddBackground()
		{
			AddPage(0);
			AddImage( Horz, Vert, BGImage, 0 );
		}
		
		public virtual void AddSpells()
		{
			int NextPage = 2;
			
			for( int i = 0; i < m_Book.BookCount; i++ )
			{
				if( HasSpell( m_Book.BookOffset + i ) )
				{
					string[] Def = ParseDefFor( m_Book.BookOffset + i );
					if( Def[0] != null )
					{
						if( i > 0 && i % 16 == 0 )
						{
							AddButton( 396, 15, 2206, 2206, 0, GumpButtonType.Page, NextPage );
							AddPage(NextPage);
							AddButton( 123, 15, 2205, 2205, 0, GumpButtonType.Page, NextPage-1 );
							//AddButton( 125, 105, micon, micon, 1, GumpButtonType.Reply, 1 );
							NextPage++;
						}
						else if( i > 15 )
						{
							AddLabel( 145, 40+((i-15)*20), TextHue, Def[0] );
							AddButton( 125, 43+((i-15)*20), SpellBtn, SpellBtnP, m_Book.BookOffset+i, GumpButtonType.Reply, 0 );
						}
						else
						{
							AddLabel( (i>7?315:145), 40+(i>7?(i-8)*20:i*20), TextHue, Def[0] );
							AddButton( (i>7?295:125), 43+(i>7?(i-8)*20:i*20), SpellBtn, SpellBtnP, m_Book.BookOffset+i, GumpButtonType.Reply, 0 );
						}
					}
				}
			}
			
			for( int i = 0; i < m_Book.BookCount; i++ )
			{
				if( HasSpell( m_Book.BookOffset + i ) )
				{
					string[] Def = ParseDefFor( m_Book.BookOffset + i );
					int Y = 35;
					
					AddButton( 396, 14, 2206, 2206, 0, GumpButtonType.Page, NextPage );
					AddPage(NextPage);
					AddButton( 123, 14, 2205, 2205, 0, GumpButtonType.Page, NextPage-1 );
					NextPage++;
					
					if( Def[0] != null )
						AddLabel( 150, 37, TextHue, Def[0] );
					if( Def[1] != null )
						AddHtml( 130, 60, 130, 145, "<basefont color=black>" + Def[1] + "</font>", false, (Def[1].Length > 130) );
					if( Def[2] != null )
					{
						string[] Regs = Def[2].Split(';');
						AddLabel( 295, Y, TextHue, "Reagents :" );
						Y += 20;
						foreach( string r in Regs )
						{
							AddLabel( 300, Y, TextHue, r.TrimStart() );
							Y += 20;
							if( Y > 185 )
								break;
						}
						Y += 10;
					}
					if( Def[3] != null && Y <= 185 )
					{
						string[] Info = Def[3].Split(';');
						foreach( string s in Info )
						{
							AddLabel( 295, Y, TextHue, s.TrimStart() );
							Y += 20;
							if( Y > 185 )
								break;
						}
					}
				}
			}
			
			AddButton( 396, 14, 2206, 2206, 0, GumpButtonType.Page, NextPage );
			AddPage(NextPage);
			AddButton( 123, 14, 2205, 2205, 0, GumpButtonType.Page, NextPage-1 );
			AddHtml( 293, 40, 130, 135, "<BODY>" +
			        "<BASEFONT COLOR=BLACK>Made Possible by:<BR>" +
			        "<BASEFONT COLOR=GREY>Voran<BR>" +
			        "<BASEFONT COLOR=GREY>Darsden<BR>" +
			        "<BASEFONT COLOR=GREY>TheOutcastDev<BR>" +
			        "<BASEFONT COLOR=GREY>wstsdwgrrr<BR>" +
			        "<BASEFONT COLOR=GREY>joshw<BR>" +
			        "<BASEFONT COLOR=GREY>ssalter<BR>" +
			        "<BASEFONT COLOR=GREY>Lucid Nagual<BR>" +
			        "<BASEFONT COLOR=GREY>X-SirSly-X<BR>" +
			        "<BASEFONT COLOR=GREY>Greystar<BR>" +
			        "<BASEFONT COLOR=GREY><BR>" +
			        "<BASEFONT COLOR=GREY>and finally myself...<BR>" +
			        "<BASEFONT COLOR=GREY>A_Li_N<BR>" +
			        "</BODY>", false, true );
		}
		
		private string[] ParseDefFor( int spellID )
		{
			string[] def = SpellDefRegistry.GetDefFor( spellID );
			return def;
		}
		
		public bool HasSpell( int spellID )
		{
			return( m_Book != null && m_Book.HasSpell( spellID ) );
		}
	}
}
