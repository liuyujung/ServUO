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
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.ACC.CM;
using Server.Commands;


namespace Server.LucidNagual
{
	public class ControlCenter : Item
	{
		//Set to false if you choose not to have restrictions.
		//Can also set in game at the Control Center.
		public static bool SetRestrictions = false;
		
		//Set to false if you choose not to have skin hues.
		//Can also set in game at the Control Center.
		public static bool SetSkinHues = true;
		
		//Set to false if you choose not to give books for free.
		//Can also set in game at the Control Center.
		public static bool SetGiveBooks = false;
		
		//Set to false if you charge or true give skills for free.
		//Can also set in game at the Control Center.
		public static bool SetFreeSkills = false;
		
		//Set to false you you do not want to use the characterization system.
		//Can also set in game at the Control Center.
		public static bool EnableCharacterization = true;

		//Set to false you you do not want to use the characterization system.
		//Can also set in game at the Control Center.
		public static bool EnableNameProperties = true;
		
		public Mobile from;
		public static ControlCenter CntrlC;
		public bool UnLocked = false;
		private static bool TramSpawn = false;
		private static bool FelSpawn  = false;
		private static bool TokSpawn  = false;
		
		[Constructable]
		public ControlCenter() : base( 0x1E5E )
		{
			Hue = 168;
			Name = "All Spells Control & Information Center";
			Movable = false;
			Visible = false;
			Light = LightType.Circle300;
		}
		
		public override void Delete()
		{
			if ( UnLocked )
				base.Delete();
			else
				return;
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if (from.AccessLevel <= AccessLevel.GameMaster)
				from.SendGump( new ClassExplainationGump( from ) );
			
			if (from.AccessLevel == AccessLevel.Administrator)
				from.SendGump( new ControlCenterGump() );
			
			else
				return;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "ASCC",   AccessLevel.GameMaster, new CommandEventHandler( ASCC_OnCommand ) );
			CommandSystem.Register( "CCount", AccessLevel.GameMaster, new CommandEventHandler( CCount_OnCommand ) );
			CommandSystem.Register( "CClean", AccessLevel.Administrator, new CommandEventHandler( CClean_OnCommand ) );
			CommandSystem.Register( "CCGen",  AccessLevel.Administrator, new CommandEventHandler( CCGen_OnCommand ) );
			CommandSystem.Register( "GoCC",  AccessLevel.Administrator, new CommandEventHandler( GoCC_OnCommand ) );
			CommandSystem.Register( "ASInfo",  AccessLevel.Player, new CommandEventHandler( ASInfo_OnCommand ) );
			CommandSystem.Register( "LockCC",  AccessLevel.Administrator, new CommandEventHandler( LockCC_OnCommand ) );
			CommandSystem.Register( "UnLockCC",  AccessLevel.Administrator, new CommandEventHandler( UnLockCC_OnCommand ) );
			
			EventSink.Login += new LoginEventHandler( OnLogin );
			EventSink.Disconnected += new DisconnectedEventHandler( EventSink_Disconnected );
			
			if ( CntrlC == null )
			{
				GenTramSpawn();
				GenFelSpawn();
				GenTokSpawn();
			}
			else
				return;
			
			//m_Entry[targeted] = new Characterization( m_Class, m_Race, m_Tribe, m_Deity );
			
		}
		
		[Usage( "ASCC" )]
		[Description( "Open the All Spells Control Center Gump." )]
		public static void ASCC_OnCommand( CommandEventArgs e )
		{
			if (!( e.Mobile is PlayerMobile ))
				return;
			else if ( e.Mobile.AccessLevel >= AccessLevel.GameMaster )
				e.Mobile.SendGump(new ControlCenterGump( ));
			else
				return;
		}
		
		[Usage( "CClean" )]
		[Description( "Deletes all control centers." )]
		public static void CClean_OnCommand( CommandEventArgs e )
		{
			//Remove ControlCenters [start]
			ArrayList cc = new ArrayList();
			
			foreach (Item item in World.Items.Values)
			{
				if (item is ControlCenter)
					cc.Add(item);
			}
			
			Console.WriteLine("There were {0} ControlCenters", cc.Count);
			
			foreach ( Item item in cc )
				item.Delete();
			
			TramSpawn = false; FelSpawn = false; TokSpawn = false;
			
			if ( cc.Count > 0 )
				World.Broadcast( 0x35, true, "{0} Control Centers have been removed from the world.", cc.Count );
			else
				return;
			//Remove ControlCenters [end]
			
			
			//Clean Internal Map [start]
			ArrayList intern = new ArrayList();
			
			foreach (Item item in World.Items.Values)
			{
				if ( item is ControlCenter && item.Map == Map.Internal )
					intern.Add(item);
			}
			
			foreach ( Item item in intern )
				item.Delete();
			
			if ( cc.Count > 0 )
				World.Broadcast( 0x35, true, "{0} internal items have been removed from the world.", cc.Count );
			
			else
				return;
			//Clean Internal Map [end]
		}
		
		[Usage( "CCount" )]
		[Description( "Counts all control centers in world." )]
		public static void CCount_OnCommand( CommandEventArgs e )
		{
			ArrayList cc = new ArrayList();
			
			foreach (Item item in World.Items.Values)
			{
				if (item is ControlCenter)
					cc.Add(item);
			}
			
			World.Broadcast( 0x35, true, "There are {0} Control Centers in the world.", cc.Count );
			Console.WriteLine("There are {0} ControlCenters", cc.Count);
		}
		
		[Usage( "CCGen" )]
		[Description( "Generate control centers." )]
		public static void CCGen_OnCommand( CommandEventArgs e )
		{
			if ( CntrlC == null )
			{
				GenTramSpawn();
				GenFelSpawn();
				GenTokSpawn();
			}
			
			else
				World.Broadcast( 0x35, true, "The control centers are already generated." );
		}
		
		[Usage( "GoCC" )]
		[Description( "Recall to control center." )]
		public static void GoCC_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;
			from.PlaySound( 0x1FC );
			from.Map = Map.Felucca;
			from.Location = new Point3D(1499,1586,10);
			from.PlaySound( 0x1FC );
		}
		
		[Usage( "ASInfo" )]
		[Description( "Info gump for players." )]
		public static void ASInfo_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;
			from.SendGump( new ClassExplainationGump( from ) );
		}
		
		[Usage( "LockCC" )]
		[Description( "Admin can lock control centers." )]
		public static void LockCC_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;
			
			if( from.AccessLevel == AccessLevel.Administrator )
			{
				if( CntrlC != null && CntrlC.UnLocked == true )
					CntrlC.UnLocked = false;
			}
			else
				return;
		}
		
		[Usage( "UnLockCC" )]
		[Description( "Admin can unlock control centers." )]
		public static void UnLockCC_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;
			
			if( from.AccessLevel == AccessLevel.Administrator )
			{
				if( CntrlC != null && CntrlC.UnLocked == false )
					CntrlC.UnLocked = true;
			}
			else
				return;
		}
		
		public static void GenTramSpawn()
		{
			if ( !TramSpawn )
			{
				TramSpawn = true;
				ControlCenter tram_cc = new ControlCenter();
				tram_cc.MoveToWorld( new Point3D(1500,1584,10), Map.Trammel );
			}
			
			else
				return;
		}
		
		public static void GenFelSpawn()
		{
			if ( !FelSpawn )
			{
				FelSpawn = true;
				ControlCenter fel_cc = new ControlCenter();
				fel_cc.MoveToWorld( new Point3D(1500,1584,10), Map.Felucca );
			}
			
			else
				return;
		}
		
		public static void GenTokSpawn()
		{
			if ( !TokSpawn )
			{
				TokSpawn = true;
				ControlCenter tok_cc = new ControlCenter();
				tok_cc.MoveToWorld( new Point3D(728,1271,25), Map.Tokuno );
			}
			
			else
				return;
		}
		
		private static void OnLogin( LoginEventArgs e )
		{
			Mobile from = e.Mobile;
			ClassREMModule class_mod = (ClassREMModule)CentralMemory.GetModule( from.Serial, typeof( ClassREMModule ) );
			RaceModule race_mod = (RaceModule)CentralMemory.GetModule( from.Serial, typeof( RaceModule ) );
			//LevelKeeperModule exp_mod = (LevelKeeperModule)CentralMemory.GetModule( from.Serial, typeof( LevelKeeperModule ) );
			
			if( ASSettings.EnableClassSystem )
			{
				if ( class_mod == null )
					from.SendGump( new ClassGump( from ) );
				else
					return;
			}
			else if( ASSettings.EnableRaceSystem )
			{			
				//if ( race_mod == null )
				//	from.SendGump( new RavensRaceGump( from, RavensRacePage.Info ) );
				//else if ( exp_mod == null )
				//	CentralMemory.AppendModule( from.Serial, new LevelKeeperModule( from.Serial ), true );
				//else
				//	return;
			}
			else
				return;
		}
		
		private static void EventSink_Disconnected( DisconnectedEventArgs e )
		{
			Mobile from = e.Mobile;
		}
		
		public ControlCenter( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0  ); // version
			
			//Version 3 [start]
			writer.Write( (bool)TramSpawn );
			writer.Write( (bool)FelSpawn );
			writer.Write( (bool)TokSpawn );
			writer.Write( (bool)UnLocked );
			//Version 3 [end]
			
			//Version 2 [start]
			writer.Write( (bool)SetFreeSkills );
			writer.Write( (bool)SetGiveBooks );
			writer.Write( (bool)SetRestrictions );
			writer.Write( (bool)SetSkinHues );
			//Version 2 [end]
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			
			//Version 3 [start]
			TramSpawn = reader.ReadBool();
			FelSpawn = reader.ReadBool();
			TokSpawn = reader.ReadBool();
			UnLocked = reader.ReadBool();
			//Version 3 [end]
			
			//Version 2 [start]
			SetFreeSkills = reader.ReadBool();
			SetGiveBooks = reader.ReadBool();
			SetRestrictions = reader.ReadBool();
			SetSkinHues = reader.ReadBool();
			//Version 2 [end]
		}
	}
}
