using System;
using Server;
using System.Collections;
using Server.Targeting;
using Server.Commands;
using Server.Mobiles;
using Server.Gumps;


namespace Server.ACC.CM
{
	public enum RaceREM
	{
		Human,  //Human is default.
		Drow,
		Angel,
		Elf,
		Vampire,
		Dwarf,
		Orc
	}

	public class RaceModule : Module
	{
		public static void Initialize()
		{
			CommandSystem.Register( "GetRace", AccessLevel.GameMaster, new CommandEventHandler( RaceGump_OnCommand ) );
			CommandSystem.Register( "MyRace", AccessLevel.Player, new CommandEventHandler( MyRace_OnCommand ) );
		}
		
		public override string Name(){ return "Race Module"; }
		
		public RaceREM m_RaceREM;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public RaceREM RaceREM { get{ return m_RaceREM; } set{ m_RaceREM = value; } }

		public RaceModule( Serial serial ) : this( serial, RaceREM.Human )
		{
		}

		public RaceModule( Serial serial, RaceREM race ): base( serial )
		{
			m_RaceREM = race;
		}
		
		private static void RaceGump_OnCommand( CommandEventArgs e )
		{
			RaceModule RK = ( RaceModule )CentralMemory.GetModule( e.Mobile.Serial, typeof( RaceModule ) );
			
			e.Mobile.Target = new InternalTarget( RK );
		}
		
		private static void MyRace_OnCommand( CommandEventArgs e )
		{
			RaceModule exp_mod = ( RaceModule )CentralMemory.GetModule( e.Mobile.Serial, typeof( RaceModule ) );
			
			if( exp_mod == null )
			{
				CentralMemory.AppendModule( e.Mobile.Serial, new RaceModule( e.Mobile.Serial, RaceREM.Human ), true );
				e.Mobile.SendMessage( "Please try again" );
			}
			else
			{
				e.Mobile.SendMessage( "My race is: {0}", exp_mod.m_RaceREM );
			}
		}

		public override void Append( Module mod, bool negatively )
		{
			RaceModule rm = mod as RaceModule;

			RaceREM = rm.RaceREM;
		}

		private class InternalTarget : Target
		{
			private RaceModule r_mod;
			
			public InternalTarget( RaceModule mod ) : base( 1, false, TargetFlags.None )
			{
				r_mod = mod;
			}
			
			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is PlayerMobile )
				{
					PlayerMobile pm = ( PlayerMobile )targeted;
					RaceModule RK = ( RaceModule )CentralMemory.GetModule( pm.Serial, typeof( RaceModule ) );
					
					if ( RK != null )
						from.SendGump( new PropertiesGump( from, RK ) );
					else
						from.SendMessage( "This player does not have a race." );
				}
				else
					from.SendMessage("Can Only Target PLAYERS!");
			}
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( ( int )0 );  //--<< Version Number >>--------------<<

			writer.Write( ( int )m_RaceREM );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			m_RaceREM = ( RaceREM )reader.ReadInt();
		}
	}
}
