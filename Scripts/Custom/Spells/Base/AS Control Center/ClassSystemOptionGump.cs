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
	public class ClassSystemOptionGump : Gump
	{
		public bool IsRestricted = false;

		public ClassSystemOptionGump() : base( 0, 0 )
		{
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(42, 33, 310, 171, 9200);
			this.AddAlphaRegion(26, 23, 342, 191);
			this.AddLabel(91, 47, 15, @"This option sets up the class ");
			this.AddLabel(101, 71, 15, @"system with restrictions.");
			this.AddLabel(58, 98, 15, @"Would you like to enable this system?");
			this.AddLabel(127, 145, 0, @"Yes");
			this.AddLabel(238, 145, 0, @"No");
			this.AddButton(101, 145, 208, 209, 1, GumpButtonType.Reply, 0);
			this.AddButton(209, 146, 208, 209, 2, GumpButtonType.Reply, 0);
		}
		
		public void GenRestrictions( Mobile from, int all )
		{
			if ( all == 1 && ControlCenter.SetRestrictions == false )
				from.PrivateOverheadMessage( MessageType.Label, 090, true,"You have successfully enabled the Class System.", from.NetState );
				ControlCenter.SetRestrictions = true;
				//return;
			
			if ( all == 1 && ControlCenter.SetRestrictions == true )
				from.PrivateOverheadMessage( MessageType.Label, 090, true,"You already have the Class System enabled.", from.NetState );
				return;
		}

		public void GenNoRestrictions( Mobile from, int all )
		{
			//Restrictions set to false
			if ( all == 2 && ControlCenter.SetRestrictions == true )
				from.PrivateOverheadMessage( MessageType.Label, 090, true,"You have successfully disabled the Class System.", from.NetState );
				ControlCenter.SetRestrictions = false;
				//return;

			//if ( all == 2 && SpellRestrictions.Restricted == true )
				//from.PrivateOverheadMessage( MessageType.Label, 090, true,"You have successfully disabled the Class System.", from.NetState );
			//	SpellRestrictions.Restricted = false;
				//return;
			//Restrictions set to false
			
			//Restrictions set to true
			if ( all == 2 && ControlCenter.SetRestrictions == false )
				from.PrivateOverheadMessage( MessageType.Label, 090, true,"The Class System is already disabled.", from.NetState );
				//return;
			
			//if ( all == 2 && SpellRestrictions.Restricted == false )
			//	from.PrivateOverheadMessage( MessageType.Label, 090, true,"The Class System is already disabled.", from.NetState );
				return;
			//Restrictions set to true
		}

      	public override void OnResponse( NetState state, RelayInfo info ) //Function for GumpButtonType.Reply Buttons 
      	{ 
         	Mobile from = state.Mobile;
			
			if ( info.ButtonID == 1) // Class System without Restrictions
			{
				GenRestrictions( from, 1 );
			}

			else if ( info.ButtonID == 2) // Class System without Restrictions
			{
				GenNoRestrictions( from, 2 );

			}
		}
	}
}
