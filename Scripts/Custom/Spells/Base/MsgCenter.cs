using System;
using System.Collections;
using Server;
//using Server.daat99;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Engines.BulkOrders;


namespace Server.LucidNagual
{
	public class MsgCenter
	{
		public static Msglist[] Msg;
		
		public static void Initialize()
		{
			if (Msg == null)
				MsgList();
		}
		
		public static void MsgList()
		{
			Msg = new Msglist[18];
			Msg[0] = new Msglist( "You do not have enough gold to purchase that item." );
			Msg[1] = new Msglist( "Gold has been withdrawn from your bankbox." );
			Msg[2] = new Msglist( "Gold has been withdrawn from your backpack." );
			Msg[3] = new Msglist( "Thank you for your purchase." );
			Msg[4] = new Msglist( "Your class cannot open this spellbook." );
			Msg[5] = new Msglist( "Your race cannot use this item." );
			Msg[6] = new Msglist( "A spellboook has been placed into your backpack." );
			Msg[7] = new Msglist( "Spellbooks are sold at the Scribe." );
			Msg[8] = new Msglist( "Congradulations, you have chosen your class." );
			Msg[9] = new Msglist( "Congradulations, you have chosen your race." );
			Msg[10] = new Msglist( "Congradulations, you have chosen your tribe." );
			Msg[11] = new Msglist( "Congradulations, you have chosen your alignment." );
			Msg[12] = new Msglist( "Congradulations, you have chosen your deity." );
			Msg[13] = new Msglist( "You do not have that spell." );
			Msg[14] = new Msglist( "Your class cannot cast that spell." );
			Msg[15] = new Msglist( "You are out of reagents." );
			Msg[16] = new Msglist( "You are out of tithe." );
			Msg[17] = new Msglist( "You cannot delete the Control Center!" );
			
		}
	}
	
	public class Msglist
	{
		private string s_Text;
		public string Text{ get{ return s_Text;} set{ s_Text = value;} }
		
		public Msglist( string text )
		{
			s_Text = text;
		}
	}
}
