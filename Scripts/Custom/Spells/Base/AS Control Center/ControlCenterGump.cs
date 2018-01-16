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


namespace Server.LucidNagual
{
	public class ControlCenterGump : Gump
	{
		public ControlCenterGump() : base( 0, 0 )
		{
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(86, 45, 290, 389, 9200);
			this.AddImage(85, 44, 10451);
			this.AddImage(157, 44, 10451);
			this.AddImage(230, 44, 10451);
			this.AddImage(303, 44, 10451);
			this.AddImage(86, 92, 10462);
			this.AddImage(145, 92, 10462);
			this.AddImage(204, 92, 10462);
			this.AddImage(262, 92, 10462);
			this.AddImage(320, 92, 10462);
			this.AddImage(209, 40, 10450);
			this.AddImage(330, 25, 10420);
			this.AddImage(72, 27, 10430);
			this.AddImage(204, 404, 10462);
			this.AddImage(86, 404, 10462);
			this.AddImage(145, 404, 10462);
			this.AddImage(262, 404, 10462);
			this.AddImage(320, 404, 10462);
			this.AddImage(70, 419, 10460);
			this.AddImage(362, 419, 10460);
			this.AddAlphaRegion(50, 11, 360, 455);

			this.AddLabel(160, 96, 52, @"All Spells Control Center");

			this.AddLabel(190, 130, 1153, @"Optional Class System");
			this.AddLabel(190, 160, 1153, @"Optional Skin Hues");
			this.AddLabel(190, 190, 1153, @"Optional Give Books");
			this.AddLabel(190, 220, 1153, @"Optional Free Training");

			this.AddButton(110, 130, 247, 238, 1, GumpButtonType.Reply, 0);
			this.AddButton(110, 160, 247, 238, 2, GumpButtonType.Reply, 0);
			this.AddButton(110, 190, 247, 238, 3, GumpButtonType.Reply, 0);
			this.AddButton(110, 220, 247, 238, 4, GumpButtonType.Reply, 0);

			if ( ControlCenter.SetRestrictions == true )
				AddHtml( 160, 300, 145, 20, String.Format( "<basefont color=#008800>Class System enabled.</font>" ), false, false );
			else
				AddHtml( 160, 300, 145, 20, String.Format( "<basefont color=#ff0000>Class System disabled.</font>" ), false, false );

			if ( ControlCenter.SetSkinHues == true )
				AddHtml( 160, 315, 145, 20, String.Format( "<basefont color=#008800>Skin Hues enabled.</font>" ), false, false );
			else
				AddHtml( 160, 315, 145, 20, String.Format( "<basefont color=#ff0000>Skin Hues disabled.</font>" ), false, false );

			if ( ControlCenter.SetGiveBooks == true )
				AddHtml( 160, 330, 145, 20, String.Format( "<basefont color=#008800>Give Books enabled.</font>" ), false, false );
			else
				AddHtml( 160, 330, 145, 20, String.Format( "<basefont color=#ff0000>Give Books disabled.</font>" ), false, false );

			if ( ControlCenter.SetFreeSkills == true )
				AddHtml( 160, 345, 145, 20, String.Format( "<basefont color=#008800>Free Skills enabled.</font>" ), false, false );
			else
				AddHtml( 160, 345, 145, 20, String.Format( "<basefont color=#ff0000>Free Skills disabled.</font>" ), false, false );
		}
		
      	public override void OnResponse( NetState state, RelayInfo info ) //Function for GumpButtonType.Reply Buttons 
      	{ 
         		Mobile from = state.Mobile;

			if ( info.ButtonID == 1) // Class System with Restrictions
			{
				from.SendGump( new ClassSystemOptionGump( ) );
				from.PrivateOverheadMessage( MessageType.Label, 090, true,"You have selected the optional Class System.", from.NetState );
			}

			else if ( info.ButtonID == 2) // Skin Hues
			{
				from.SendGump( new SkinHueOptionGump( ) );
				from.PrivateOverheadMessage( MessageType.Label, 090, true,"You have selected the optional Skin Hue.", from.NetState );
			}

			else if ( info.ButtonID == 3) // Skin Hues
			{
				from.SendGump( new GiveBooksOptionGump( ) );
				from.PrivateOverheadMessage( MessageType.Label, 090, true,"You have selected the option to Give Books.", from.NetState );
			}
			else if ( info.ButtonID == 4) // Free Skills
			{
				from.SendGump( new FreeSkillsOptionGump( ) );
				from.PrivateOverheadMessage( MessageType.Label, 090, true,"You have selected the option for Free Training.", from.NetState );
			}
		}
	}
}