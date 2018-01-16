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
using Server.ContextMenus;
using Server.Network;
using Server.Spells;


namespace Server.LucidNagual
{
	public class SkinHueOptionGump : Gump
	{
		public SkinHueOptionGump() : base( 0, 0 )
		{
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(42, 33, 310, 171, 9200);
			this.AddAlphaRegion(26, 23, 342, 191);
			this.AddLabel(100, 47, 15, @"This option sets up the skin ");
			this.AddLabel(140, 71, 15, @"hue options.");
			this.AddLabel(58, 98, 15, @"Would you like to enable this system?");
			this.AddLabel(127, 145, 0, @"Yes");
			this.AddLabel(238, 145, 0, @"No");
			this.AddButton(101, 145, 208, 209, 1, GumpButtonType.Reply, 0);
			this.AddButton(209, 146, 208, 209, 2, GumpButtonType.Reply, 0);
		}
		
		public void GenClassHues( Mobile from, int all )
		{
			if ( all == 1 && ControlCenter.SetSkinHues == false )
				from.PrivateOverheadMessage( MessageType.Label, 090, true,"You have successfully enabled the Class Hues.", from.NetState );
				ControlCenter.SetSkinHues = true;
				//return;
			
			if ( all == 1 && ControlCenter.SetSkinHues == true )
				from.PrivateOverheadMessage( MessageType.Label, 090, true,"You already have the Class Hues enabled.", from.NetState );
				return;
		}

		public void GenNoClassHues( Mobile from, int all )
		{
			if ( all == 2 && ControlCenter.SetSkinHues == true )
				from.PrivateOverheadMessage( MessageType.Label, 090, true,"You have successfully disabled the Class hues.", from.NetState );
				ControlCenter.SetSkinHues = false;
				//return;
			
			if ( all == 2 && ControlCenter.SetSkinHues == false )
				from.PrivateOverheadMessage( MessageType.Label, 090, true,"The Class Hues are already disabled.", from.NetState );
				return;
		}

      	public override void OnResponse( NetState state, RelayInfo info ) //Function for GumpButtonType.Reply Buttons 
      	{ 
         	Mobile from = state.Mobile;
			
			if ( info.ButtonID == 1) // Class System without Restrictions
			{
				GenClassHues( from, 1 );
			}

			else if ( info.ButtonID == 2) // Class System without Restrictions
			{
				GenNoClassHues( from, 2 );

			}
		}
	}
}