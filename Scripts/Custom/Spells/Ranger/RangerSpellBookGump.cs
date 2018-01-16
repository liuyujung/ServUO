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
using Server;
using Server.Gumps;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.Spells.Ranger
{
	public class RangerSpellbookGump : BaseCGump
	{
		public override int TextHue{   get{ return 2307; } }
		public override int BGImage{   get{ return 2201; } }
		public override int SpellBtn{  get{ return 2362; } }
		public override int SpellBtnP{ get{ return 2361; } }
		public override string MLab{   get{ return "Ranger Spells"; } }
		public override int Horz{   get{ return 100; } }
		public override int Vert{   get{ return 10; } }
		
		public RangerSpellbookGump( Mobile from, RangerSpellbook book ) : base( from, book )
		{
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;
			int BID = info.ButtonID;

			if( BID >= 900 && BID <= 912 )
			{
				Spell spell = SpellRegistry.NewSpell( BID, from, null );
				if( spell != null )
					spell.Cast();
			}
		}
	}
}
