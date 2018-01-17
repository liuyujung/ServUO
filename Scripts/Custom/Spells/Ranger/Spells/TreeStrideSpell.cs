using System;
using Server.Network;
using Server.Multis;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using Server.Regions;

namespace Server.Spells.Ranger
{
	public class RangerTreeStrideSpell : RangerSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
		"Tree Stride", "Lema ed' Taur",
		263,
		9032,
		Reagent.DestroyingAngel,
		Reagent.Ginseng,
		Reagent.Kindling
	);
		
		public override double CastDelay{ get{ return 4.0; } }
		public override int RequiredMana{ get{ return 40; } }
		public override double RequiredSkill{ get{ return 80; } }
		
		private RunebookEntry m_Entry;
		
		public RangerTreeStrideSpell( Mobile caster, Item scroll ) : this( caster, scroll, null )
		{
		}

		public override SpellCircle Circle
		{
			get
			{
				return SpellCircle.Seventh;
			}
		}
		
		public RangerTreeStrideSpell( Mobile caster, Item scroll, RunebookEntry entry ) : base( caster, scroll, m_Info )
		{
			m_Entry = entry;
		}
		
		public override void OnCast()
		{
            if ( m_Entry == null )
				Caster.Target = new InternalTarget( this );

			else
				Effect( m_Entry.Location, m_Entry.Map, true );
		}
		
		public override bool CheckCast()
		{
			if ( Caster.Criminal )
			{
				Caster.SendLocalizedMessage( 1005561, "", 0x22 ); // Thou'rt a criminal and cannot escape so easily.
				return false;
			}
			else if ( SpellHelper.CheckCombat( Caster ) )
			{
				Caster.SendLocalizedMessage( 1005564, "", 0x22 ); // Wouldst thou flee during the heat of battle??
				return false;
			}
			
			return SpellHelper.CheckTravel( Caster, TravelCheckType.GateFrom );
		}
		
		public void Effect( Point3D loc, Map map, bool checkMulti )
		{
			if ( map == null || (!Core.AOS && Caster.Map != map) )
			{
				Caster.SendLocalizedMessage( 1005570 ); // You can not gate to another facet.
			}
			else if ( !SpellHelper.CheckTravel( Caster, TravelCheckType.GateFrom ) )
			{
			}
			else if ( !SpellHelper.CheckTravel( Caster,  map, loc, TravelCheckType.GateTo ) )
			{
			}
			else if ( Caster.Kills >= 5 && map != Map.Felucca )
			{
				Caster.SendLocalizedMessage( 1019004 ); // You are not allowed to travel there.
			}
			else if ( Caster.Criminal )
			{
				Caster.SendLocalizedMessage( 1005561, "", 0x22 ); // Thou'rt a criminal and cannot escape so easily.
			}
			else if ( SpellHelper.CheckCombat( Caster ) )
			{
				Caster.SendLocalizedMessage( 1005564, "", 0x22 ); // Wouldst thou flee during the heat of battle??
			}
			else if ( !map.CanSpawnMobile( loc.X, loc.Y, loc.Z ) )
			{
				Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
			}
			else if ( (checkMulti && SpellHelper.CheckMulti( loc, map )) )
			{
				Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
			}
			else if ( CheckSequence() )
			{
				Caster.SendMessage( "You open a woodland gate to another location." ); // You open a magical gate to another location
				
				Effects.PlaySound( Caster.Location, Caster.Map, 1 );
				int treex;
				int treey;
				int treez;

//GATE 1				
				InternalItem firstGatea = new InternalItem( loc, map );
				treex=Caster.X;
				treey=Caster.Y;
				firstGatea.ItemID=0xD40; //Moongate
				treez=Caster.Z+1;
				Point3D treexyza = new Point3D(treex,treey,treez);
				firstGatea.MoveToWorld( treexyza, Caster.Map );
				
				InternalItem firstGateb = new InternalItem( loc, map );
				treex=Caster.X-1;
				firstGateb.ItemID=0xC9E;//Left Tree
				treey=Caster.Y+1;
				treez=Caster.Z;
				Point3D treexyzb = new Point3D(treex,treey,treez);
				firstGateb.MoveToWorld( treexyzb, Caster.Map );
				
				InternalItem firstGatec = new InternalItem( loc, map );
				treex=Caster.X+1;
				firstGatec.ItemID=0xCF2;//Left Vine
				treey=Caster.Y+1;
				treez=Caster.Z;
				Point3D treexyzc = new Point3D(treex,treey,treez);
				firstGatec.MoveToWorld( treexyzc, Caster.Map );
				
				InternalItem firstGated = new InternalItem( loc, map );
				firstGated.ItemID=0xCF2;//Right Vine
				treex=Caster.X+2;
				treey=Caster.Y;
				treez=Caster.Z;
				Point3D treexyzd = new Point3D(treex,treey,treez);
				firstGated.MoveToWorld( treexyzd, Caster.Map );
				
				InternalItem firstGatee = new InternalItem( loc, map );
				treex=Caster.X+1;
				firstGatee.ItemID=0xC9E;//Right Tree
				treey=Caster.Y-1;
				treez=Caster.Z;
				Point3D treexyze = new Point3D(treex,treey,treez);
				firstGatee.MoveToWorld( treexyze, Caster.Map );
				
				Effects.PlaySound( loc, map, 0x482 );
				
//GATE 2			
				InternalItem secondGatea = new InternalItem( Caster.Location, Caster.Map );
				treex=loc.X;
				treey=loc.Y;
				secondGatea.ItemID=0xD40; //Moongate
				treez=loc.Z+1;
				Point3D treeaxyza = new Point3D(treex,treey,treez);
				secondGatea.MoveToWorld( treeaxyza, map);
				
				InternalItem secondGateb = new InternalItem( Caster.Location, Caster.Map );
				treex=loc.X-1;
				secondGateb.ItemID=0xC9E;//Left Tree
				treey=loc.Y+1;
				treez=loc.Z-1;
				Point3D treeaxyzb = new Point3D(treex,treey,treez);
				secondGateb.MoveToWorld( treeaxyzb, map);
				
				InternalItem secondGatec = new InternalItem( Caster.Location, Caster.Map );
				treex=loc.X+1;
				treey=loc.Y+1;
				treez=loc.Z;
				secondGatec.ItemID=0xCF2;//Left Vine
				Point3D treeaxyzc = new Point3D(treex,treey,treez);
				secondGatec.MoveToWorld( treeaxyzc, map);
				
				InternalItem secondGated = new InternalItem( Caster.Location, Caster.Map );
				treex=loc.X+2;
				treey=loc.Y;
				treez=loc.Z;
				secondGated.ItemID=0xCF2;//Right Vine
				Point3D treeaxyzd = new Point3D(treex,treey,treez);
				secondGated.MoveToWorld( treeaxyzd, map);
				
				InternalItem secondGatee = new InternalItem( Caster.Location, Caster.Map );
				treex=loc.X+1;
				secondGatee.ItemID=0xC9E;//Right Tree
				treey=loc.Y-1;
				treez=loc.Z;
				Point3D treeaxyze = new Point3D(treex,treey,treez);
				secondGatee.MoveToWorld( treeaxyze, map);
			}
			
			FinishSequence();
		}
		
		[DispellableField]
		private class InternalItem : Moongate
		{
			public InternalItem( Point3D target, Map map ) : base( target, map )
			{
				Map = map;
	
				Dispellable = true;
				
				InternalTimer t = new InternalTimer( this );
				t.Start();
			}
			
			public InternalItem( Serial serial ) : base( serial )
			{
			}
			
			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
			}
			
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				
				Delete();
			}
			
			private class InternalTimer : Timer
			{
				private Item m_Item;
				
				public InternalTimer( Item item ) : base( TimeSpan.FromSeconds( 30.0 ) )
				{
					m_Item = item;
				}
				
				protected override void OnTick()
				{
					m_Item.Delete();
				}
			}
		}
		
		
		private class InternalTarget : Target
		{
			private RangerTreeStrideSpell m_Owner;
			
			public InternalTarget( RangerTreeStrideSpell owner ) : base( 12, false, TargetFlags.None )
			{
				m_Owner = owner;
				
				owner.Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, 501029 ); // Select Marked item.
			}
			
			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is RecallRune )
				{
					RecallRune rune = (RecallRune)o;
					
					if ( rune.Marked )
						m_Owner.Effect( rune.Target, rune.TargetMap, true );
					else
						from.SendLocalizedMessage( 501803 ); // That rune is not yet marked.
				}
				else if ( o is Runebook )
				{
					RunebookEntry e = ((Runebook)o).Default;
					
					if ( e != null )
						m_Owner.Effect( e.Location, e.Map, true );
					else
						from.SendLocalizedMessage( 502354 ); // Target is not marked.
				}
				else
				{
					from.Send( new MessageLocalized( from.Serial, from.Body, MessageType.Regular, 0x3B2, 3, 501030, from.Name, "" ) ); // I can not gate travel from that object.
				}
			}
			
			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}

