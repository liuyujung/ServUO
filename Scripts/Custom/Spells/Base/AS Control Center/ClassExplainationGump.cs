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
using Server.Items;
using Server.Mobiles;
using Server.Commands;


namespace Server.Gumps
{ 
   public class ClassExplainationGump : Gump 
   { 
      public static void Initialize() 
      { 
         CommandSystem.Register( "ClassExplainationGump", AccessLevel.GameMaster, new CommandEventHandler( ClassExplainationGump_OnCommand ) ); 
      } 

      private static void ClassExplainationGump_OnCommand( CommandEventArgs e ) 
      { 
         e.Mobile.SendGump( new ClassExplainationGump( e.Mobile ) ); 
      } 

      public ClassExplainationGump( Mobile owner ) : base( 50,50 ) 
      { 
//----------------------------------------------------------------------------------------------------

				AddPage( 0 );
			AddImageTiled(  54, 33, 369, 400, 2624 );
			AddAlphaRegion( 54, 33, 369, 400 );

			AddImageTiled( 416, 39, 44, 389, 203 );
//--------------------------------------Window size bar--------------------------------------------
			
			AddImage( 97, 49, 9005 );
			AddImageTiled( 58, 39, 29, 390, 10460 );
			AddImageTiled( 412, 37, 31, 389, 10460 );
			AddLabel( 140, 60, 0x34, "Class Info Center for Players" );
			

			AddHtml( 107, 140, 300, 230, "<BODY>" +
//----------------------/----------------------------------------------/
"<BASEFONT COLOR=YELLOW>The Nomad: <BR><BR>The Nomad can use multiple magics but can only GM up to a maximum of 100 skill points and cannot use a powerscroll.<BR><BR>" +
"<BASEFONT COLOR=YELLOW>The Druid Tamer:<BR><BR>The Druid Tamer can use druid and bard spells. Druid spells have the important spells, such as: recall, gate and mark.<BR><BR>" +
//"<BASEFONT COLOR=YELLOW>Find my heart, it's called A Tin Man's Heart.<BR><BR> Once you kill The Borg Lord open his backpack and you should see my heart there.<BR>" +
//"<BASEFONT COLOR=YELLOW>I'll give you a Special Gift and 5000 gold coins if you give me my heart back.<BR><BR> So think about it, a lot of gold for your work!<BR>" +
//"<BASEFONT COLOR=YELLOW>Last I heard The Borg Lord was seen at a tower near the Broken Mountains in Luna.<BR><BR> I wish you the best of luck my friend!" +
						     "</BODY>", false, true);

			
			AddImage( 430, 9, 10441);
			AddImageTiled( 40, 38, 17, 391, 9263 );
			AddImage( 6, 25, 10421 );
			AddImage( 34, 12, 10420 );
			AddImageTiled( 94, 25, 342, 15, 10304 );
			AddImageTiled( 40, 427, 415, 16, 10304 );
			AddImage( -10, 314, 10402 );
			AddImage( 56, 150, 10411 );
			AddImage( 155, 120, 2103 );
			AddImage( 136, 84, 96 );

			AddButton( 225, 390, 0xF7, 0xF8, 0, GumpButtonType.Reply, 0 ); 

//--------------------------------------------------------------------------------------------------------------
      } 

      public override void OnResponse( NetState state, RelayInfo info ) //Function for GumpButtonType.Reply Buttons 
      { 
         Mobile from = state.Mobile; 

         switch ( info.ButtonID ) 
         { 
            case 0: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0 
            { 
               //Cancel 
               from.SendMessage( "Come back anytime you need Class Info!" );
               break; 
            } 

         }
      }
   }
}
