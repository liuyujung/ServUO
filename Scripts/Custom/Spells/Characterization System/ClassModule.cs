using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Targeting;
using Server.Mobiles;
using Server.Network;
using Server.Commands;


namespace Server.ACC.CM
{
	public enum ClassREM
	{
		None,      //None is default.
		Nomad,
		Cleric,
		Druid,
		Bard,
		Ranger,
		Rogue,
		Farmer,
		Crafter,
		Paladin,
		Mage,
		Necromancer,
		Ninja,
		Samurai,
		Arcanist,
		Tamer
	}
	
	public class ClassREMModule : Module
	{
		public static void Initialize()
		{
			CommandSystem.Register( "GetClass", AccessLevel.GameMaster, new CommandEventHandler( ClassGump_OnCommand ) );
			CommandSystem.Register( "MyClass", AccessLevel.Player, new CommandEventHandler( MyClass_OnCommand ) );
		}
		
		public override string Name(){ return "Class Module"; }
		
		private ClassREM m_ClassREM;
		
		[CommandProperty( AccessLevel.GameMaster)]
		public ClassREM ClassREM
		{
			get{ return m_ClassREM; }
			set{ m_ClassREM = value; }
		}
		
		public ClassREMModule( Serial serial ) : this( serial, ClassREM.None )
		{
		}
		
		public ClassREMModule( Serial serial, ClassREM c_class ): base( serial )
		{
			ClassREM = c_class;
		}
		
		private static void ClassGump_OnCommand( CommandEventArgs e )
		{
			ClassREMModule RK = ( ClassREMModule )CentralMemory.GetModule( e.Mobile.Serial, typeof( ClassREMModule ) );
			
			e.Mobile.Target = new InternalTarget( RK );
		}
		
		private static void MyClass_OnCommand( CommandEventArgs e )
		{
			ClassREMModule exp_mod = ( ClassREMModule )CentralMemory.GetModule( e.Mobile.Serial, typeof( ClassREMModule ) );
			
			if( exp_mod == null )
			{
				CentralMemory.AppendModule( e.Mobile.Serial, new ClassREMModule( e.Mobile.Serial, ClassREM.Nomad ), true );
				e.Mobile.SendMessage( "Please try again" );
			}
			else
			{
				e.Mobile.SendMessage( "My class is: {0}", exp_mod.m_ClassREM );
			}
		}
		
		public override void Append( Module mod, bool negatively )
		{
			if( !(mod is ClassREMModule) )
				return;
			
			ClassREMModule cm = mod as ClassREMModule;
			ClassREM = cm.ClassREM;
		}
		
		private class InternalTarget : Target
		{
			private ClassREMModule t_mod;
			
			public InternalTarget( ClassREMModule mod ) : base( 1, false, TargetFlags.None )
			{
				t_mod = mod;
			}
			
			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is PlayerMobile )
				{
					PlayerMobile pm = ( PlayerMobile )targeted;
					ClassREMModule RK = ( ClassREMModule )CentralMemory.GetModule( pm.Serial, typeof( ClassREMModule ) );
					
					if ( RK != null )
						from.SendGump( new PropertiesGump( from, RK ) );
					else
						from.SendMessage( "This player does not have a class." );
				}
				else
					from.SendMessage("Can Only Target PLAYERS!");
			}
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
			
			writer.Write( (int)m_ClassREM );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			
			m_ClassREM = (ClassREM)reader.ReadInt();
		}
	}
}
