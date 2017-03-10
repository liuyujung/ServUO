#region AuthorHeader
//
//	Safe Resurrection System version 2.2, by Xanthos
//
//
//
#endregion AuthorHeader
using System;
using System.Threading;
using System.Collections;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Targeting;
using Server.Commands;

namespace Xanthos.SafeResurrection
{
	public class SafeResContext : Item
	{
		//
		// Public interfaces
		//

		// Called from PlayerMobile
		public static void Add( PlayerMobile player )
		{
			Instance.BeginSafety( player );
		}

		// Called from PlayerMobile
		public bool AllowSkillUse( PlayerMobile player, SkillName skillName )
		{
			// This disables only animal taming - other skills can easily be added here.
			if ( skillName == SkillName.AnimalTaming )
			{
				player.SendMessage( "You cannot use that skill while invulnerable." );
				return false;
			}
			return true;
		}

		// In-game commands
		public static void Initialize()
		{
			if ( SafeResConfig.AllowSelfRes && !SafeResConfig.UseSelfResStone )
				CommandHandlers.Register( "ResMe", AccessLevel.Player, new CommandEventHandler( Resme_OnCommand ) );

			if ( SafeResConfig.AllowPetRes && !SafeResConfig.UsePetResStone )
				CommandHandlers.Register( "ResMyPet", AccessLevel.Player, new CommandEventHandler( ResMyPet_OnCommand ) );

			if ( SafeResConfig.RandomDeathCry )
				EventSink.PlayerDeath += new PlayerDeathEventHandler( DeathHandler.EventSink_PlayerDeath );
		}

		//
		// Private interfaces
		//

		private static SafeResTimer s_SafeResTimer;

		// Singleton class - do not call new on it, use Instance to 
		// get access to the one and only instance of it.

		public static SafeResContext Instance { get { return s_SafeResContext == null ? Nested.s_Instance : s_SafeResContext; } }
		private static SafeResContext s_SafeResContext;

		SafeResContext() : base( 4484 )
		{
			s_SafeResContext = this;
			Name = "Safe Resurrection System version 2.2, by Xanthos";
			Internalize();
			Visible = false;
			Movable = false;
			s_SafeResTimer = new SafeResTimer();
			World.Broadcast( SafeResConfig.MessageHue, true, "{0} is now enabled.", Name );
		}

		public override bool Decays { get { return false; } }

		public SafeResContext( Serial serial ) : base( serial )
		{
			s_SafeResContext = this;
			s_SafeResTimer = new SafeResTimer();
		}

		[Usage( "ResMe" )]
		[Description( "Resurrects a player." )]
		private static void Resme_OnCommand( CommandEventArgs e )
		{
			e.Mobile.Resurrect();
		}

		[Usage( "ResMyPet" )]
		[Description( "Resurrects a player's pet." )]
		private static void ResMyPet_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;

			from.Target = new PetResTarget( from );
		}

		internal void BeginSafety( PlayerMobile player )
		{
			s_SafeResTimer.BeginSafety( player );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version

			Array resArray = s_SafeResTimer.ToArray();
			writer.Write( (int)resArray.Length );
			for ( int i = 0; i < resArray.Length; i++ )
			{
				writer.Write( (Mobile)( resArray.GetValue( i ) ) );
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			switch ( reader.ReadInt() )	//version
			{
				case 0:	// Expire everyone on start up
				{
					for ( int count = reader.ReadInt(), i = 0; i < count; i++ )
					{
						Mobile mobile = reader.ReadMobile();
						PlayerMobile player;

						if ( mobile != null && (player = mobile as PlayerMobile) != null )
						{
							SafeResTimer.UnblessPlayer( player );
						}
					}
					break;
				}
			}
		}	

		// Support for the Singleton pattern
		class Nested { static Nested() { } internal static readonly SafeResContext s_Instance = new SafeResContext();}
	}

	internal class SafeResTimer : Server.Timer
	{
		private Queue m_Resurrections;
		private Queue m_DateTimes;

		internal Queue Resurrections
		{
			get { return m_Resurrections; }
		}

		internal Queue DateTimes
		{
			get { return m_DateTimes; }
		}

		public SafeResTimer() : base( TimeSpan.FromMinutes( 5 ), TimeSpan.FromMinutes( 5 ) )
		{
			Priority = TimerPriority.OneSecond;
			m_Resurrections = new Queue();
			m_DateTimes = new Queue();
		}

		protected override void OnTick()
		{
			lock ( Resurrections.SyncRoot )
			{
				Stop();

				DateTime onTickTime = DateTime.Now;

				while ( Resurrections.Count > 0 )
				{
					DateTime nextDateTime = (DateTime)DateTimes.Peek();

					if ( DateTime.Compare( nextDateTime, onTickTime ) <= 0 )
					{
						// Handle any that have expired at this time
						EndSafety( Resurrections.Peek() as PlayerMobile );
					}
					else
					{
						// No more concurrent expiries - schedule the next one
						Delay = nextDateTime.Subtract( DateTime.Now );
						Start();
						break;
					}
				}
			}
		}

		internal void BeginSafety( PlayerMobile player )
		{
			if ( player == null || player.AccessLevel != AccessLevel.Player )
			    return;

			TimeSpan delay = TimeSpan.FromSeconds( SafeResConfig.SafeForSeconds );
			DateTime when = DateTime.Now.Add( delay );

			player.SafeResContext = SafeResContext.Instance;
			BlessPlayer( player );

			lock ( Resurrections.SyncRoot )
			{
				// Push both queues
				Resurrections.Enqueue( player );
				DateTimes.Enqueue( when );

				if ( !Running )
				{
					Delay = delay;
					Start();
				}
			}
		}

		private void EndSafety( PlayerMobile player )
		{
			if ( player == null || player.AccessLevel != AccessLevel.Player )
			    return;

			lock ( Resurrections.SyncRoot )
			{
				// Pop both queues
				Resurrections.Dequeue();
				DateTimes.Dequeue();
			}

			UnblessPlayer( player );
		}

		private static void BlessPlayer( PlayerMobile player )
		{
			if ( player == null || player.Blessed == true )
				return;

			player.Blessed = true;
			player.SendMessage( SafeResConfig.MessageHue, "You have been made invulnerable for {0} seconds.", SafeResConfig.SafeForSeconds );
			player.FixedParticles( 0x373A, 10, 15, 5018, EffectLayer.Head );
			player.PlaySound( 0x1F5 );

			if ( SafeResConfig.FullStatsOnRes )
			{
				player.Hits = player.HitsMax;
				player.Mana = player.ManaMax;
				player.Stam = player.StamMax;
			}
		}

		internal static void UnblessPlayer( PlayerMobile player )
		{
			if ( player == null || player.Blessed == false )
				return;

			player.SendMessage( SafeResConfig.MessageHue, "Your invulnerability has EXPIRED!!" );
			player.FixedParticles( 0x373A, 10, 15, 5018, EffectLayer.Head );
			player.PlaySound( 0x1F5 );
			player.SafeResContext = null;
			player.Blessed = false;
		}

		internal Array ToArray()
		{
			Array array;

			lock ( Resurrections.SyncRoot )
				array = Resurrections.ToArray();

			return array;
		}
	}

	public class PetResTarget : Target
	{
		public PetResTarget( Mobile from ) : base( 15, false, TargetFlags.None )
		{
			from.SendMessage( "Which pet will you resurrect?" );
		}

		protected override void OnTarget( Mobile from, object obj )
		{
			BaseCreature creature = obj as BaseCreature;

			if ( !from.Alive )
				from.SendMessage( "You may not do that while dead." );

			else if ( null == creature )
				from.SendMessage( "That is not a pet!" );

			else if ( from.AccessLevel == AccessLevel.Player && ( creature.Controlled == false || creature.ControlMaster != from ) )
				from.SendMessage( "You do not control that creature!" );

			else if ( !creature.IsDeadPet )
				from.SendMessage( "That pet is not a dead!" );

			else
				creature.ResurrectPet();
		}
	}

	internal class DeathHandler
	{
		internal static Hashtable m_Deaths = new Hashtable();
	
		internal static void EventSink_PlayerDeath( PlayerDeathEventArgs e )
		{
			Mobile mobile = e.Mobile;
			string victim = mobile.Name;
			string proNoun = ( mobile.Female ? "she" : "he" );
			string possesiveProNoun = ( mobile.Female ? "her" : "his" );
			string killer = ( null == mobile.LastKiller ? "no one in particular" : mobile.LastKiller.Name );
			int which = Utility.RandomMinMax( 0, SafeResConfig.DeathCrys.Length - 1 );
			object obj = m_Deaths[ mobile ];
			int deaths = obj == null ? 0 : (int)obj;

			m_Deaths[ mobile ] = ++deaths;

			World.Broadcast( SafeResConfig.MessageHue, true, SafeResConfig.DeathCrys[ which ], victim, proNoun, possesiveProNoun, killer, deaths.ToString() );
		}
		
		internal ArrayList GetKillers( Mobile mobile )
		{
			ArrayList killers = new ArrayList();

			foreach ( AggressorInfo ai in mobile.Aggressors )
			{
				if ( ai.Attacker.Player && ai.CanReportMurder && !ai.Reported )
				{
					killers.Add( ai.Attacker );
				}
			}
			return killers;
		}
	}
}