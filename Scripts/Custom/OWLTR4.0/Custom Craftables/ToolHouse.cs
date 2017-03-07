/*
 created by:
     /\            888                   888     .d8888b.   .d8888b.  
____/_ \____       888                   888    d88P  Y88b d88P  Y88b 
\  ___\ \  /       888                   888    888    888 888    888 
 \/ /  \/ /    .d88888  8888b.   8888b.  888888 Y88b. d888 Y88b. d888 
 / /\__/_/\   d88" 888     "88b     "88b 888     "Y888P888  "Y888P888 
/__\ \_____\  888  888 .d888888 .d888888 888           888        888 
    \  /      Y88b 888 888  888 888  888 Y88b.  Y88b  d88P Y88b  d88P 
     \/        "Y88888 "Y888888 "Y888888  "Y888  "Y8888P"   "Y8888P"  

Last edited on 25/12/2004
Description: a small mini house blessed item that can hold varius tools up to limit that can be set by gm
*/
using System;
using System.Collections;
using Server;
using Server.Prompts;
using Server.Mobiles;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Multis;
using Server.Regions;

namespace Server.Items
{
	[FlipableAttribute( 0x22c4, 0x22c4 )]
	public class ToolHouse : Item
	{
		private int m_LumberjackingProspectorsTool;
		private int m_SewingKit;
		private int m_MortarPestle;
		private int m_ScribesPen;
		private int m_MalletAndChisel;
		private int m_Carpentry;
		private int m_FletcherTools;
		private int m_TinkerTools;
		private int m_BlackSmith;
		private int m_Shovel;
		private int m_SturdyShovel; //not used since v3
		private int m_GargoylesPickaxe;
		private int m_Lumberjacking;
		private int m_ProspectorsTool;
		private int m_Cooking;
		private int m_Cartography;
		private int m_Glassblowing;
		private int m_TaxidermyKit;
		private int m_GargoylesAxe;
		private int m_GargoylesKnife;
		private int m_Brush;
		private int m_Limit;

		[CommandProperty( AccessLevel.GameMaster )]
		public int LumberjackingProspectorsTool{ get{ return m_LumberjackingProspectorsTool; } set{ m_LumberjackingProspectorsTool = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int SewingKit{ get{ return m_SewingKit; } set{ m_SewingKit = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int MortarPestle{ get{ return m_MortarPestle; } set{ m_MortarPestle = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int ScribesPen{ get{ return m_ScribesPen; } set{ m_ScribesPen = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int MalletAndChisel{ get{ return m_MalletAndChisel; } set{ m_MalletAndChisel = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Carpentry{ get{ return m_Carpentry; } set{ m_Carpentry = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int FletcherTools{ get{ return m_FletcherTools; } set{ m_FletcherTools = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int TinkerTools{ get{ return m_TinkerTools; } set{ m_TinkerTools = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int BlackSmith{ get{ return m_BlackSmith; } set{ m_BlackSmith = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Shovel{ get{ return m_Shovel; } set{ m_Shovel = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int GargoylesPickaxe{ get{ return m_GargoylesPickaxe; } set{ m_GargoylesPickaxe = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Lumberjacking{ get{ return m_Lumberjacking; } set{ m_Lumberjacking = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int ProspectorsTool{ get{ return m_ProspectorsTool; } set{ m_ProspectorsTool = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Cooking{ get{ return m_Cooking; } set{ m_Cooking = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Cartography{ get{ return m_Cartography; } set{ m_Cartography = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Glassblowing{ get{ return m_Glassblowing; } set{ m_Glassblowing = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int TaxidermyKit{ get{ return m_TaxidermyKit; } set{ m_TaxidermyKit = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int GargoylesAxe{ get{ return m_GargoylesAxe; } set{ m_GargoylesAxe = value; InvalidateProperties(); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int GargoylesKnife{ get{ return m_GargoylesKnife; } set{ m_GargoylesKnife = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Brush{ get{ return m_Brush; } set{ m_Brush = value; InvalidateProperties(); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int Limit{ get{ return m_Limit; } set{ m_Limit = value; InvalidateProperties(); } }

		[Constructable]
		public ToolHouse() : base( 0x22c4 )
		{
			Movable = true;
			Weight = 1.0;
			Name = "Tool House";
			//LootType = LootType.Blessed;
			Limit = 60000;
			Hue = 69;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if (!(from is PlayerMobile))
				return;
			if (IsChildOf(from.Backpack))
				from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
			else
				from.SendMessage("This must be in your backpack.");
		}
				
		public override bool OnDragDrop ( Mobile from, Item dropped)
		{
			return false;
		}

		public void BeginCombine( Mobile from )
		{
			from.Target = new ToolHouseTarget( this );
		}

		public void EndCombine( Mobile from, object o )
		{
			if ( o is Item )
			{                    
				if (o is Item && (((Item)o).IsChildOf(from.Backpack) || ((Item)o).IsChildOf(from.BankBox)))
				{
					if ( o is ProspectorsTool )
					{
						if ( ProspectorsTool > (m_Limit - ((ProspectorsTool)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
							ProspectorsTool = ( ProspectorsTool + ((ProspectorsTool)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					else if ( o is LumberjackingProspectorsTool )
					{
						if ( LumberjackingProspectorsTool > (m_Limit - ((LumberjackingProspectorsTool)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
							LumberjackingProspectorsTool = ( LumberjackingProspectorsTool + ((LumberjackingProspectorsTool)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					else if ( o is TaxidermyKit )
					{
						if ( TaxidermyKit > (m_Limit - 1) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
							TaxidermyKit = ( TaxidermyKit + 1 );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					else if ( o is SewingKit || o is SturdySewingKit)
					{
						if ( SewingKit >= (m_Limit - ((BaseTool)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
							SewingKit = ( SewingKit + ((BaseTool)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					else if ( o is MortarPestle )
					{
						if ( MortarPestle > (m_Limit - ((BaseTool)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
							MortarPestle = ( MortarPestle + ((BaseTool)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					else if ( o is ScribesPen )
					{
						if ( ScribesPen > (m_Limit - ((BaseTool)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
							ScribesPen = ( ScribesPen + ((BaseTool)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					else if ( o is MalletAndChisel )
					{
						if ( MalletAndChisel > (m_Limit - ((BaseTool)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
							MalletAndChisel = ( MalletAndChisel + ((BaseTool)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					else if ( o is Hammer || o is Nails || o is DrawKnife || o is DovetailSaw || o is Froe || 
						o is Inshave || o is MouldingPlane || o is JointingPlane || o is SmoothingPlane || 
						o is Scorp || o is Saw )
					{
						if ( Carpentry > (m_Limit - ((BaseTool)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
							Carpentry = ( Carpentry + ((BaseTool)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					else if ( o is FletcherTools )
					{
						if ( FletcherTools > (m_Limit - ((BaseTool)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
							FletcherTools = ( FletcherTools + ((BaseTool)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					else if ( o is TinkerTools )
					{
						if ( TinkerTools > (m_Limit - ((BaseTool)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
							TinkerTools = ( TinkerTools + ((BaseTool)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					else if ( o is Tongs || o is SturdySmithHammer || o is SmithHammer )
					{
						if ( BlackSmith > (m_Limit - ((BaseTool)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
							BlackSmith = ( BlackSmith + ((BaseTool)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					else if ( o is Shovel || o is SturdyShovel )
					{
						if ( Shovel > (m_Limit - ((BaseHarvestTool)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
					
							Shovel = ( Shovel + ((BaseHarvestTool)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					else if ( o is Pickaxe || o is SturdyPickaxe )
					{
						if ( Shovel > (m_Limit - ((BaseAxe)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
					
							Shovel = ( Shovel + ((BaseAxe)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					else if ( o is GargoylesPickaxe )
					{
						if ( GargoylesPickaxe > (m_Limit - ((BaseAxe)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
					
							GargoylesPickaxe = ( GargoylesPickaxe + ((BaseAxe)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					else if ( o is Hatchet || o is Axe || o is BattleAxe || o is DoubleAxe || o is ExecutionersAxe ||
						o is LargeBattleAxe || o is TwoHandedAxe )
					{
						if ( Lumberjacking > (m_Limit - ((BaseAxe)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
							Lumberjacking = ( Lumberjacking + ((BaseAxe)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}					
					else if ( o is Halberd || o is Bardiche )
					{
						if ( Lumberjacking > (m_Limit - ((BasePoleArm)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
							Lumberjacking = ( Lumberjacking + ((BasePoleArm)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					else if ( o is RollingPin || o is Skillet || o is FlourSifter )
					{
						if ( Cooking > (m_Limit - ((BaseTool)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
							Cooking = ( Cooking + ((BaseTool)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					else if ( o is MapmakersPen )
					{
						if ( Cartography > (m_Limit - ((BaseTool)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
							Cartography = ( Cartography + ((BaseTool)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					else if ( o is Blowpipe )
					{
						if ( Glassblowing > (m_Limit - ((BaseTool)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
							Glassblowing = ( Glassblowing + ((BaseTool)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					else if ( o is GargoylesAxe )
					{
						if ( GargoylesAxe > (m_Limit - ((BaseAxe)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
							GargoylesAxe = ( GargoylesAxe + ((BaseAxe)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					else if ( o is GargoylesKnife )
					{
						if ( GargoylesKnife > (m_Limit - ((GargoylesKnife)o).UsesRemaining) )
							from.SendMessage( "That tool type is too full to add more." );
						else
						{
							GargoylesKnife = ( GargoylesKnife + ((GargoylesKnife)o).UsesRemaining );
							((Item)o).Delete();
							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
							BeginCombine( from );
						}
					}
					///					else if ( o is Brush )
					///					{
					///						if ( Brush > (m_Limit - ((BaseTool)o).UsesRemaining) )
					///							from.SendMessage( "That tool type is too full to add more." );
					///						else
					///						{
					///							Brush = ( Brush + ((BaseTool)o).UsesRemaining );
					///							((Item)o).Delete();
					///							from.SendGump( new ToolHouseGump( (PlayerMobile)from, this ) );
					///							BeginCombine( from );
					///						}
					///					}
				}
			}
			else
			{
				from.SendLocalizedMessage( 1045158 ); // You must have the item in your backpack to target it.
			}
		}

		public ToolHouse( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 3 ); // version
			//version 3 removing study adding brush
			writer.Write( (int) m_Brush); 
			//version 2
			writer.Write( (int) m_LumberjackingProspectorsTool); 
			//version 1			
			writer.Write( (int) m_GargoylesAxe); 
			writer.Write( (int) m_GargoylesKnife);
			//version 0
			writer.Write( (int) m_SewingKit); 
			writer.Write( (int) m_MortarPestle);
			writer.Write( (int) m_ScribesPen); 
			writer.Write( (int) m_MalletAndChisel);
			writer.Write( (int) m_Carpentry); 
			writer.Write( (int) m_FletcherTools); 
			writer.Write( (int) m_TinkerTools); 
			writer.Write( (int) m_BlackSmith); 
			writer.Write( (int) m_Shovel); 
			writer.Write( (int) m_GargoylesPickaxe); 
			writer.Write( (int) m_ProspectorsTool);
			writer.Write( (int) m_Lumberjacking); 
			writer.Write( (int) m_Cooking); 
			writer.Write( (int) m_Cartography); 
			writer.Write( (int) m_Glassblowing);
			writer.Write( (int) m_TaxidermyKit); 
			writer.Write( (int) m_Limit);
		}
		
		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt();

			switch ( version )
			{
				case 3:
				{
					m_Brush = reader.ReadInt();
					goto case 2;
				}
				case 2:
				{
					m_LumberjackingProspectorsTool = reader.ReadInt();
					goto case 1;
				}
				case 1:
				{
					m_GargoylesAxe = reader.ReadInt(); 
					m_GargoylesKnife = reader.ReadInt();
					goto case 0;
				}
				case 0:
				{		
					m_SewingKit = reader.ReadInt(); 
					m_MortarPestle = reader.ReadInt();
					m_ScribesPen = reader.ReadInt(); 
					m_MalletAndChisel = reader.ReadInt();
					m_Carpentry = reader.ReadInt(); 
					m_FletcherTools = reader.ReadInt(); 
					m_TinkerTools = reader.ReadInt(); 
					m_BlackSmith = reader.ReadInt(); 
					m_Shovel = reader.ReadInt(); 
					if (version < 3)
						m_Shovel = m_Shovel + reader.ReadInt(); 
					m_GargoylesPickaxe = reader.ReadInt(); 
					m_ProspectorsTool = reader.ReadInt();
					m_Lumberjacking = reader.ReadInt(); 
					m_Cooking = reader.ReadInt();
					m_Cartography = reader.ReadInt(); 
					m_Glassblowing = reader.ReadInt();
					m_TaxidermyKit = reader.ReadInt(); 
					m_Limit = reader.ReadInt(); 
					break;
				} 
			} 
		}
	}

	public class ToolHouseGump : Gump
	{
		private PlayerMobile m_From;
		private ToolHouse m_House;

		public ToolHouseGump( PlayerMobile from, ToolHouse house ) : base( 25, 25 )
		{
			m_From = from;
			m_House = house;

			m_From.CloseGump( typeof( ToolHouseGump ) );

			AddPage( 0 );

			AddBackground( 50, 10, 455, 335, 5054 );
			AddImageTiled( 58, 20, 438, 306, 2624 );
			AddAlphaRegion( 58, 20, 438, 306 );

			AddLabel( 250, 25, 0x480, "Tool House" );

			AddLabel( 120, 50, 0x486, "Sewing Kit" );
			AddLabel( 230, 50, 0x480, house.SewingKit.ToString() );
			AddButton( 70, 50, 4005, 4007, 1, GumpButtonType.Reply, 0 );

			AddLabel( 120, 75, 0x486, "Mortar and Pestle" );
			AddLabel( 230, 75, 0x480, house.MortarPestle.ToString() );
			AddButton( 70, 75, 4005, 4007, 2, GumpButtonType.Reply, 0 );

			AddLabel( 120, 100, 0x486, "Scribes Pen" );
			AddLabel( 230, 100, 0x480, house.ScribesPen.ToString() );
			AddButton( 70, 100, 4005, 4007, 3, GumpButtonType.Reply, 0 );
			
			AddLabel( 120, 125, 0x486, "Mallet and Chisel" );
			AddLabel( 230, 125, 0x480, house.MalletAndChisel.ToString() );
			AddButton( 70, 125, 4005, 4007, 4, GumpButtonType.Reply, 0 );

			AddLabel( 120, 150, 0x486, "Carpentry" );
			AddLabel( 230, 150, 0x480, house.Carpentry.ToString() );
			AddButton( 70, 150, 4005, 4007, 5, GumpButtonType.Reply, 0 );

			AddLabel( 120, 175, 0x486, "Fletcher Tools" );
			AddLabel( 230, 175, 0x480, house.FletcherTools.ToString() );
			AddButton( 70, 175, 4005, 4007, 6, GumpButtonType.Reply, 0 );

			AddLabel( 120, 200, 0x486, "Tinker Tools" );
			AddLabel( 230, 200, 0x480, house.TinkerTools.ToString() );
			AddButton( 70, 200, 4005, 4007, 7, GumpButtonType.Reply, 0 );

			AddLabel( 120, 225, 0x486, "BlackSmith" );
			AddLabel( 230, 225, 0x480, house.BlackSmith.ToString() );
			AddButton( 70, 225, 4005, 4007, 8, GumpButtonType.Reply, 0 );

			AddLabel( 120, 250, 0x486, "Shovel & Pickaxe" );
			AddLabel( 230, 250, 0x480, house.Shovel.ToString() );
			AddButton( 70, 250, 4005, 4007, 9, GumpButtonType.Reply, 0 );

			AddLabel( 120, 275, 0x486, "Gargoyles Pickaxe" );
			AddLabel( 230, 275, 0x480, house.GargoylesPickaxe.ToString() );
			AddButton( 70, 275, 4005, 4007, 10, GumpButtonType.Reply, 0 );

			AddLabel( 120, 300, 0x486, "Prospectors Tool" );
			AddLabel( 230, 300, 0x480, house.ProspectorsTool.ToString() );
			AddButton( 70, 300, 4005, 4007, 11, GumpButtonType.Reply, 0 );
			
			AddLabel( 325, 50, 0x486, "Prospectors Axe" );
			AddLabel( 440, 50, 0x480, house.LumberjackingProspectorsTool.ToString() );
			AddButton( 275, 50, 4005, 4007, 12, GumpButtonType.Reply, 0 );

			AddLabel( 325, 75, 0x486, "Lumberjacking" );
			AddLabel( 440, 75, 0x480, house.Lumberjacking.ToString() );
			AddButton( 275, 75, 4005, 4007, 13, GumpButtonType.Reply, 0 );

			AddLabel( 325, 100, 0x486, "Cooking" );
			AddLabel( 440, 100, 0x480, house.Cooking.ToString() );
			AddButton( 275, 100, 4005, 4007, 14, GumpButtonType.Reply, 0 );
			
			AddLabel( 325, 125, 0x486, "Cartography" );
			AddLabel( 440, 125, 0x480, house.Cartography.ToString() );
			AddButton( 275, 125, 4005, 4007, 15, GumpButtonType.Reply, 0 );

			AddLabel( 325, 150, 0x486, "Glassblowing" );
			AddLabel( 440, 150, 0x480, house.Glassblowing.ToString() );
			AddButton( 275, 150, 4005, 4007, 16, GumpButtonType.Reply, 0 );
			
			AddLabel( 325, 175, 0x486, "Taxidermy Kit" );
			AddLabel( 440, 175, 0x480, house.TaxidermyKit.ToString() );
			AddButton( 275, 175, 4005, 4007, 17, GumpButtonType.Reply, 0 );

			AddLabel( 325, 200, 0x486, "Gargoyles Axe" );
			AddLabel( 440, 200, 0x480, house.GargoylesAxe.ToString() );
			AddButton( 275, 200, 4005, 4007, 18, GumpButtonType.Reply, 0 );
			
			AddLabel( 325, 225, 0x486, "Gargoyles Knife" );
			AddLabel( 440, 225, 0x480, house.GargoylesKnife.ToString() );
			AddButton( 275, 225, 4005, 4007, 19, GumpButtonType.Reply, 0 );

///			AddLabel( 325, 250, 0x486, "Brush" );
///			AddLabel( 440, 250, 0x480, house.Brush.ToString() );
///			AddButton( 275, 250, 4005, 4007, 20, GumpButtonType.Reply, 0 );

			AddLabel(325, 275, 88, "Max Uses:" );
			AddLabel(425, 275, 0x480, house.Limit.ToString() );	
			
			AddButton( 275, 300, 4005, 4007, 21, GumpButtonType.Reply, 0 );
			AddLabel( 325, 300, 0x486, "Add Tool" );
                        
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( m_House.Deleted )
				return;

			else if ( info.ButtonID == 1 )
			{
				if ( m_House.SewingKit > 0 )
				{
					m_From.AddToBackpack( new SewingKit(m_House.SewingKit) );
					m_House.SewingKit = ( 0 );
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				}
				else
				{
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
					m_House.BeginCombine( m_From );
				}
			}
			else if ( info.ButtonID == 2 )
			{
				if ( m_House.MortarPestle > 0 )
				{
					m_From.AddToBackpack( new MortarPestle(m_House.MortarPestle) );
					m_House.MortarPestle = ( 0 );
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				}
				else
				{
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
					m_House.BeginCombine( m_From );
				}
			}
			else if ( info.ButtonID == 3 )
			{
				if ( m_House.ScribesPen > 0 )
				{
					m_From.AddToBackpack( new ScribesPen(m_House.ScribesPen) );
					m_House.ScribesPen = ( 0 );
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				}
				else
				{
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
					m_House.BeginCombine( m_From );
				}
			}
			else if ( info.ButtonID == 4 )
			{
				if ( m_House.MalletAndChisel > 0 )
				{
					m_From.AddToBackpack( new MalletAndChisel(m_House.MalletAndChisel) );
					m_House.MalletAndChisel = ( 0 );
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				}
				else
				{
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
					m_House.BeginCombine( m_From );
				}
			}
			else if ( info.ButtonID == 5 )
			{
				if ( m_House.Carpentry > 0 )
				{
					m_From.AddToBackpack( new Hammer(m_House.Carpentry) );
					m_House.Carpentry = ( 0 );
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				}
				else
				{
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
					m_House.BeginCombine( m_From );
				}
			}
			else if ( info.ButtonID == 6 )
			{
				if ( m_House.FletcherTools > 0 )
				{
					m_From.AddToBackpack( new FletcherTools(m_House.FletcherTools) );
					m_House.FletcherTools = ( 0 );
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				}
				else
				{
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
					m_House.BeginCombine( m_From );
				}
			}
			else if ( info.ButtonID == 7 )
			{
				if ( m_House.TinkerTools > 0 )
				{
					m_From.AddToBackpack( new TinkerTools(m_House.TinkerTools) );
					m_House.TinkerTools = ( 0 );
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				}
				else
				{
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
					m_House.BeginCombine( m_From );
				}
			}
			else if ( info.ButtonID == 8 )
			{
				if ( m_House.BlackSmith > 0 )
				{
					m_From.AddToBackpack( new SmithHammer(m_House.BlackSmith) );
					m_House.BlackSmith = ( 0 );
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				}
				else
				{
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
					m_House.BeginCombine( m_From );
				}
			}
			else if ( info.ButtonID == 9 )
			{
				if ( m_House.Shovel > 0 )
				{
                	m_From.AddToBackpack( new Shovel(m_House.Shovel) );
					m_House.Shovel = ( 0 );
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				}
				else
				{
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
					m_House.BeginCombine( m_From );
				}
			}
			else if ( info.ButtonID == 10 )
			{
				if ( m_House.GargoylesPickaxe > 0 )
				{
					GargoylesPickaxe gargoylesPickaxe = new GargoylesPickaxe();	// items that don't accept uses need to be handled like this
					gargoylesPickaxe.UsesRemaining = m_House.GargoylesPickaxe;	// items that don't accept uses need to be handled like this
					m_From.AddToBackpack(gargoylesPickaxe);	// items that don't accept uses need to be handled like this
					m_House.GargoylesPickaxe = ( 0 );
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				}
				else
				{
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
					m_House.BeginCombine( m_From );
				}
			}
			else if ( info.ButtonID == 11 )
			{
				if ( m_House.ProspectorsTool > 0 )
				{
					ProspectorsTool prospectorstool = new ProspectorsTool();	// items that don't accept uses need to be handled like this
					prospectorstool.UsesRemaining = m_House.ProspectorsTool;	// items that don't accept uses need to be handled like this
					m_From.AddToBackpack(prospectorstool);	// items that don't accept uses need to be handled like this
					m_House.ProspectorsTool = ( 0 );
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				}
				else
				{
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
					m_House.BeginCombine( m_From );
				}
			}
			else if ( info.ButtonID == 12 )
			{
				if ( m_House.LumberjackingProspectorsTool > 0 )
				{
					LumberjackingProspectorsTool lumberjackingprospectorstool = new LumberjackingProspectorsTool();	// items that don't accept uses need to be handled like this
					lumberjackingprospectorstool.UsesRemaining = m_House.LumberjackingProspectorsTool;	// items that don't accept uses need to be handled like this
					m_From.AddToBackpack(lumberjackingprospectorstool);	// items that don't accept uses need to be handled like this
					m_House.LumberjackingProspectorsTool = ( 0 );
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				}
				else
				{
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
					m_House.BeginCombine( m_From );
				}
			}
			else if ( info.ButtonID == 13 )
			{
				if ( m_House.Lumberjacking > 0 )
				{
					Hatchet lumberjacking = new Hatchet();	// items that don't accept uses need to be handled like this
					lumberjacking.UsesRemaining = m_House.Lumberjacking;	// items that don't accept uses need to be handled like this
					m_From.AddToBackpack(lumberjacking);	// items that don't accept uses need to be handled like this
					lumberjacking.ShowUsesRemaining = true;
					m_House.Lumberjacking = ( 0 );
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				}
				else
				{
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
					m_House.BeginCombine( m_From );
				}
			}
			else if ( info.ButtonID == 14 )
			{
				if ( m_House.Cooking > 0 )
				{                       
					m_From.AddToBackpack( new Skillet(m_House.Cooking) );
					m_House.Cooking = ( 0 );
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				}
				else
				{
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
					m_House.BeginCombine( m_From );
				}
			}
			else if ( info.ButtonID == 15 )
			{
				if ( m_House.Cartography > 0 )
				{                       
					m_From.AddToBackpack( new MapmakersPen(m_House.Cartography) );
					m_House.Cartography = ( 0 );
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				}
				else
				{
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
					m_House.BeginCombine( m_From );
				}
			}
			else if ( info.ButtonID == 16 )
			{
				if ( m_House.Glassblowing > 0 )
				{                       
					m_From.AddToBackpack( new Blowpipe(m_House.Glassblowing) );
					m_House.Glassblowing = ( 0 );
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				}
				else
				{
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
					m_House.BeginCombine( m_From );
				}
			}
			else if ( info.ButtonID == 17 )
			{
				if ( m_House.TaxidermyKit > 0 )
				{                       
					m_From.AddToBackpack( new TaxidermyKit() ); // have 1 time use
					m_House.TaxidermyKit = ( m_House.TaxidermyKit - 1 ); // have 1 time use
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				}
				else
				{
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
					m_House.BeginCombine( m_From );
				}
			}
			else if ( info.ButtonID == 18 )
			{
				if ( m_House.GargoylesAxe > 0 )
				{                       
					m_From.AddToBackpack( new GargoylesAxe(m_House.GargoylesAxe) );
					m_House.GargoylesAxe = ( 0 );
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				}
				else
				{
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
					m_House.BeginCombine( m_From );
				}
			}
			else if ( info.ButtonID == 19 )
			{
				if ( m_House.GargoylesKnife > 0 )
				{                       
					m_From.AddToBackpack( new GargoylesKnife(m_House.GargoylesKnife) );
					m_House.GargoylesKnife = ( 0 );
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				}
				else
				{
					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
					m_House.BeginCombine( m_From );
				}
			}
///			else if ( info.ButtonID == 20 )
///			{
///				if ( m_House.Brush > 0 )
///				{
///					m_From.AddToBackpack( new Brush(m_House.Brush) );
///					m_House.Brush = ( 0 );
///					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
///				}
///				else
///				{
///					m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
///					m_House.BeginCombine( m_From );
///				}
///			}
			else if ( info.ButtonID == 21 )
			{
				m_From.SendGump( new ToolHouseGump( m_From, m_House ) );
				m_House.BeginCombine( m_From );
			}
		}
	}

}

namespace Server.Items
{
	public class ToolHouseTarget : Target
	{
		private ToolHouse m_House;

		public ToolHouseTarget( ToolHouse house ) : base( 18, false, TargetFlags.None )
		{
			m_House = house;
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( m_House.Deleted )
				return;

			m_House.EndCombine( from, targeted );
		}
	}
}
/*
SewingKit
MortarPestle
ScribesPen
MalletAndChisel
Carpentry
FletcherTools
TinkerTools
BlackSmith & SmithCarpentry
Shovel & Pickaxe
SturdyShovel & SturdyPickaxe
GargoylesPickaxe
Lumberjacking & Axes
Cooking
Cartography
Glassblowing
TaxidermyKit
GargoylesAxe
GargoylesKnife
Lumberjacking
*/