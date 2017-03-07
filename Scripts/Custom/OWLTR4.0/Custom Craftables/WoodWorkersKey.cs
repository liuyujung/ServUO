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

////////////////////////////////
Sources:

Ingot Key script by GoldDrac13
Granite Box script by (unknown)
////////////////////////////////
////////////////////////////////////////
Modified by Ashlar, beloved of Morrigan.  
Modified by Tylius.Also Modified by Morpheus and Dave
Modified by daat99 to fit a custom ore\wood pack.
Modified by daat99 to fit custom craftable pack.
MOdified by daat99 to accept both boards and logs and give back boards.
////////////////////////////////////////
This item is a resource storage key.
Add or remove references to fit your shard.  See IngotKey.cs for comments
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
    [FlipableAttribute(0xFEF, 0xFF0, 0xFF1, 0xFF2, 0xFF3, 0xFF4, 0xFBD, 0xFBE)]
    public class WoodWorkersKey : Item
    {
    	private int m_Board;
		private int m_OakBoard;
    	private int m_AshBoard;
		private int m_YewBoard;
		private int m_HeartwoodBoard;
		private int m_BloodwoodBoard;
		private int m_FrostwoodBoard;
    	private int m_EbonyBoard;
    	private int m_BambooBoard;
		private int m_PurpleHeartBoard;
		private int m_RedwoodBoard;
		private int m_PetrifiedBoard;
		private int m_Arrow;
        private int m_Bolt;
        private int m_Feather;
        private int m_Shaft;
        private int m_StorageLimit;
        private int m_WithdrawIncrement;

        [CommandProperty(AccessLevel.GameMaster)]
        public int StorageLimit { get { return m_StorageLimit; } set { m_StorageLimit = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int WithdrawIncrement { get { return m_WithdrawIncrement; } set { m_WithdrawIncrement = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Board { get { return m_Board; } set { m_Board = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int OakBoard { get { return m_OakBoard; } set { m_OakBoard = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int AshBoard { get { return m_AshBoard; } set { m_AshBoard = value; InvalidateProperties(); } }
        
        [CommandProperty(AccessLevel.GameMaster)]
		public int YewBoard { get { return m_YewBoard; } set { m_YewBoard = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int HeartwoodBoard { get { return m_HeartwoodBoard; } set { m_HeartwoodBoard = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int BloodwoodBoard { get { return m_BloodwoodBoard; } set { m_BloodwoodBoard = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int FrostwoodBoard { get { return m_FrostwoodBoard; } set { m_FrostwoodBoard = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int EbonyBoard { get { return m_EbonyBoard; } set { m_EbonyBoard = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int BambooBoard { get { return m_BambooBoard; } set { m_BambooBoard = value; InvalidateProperties(); } }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public int PurpleHeartBoard { get { return m_PurpleHeartBoard; } set { m_PurpleHeartBoard = value; InvalidateProperties(); } }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public int RedwoodBoard { get { return m_RedwoodBoard; } set { m_RedwoodBoard = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int PetrifiedBoard { get { return m_PetrifiedBoard; } set { m_PetrifiedBoard = value; InvalidateProperties(); } }
        
        [CommandProperty(AccessLevel.GameMaster)]
        public int Arrow { get { return m_Arrow; } set { m_Arrow = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Bolt { get { return m_Bolt; } set { m_Bolt = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Feather { get { return m_Feather; } set { m_Feather = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Shaft { get { return m_Shaft; } set { m_Shaft = value; InvalidateProperties(); } }

        [Constructable]
        public WoodWorkersKey() : base( 0x176B )
        {
            Movable = true;
            Weight = 1.0;
            Hue = 88;
            Name = "Wood Worker's Keys";
            //LootType = LootType.Blessed;
            StorageLimit = 60000;
            WithdrawIncrement = 100;
        }

        [Constructable]
        public WoodWorkersKey(int storageLimit, int withdrawIncrement) : base( 0x176B )
        {
            Movable = true;
            Weight = 1.0;
            Hue = 88;
            Name = "Wood Worker's Keys";
            //LootType = LootType.Blessed;
            StorageLimit = storageLimit;
            WithdrawIncrement = withdrawIncrement;
        }

        public override void OnDoubleClick(Mobile from)
        {
			if (!(from is PlayerMobile))
				return;
			if (IsChildOf(from.Backpack))
				from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
			else
				from.SendMessage("This must be in your backpack.");
        }
        public void BeginCombine(Mobile from)
        {
            from.Target = new WoodWorkersKeyTarget(this);
        }
        public void EndCombine(Mobile from, object o)
        {
			if (o is Item && (((Item)o).IsChildOf(from.Backpack) || ((Item)o).IsChildOf(from.BankBox)))
            {
                Item curItem = o as Item;
				if (curItem is Board)
				{
					if (Board + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (Board + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						Board += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is OakBoard)
				{
					if (OakBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add " + ((OakBoard + curItem.Amount) - m_StorageLimit) + " too much! The warehouse can store only " + m_StorageLimit + " of this resource.");
					else
					{
						OakBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is AshBoard)
				{
					if (AshBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add " + ((AshBoard + curItem.Amount) - m_StorageLimit) + " too much! The warehouse can store only " + m_StorageLimit + " of this resource.");
					else
					{
						AshBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is YewBoard)
				{
					if (YewBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add " + ((YewBoard + curItem.Amount) - m_StorageLimit) + " too much! The warehouse can store only " + m_StorageLimit + " of this resource.");
					else
					{
						YewBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is HeartwoodBoard)
				{
					if (HeartwoodBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add " + ((HeartwoodBoard + curItem.Amount) - m_StorageLimit) + " too much! The warehouse can store only " + m_StorageLimit + " of this resource.");
					else
					{
						HeartwoodBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is BloodwoodBoard)
				{
					if (BloodwoodBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add " + ((BloodwoodBoard + curItem.Amount) - m_StorageLimit) + " too much! The warehouse can store only " + m_StorageLimit + " of this resource.");
					else
					{
						BloodwoodBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is FrostwoodBoard)
				{
					if (FrostwoodBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add " + ((FrostwoodBoard + curItem.Amount) - m_StorageLimit) + " too much! The warehouse can store only " + m_StorageLimit + " of this resource.");
					else
					{
						FrostwoodBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is EbonyBoard)
				{
					if (EbonyBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add " + ((EbonyBoard + curItem.Amount) - m_StorageLimit) + " too much! The warehouse can store only " + m_StorageLimit + " of this resource.");
					else
					{
						EbonyBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is BambooBoard)
				{
					if (BambooBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add " + ((BambooBoard + curItem.Amount) - m_StorageLimit) + " too much! The warehouse can store only " + m_StorageLimit + " of this resource.");
					else
					{
						BambooBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is PurpleHeartBoard)
				{
					if (PurpleHeartBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (PurpleHeartBoard + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						PurpleHeartBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is RedwoodBoard)
				{
					if (RedwoodBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (RedwoodBoard + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						RedwoodBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is PetrifiedBoard)
				{
					if (PetrifiedBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (PetrifiedBoard + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						PetrifiedBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is Log)
				{
					if (Board + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (Board + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						Board += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is OakLog)
				{
					if (OakBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add " + ((OakBoard + curItem.Amount) - m_StorageLimit) + " too much! The warehouse can store only " + m_StorageLimit + " of this resource.");
					else
					{
						OakBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is AshLog)
				{
					if (AshBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add " + ((AshBoard + curItem.Amount) - m_StorageLimit) + " too much! The warehouse can store only " + m_StorageLimit + " of this resource.");
					else
					{
						AshBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is YewLog)
				{
					if (YewBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add " + ((YewBoard + curItem.Amount) - m_StorageLimit) + " too much! The warehouse can store only " + m_StorageLimit + " of this resource.");
					else
					{
						YewBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is HeartwoodLog)
				{
					if (HeartwoodBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add " + ((HeartwoodBoard + curItem.Amount) - m_StorageLimit) + " too much! The warehouse can store only " + m_StorageLimit + " of this resource.");
					else
					{
						HeartwoodBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is BloodwoodLog)
				{
					if (BloodwoodBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add " + ((BloodwoodBoard + curItem.Amount) - m_StorageLimit) + " too much! The warehouse can store only " + m_StorageLimit + " of this resource.");
					else
					{
						BloodwoodBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is FrostwoodLog)
				{
					if (FrostwoodBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add " + ((FrostwoodBoard + curItem.Amount) - m_StorageLimit) + " too much! The warehouse can store only " + m_StorageLimit + " of this resource.");
					else
					{
						FrostwoodBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is EbonyLog)
				{
					if (EbonyBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add " + ((EbonyBoard + curItem.Amount) - m_StorageLimit) + " too much! The warehouse can store only " + m_StorageLimit + " of this resource.");
					else
					{
						EbonyBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is BambooLog)
				{
					if (BambooBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add " + ((BambooBoard + curItem.Amount) - m_StorageLimit) + " too much! The warehouse can store only " + m_StorageLimit + " of this resource.");
					else
					{
						BambooBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is PurpleHeartLog)
				{
					if (PurpleHeartBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (PurpleHeartBoard + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						PurpleHeartBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is RedwoodLog)
				{
					if (RedwoodBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (RedwoodBoard + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						RedwoodBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is PetrifiedLog)
				{
					if (PetrifiedBoard + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (PetrifiedBoard + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						PetrifiedBoard += curItem.Amount;
						curItem.Delete();
						from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is Arrow)
                {
                    if (Arrow + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Arrow + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Arrow += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is Bolt)
                {
                    if (Bolt + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Bolt + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Bolt += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is Feather)
                {
                    if (Feather + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Feather + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Feather += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is Shaft)
                {
                    if (Shaft + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Shaft + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Shaft += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new WoodWorkersKeyGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
            }
            else
            {
                from.SendLocalizedMessage(1045158); // You must have the item in your backpack to target it.
            }
        }
        public WoodWorkersKey(Serial serial) : base( serial )
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
			writer.Write( (int) 0 ); // version

        	writer.Write((int)m_Board);
			writer.Write((int)m_OakBoard);
			writer.Write((int)m_AshBoard);
			writer.Write((int)m_YewBoard);
			writer.Write((int)m_HeartwoodBoard);
			writer.Write((int)m_BloodwoodBoard);
			writer.Write((int)m_FrostwoodBoard);
			writer.Write((int)m_EbonyBoard);
			writer.Write((int)m_BambooBoard);
        	writer.Write((int)m_PurpleHeartBoard);
        	writer.Write((int)m_RedwoodBoard);
        	writer.Write((int)m_PetrifiedBoard);
			writer.Write((int)m_Arrow);
            writer.Write((int)m_Bolt);
            writer.Write((int)m_Feather);
            writer.Write((int)m_Shaft);
            writer.Write((int)m_StorageLimit);
            writer.Write((int)m_WithdrawIncrement);
        }
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					goto case 0;
				case 0:
				{
					m_Board = reader.ReadInt();
					m_OakBoard = reader.ReadInt();
					m_AshBoard = reader.ReadInt();
					m_YewBoard = reader.ReadInt();
					m_HeartwoodBoard = reader.ReadInt();
					m_BloodwoodBoard = reader.ReadInt();
					m_FrostwoodBoard = reader.ReadInt();
					m_EbonyBoard = reader.ReadInt();
					m_BambooBoard = reader.ReadInt();
					m_PurpleHeartBoard = reader.ReadInt();
					m_RedwoodBoard = reader.ReadInt();
					m_PetrifiedBoard = reader.ReadInt();
					m_Arrow = reader.ReadInt();
					m_Bolt = reader.ReadInt();
					m_Feather = reader.ReadInt();
					m_Shaft = reader.ReadInt();
					m_StorageLimit = reader.ReadInt();
					m_WithdrawIncrement = reader.ReadInt();
					break;
				}
			}
		}
    }
}

namespace Server.Items
{
    public class WoodWorkersKeyGump : Gump
    {
        private PlayerMobile m_From;
        private WoodWorkersKey m_Key;

        public WoodWorkersKeyGump(PlayerMobile from, WoodWorkersKey key) : base( 25, 25 )
        {
            m_From = from;
            m_Key = key;

            m_From.CloseGump(typeof(WoodWorkersKeyGump));

            AddPage(0);
			
			AddBackground(50, 10, 455, 285, 5054);
			AddImageTiled(58, 20, 438, 271, 2624);
			AddAlphaRegion(58, 20, 438, 271);

            AddLabel(200, 25, 88, "Wood Worker's Warehouse");

            AddLabel(125, 50, 0x486, "Board");
            AddLabel(225, 50, 0x480, key.Board.ToString());
            AddButton(75, 50, 4005, 4007, 1, GumpButtonType.Reply, 0);

			AddLabel(125, 75, 0x486, "Oak");
			AddLabel(225, 75, 0x480, key.OakBoard.ToString());
			AddButton(75, 75, 4005, 4007, 2, GumpButtonType.Reply, 0);

			AddLabel(125, 100, 0x486, "Ash");
			AddLabel(225, 100, 0x480, key.AshBoard.ToString());
			AddButton(75, 100, 4005, 4007, 3, GumpButtonType.Reply, 0);

			AddLabel(125, 125, 0x486, "Yew");
			AddLabel(225, 125, 0x480, key.YewBoard.ToString());
			AddButton(75, 125, 4005, 4007, 4, GumpButtonType.Reply, 0);
			
			AddLabel(125, 150, 0x486, "Heartwood");
			AddLabel(225, 150, 0x480, key.HeartwoodBoard.ToString());
			AddButton(75, 150, 4005, 4007, 5, GumpButtonType.Reply, 0);
        	
			AddLabel(125, 175, 0x486, "Bloodwood");
			AddLabel(225, 175, 0x480, key.BloodwoodBoard.ToString());
			AddButton(75, 175, 4005, 4007, 6, GumpButtonType.Reply, 0);
			
			AddLabel(125, 200, 0x486, "Frostwood");
			AddLabel(225, 200, 0x480, key.FrostwoodBoard.ToString());
			AddButton(75, 200, 4005, 4007, 7, GumpButtonType.Reply, 0);
			
			AddLabel(125, 225, 0x486, "Ebony");
			AddLabel(225, 225, 0x480, key.EbonyBoard.ToString());
			AddButton(75, 225, 4005, 4007, 8, GumpButtonType.Reply, 0);
      	
			AddLabel(125, 250, 0x486, "Bamboo");
			AddLabel(225, 250, 0x480, key.BambooBoard.ToString());
			AddButton(75, 250, 4005, 4007, 9, GumpButtonType.Reply, 0);
			
			AddLabel(325, 50, 0x486, "PurpleHeart");
            AddLabel(425, 50, 0x480, key.PurpleHeartBoard.ToString());
            AddButton(275, 50, 4005, 4007, 10, GumpButtonType.Reply, 0);

			AddLabel(325, 75, 0x486, "Redwood");
			AddLabel(425, 75, 0x480, key.RedwoodBoard.ToString());
			AddButton(275, 75, 4005, 4007, 11, GumpButtonType.Reply, 0);

			AddLabel(325, 100, 0x486, "Petrified");
			AddLabel(425, 100, 0x480, key.PetrifiedBoard.ToString());
			AddButton(275, 100, 4005, 4007, 12, GumpButtonType.Reply, 0);
			
			AddLabel(325, 125, 0x486, "Arrow");
            AddLabel(425, 125, 0x480, key.Arrow.ToString());
            AddButton(275, 125, 4005, 4007, 13, GumpButtonType.Reply, 0);

			AddLabel(325, 150, 0x486, "Bolt");
            AddLabel(425, 150, 0x480, key.Bolt.ToString());
            AddButton(275, 150, 4005, 4007, 14, GumpButtonType.Reply, 0);
        	
			AddLabel(325, 175, 0x486, "Feather");
            AddLabel(425, 175, 0x480, key.Feather.ToString());
            AddButton(275, 175, 4005, 4007, 15, GumpButtonType.Reply, 0);
        	
			AddLabel(325, 200, 0x486, "Shaft");
            AddLabel(425, 200, 0x480, key.Shaft.ToString());
            AddButton(275, 200, 4005, 4007, 16, GumpButtonType.Reply, 0);
        				
			AddLabel(325, 225, 88, "Each Max:" );
			AddLabel(425, 225, 0x480, key.StorageLimit.ToString() );	

			AddLabel(325, 250, 88, "Add resource");
			AddButton(275, 250, 4005, 4007, 17, GumpButtonType.Reply, 0);
        }
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Key.Deleted)
                return;
			else if (info.ButtonID == 1)
			{
				if (m_Key.Board >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new Board(m_Key.WithdrawIncrement));
					m_Key.Board = m_Key.Board - m_Key.WithdrawIncrement;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else if (m_Key.Board > 0)
				{
					m_From.AddToBackpack(new Board(m_Key.Board));
					m_Key.Board = 0;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.SendMessage("You do not have any of that board!");
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
					m_Key.BeginCombine(m_From);
				}
			}            
			else if (info.ButtonID == 2)
			{
				if (m_Key.OakBoard >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new OakBoard(m_Key.WithdrawIncrement));
					m_Key.OakBoard = m_Key.OakBoard - m_Key.WithdrawIncrement;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else if (m_Key.OakBoard > 0)
				{
					m_From.AddToBackpack(new OakBoard(m_Key.OakBoard));
					m_Key.OakBoard = 0;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.SendMessage("You do not have any of that board!");
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
					m_Key.BeginCombine(m_From);
				}
			}
			else if (info.ButtonID == 3)
			{
				if (m_Key.AshBoard >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new AshBoard(m_Key.WithdrawIncrement));
					m_Key.AshBoard = m_Key.AshBoard - m_Key.WithdrawIncrement;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else if (m_Key.AshBoard > 0)
				{
					m_From.AddToBackpack(new AshBoard(m_Key.AshBoard));
					m_Key.AshBoard = 0;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.SendMessage("You do not have any of that board!");
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
					m_Key.BeginCombine(m_From);
				}
			}
			else if (info.ButtonID == 4)
			{
				if (m_Key.YewBoard >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new YewBoard(m_Key.WithdrawIncrement));
					m_Key.YewBoard = m_Key.YewBoard - m_Key.WithdrawIncrement;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else if (m_Key.YewBoard > 0)
				{
					m_From.AddToBackpack(new YewBoard(m_Key.YewBoard));
					m_Key.YewBoard = 0;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.SendMessage("You do not have any of that board!");
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
					m_Key.BeginCombine(m_From);
				}
			}
			else if (info.ButtonID == 5)
			{
				if (m_Key.HeartwoodBoard >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new HeartwoodBoard(m_Key.WithdrawIncrement));
					m_Key.HeartwoodBoard = m_Key.HeartwoodBoard - m_Key.WithdrawIncrement;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else if (m_Key.HeartwoodBoard > 0)
				{
					m_From.AddToBackpack(new HeartwoodBoard(m_Key.HeartwoodBoard));
					m_Key.HeartwoodBoard = 0;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.SendMessage("You do not have any of that board!");
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
					m_Key.BeginCombine(m_From);
				}
			}
			else if (info.ButtonID == 6)
			{
				if (m_Key.BloodwoodBoard >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new BloodwoodBoard(m_Key.WithdrawIncrement));
					m_Key.BloodwoodBoard = m_Key.BloodwoodBoard - m_Key.WithdrawIncrement;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else if (m_Key.BloodwoodBoard > 0)
				{
					m_From.AddToBackpack(new BloodwoodBoard(m_Key.BloodwoodBoard));
					m_Key.BloodwoodBoard = 0;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.SendMessage("You do not have any of that board!");
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
					m_Key.BeginCombine(m_From);
				}
			}
			else if (info.ButtonID == 7)
			{
				if (m_Key.FrostwoodBoard >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new FrostwoodBoard(m_Key.WithdrawIncrement));
					m_Key.FrostwoodBoard = m_Key.FrostwoodBoard - m_Key.WithdrawIncrement;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else if (m_Key.FrostwoodBoard > 0)
				{
					m_From.AddToBackpack(new FrostwoodBoard(m_Key.FrostwoodBoard));
					m_Key.FrostwoodBoard = 0;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.SendMessage("You do not have any of that board!");
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
					m_Key.BeginCombine(m_From);
				}
			}
			else if (info.ButtonID == 8)
			{
				if (m_Key.EbonyBoard >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new EbonyBoard(m_Key.WithdrawIncrement));
					m_Key.EbonyBoard = m_Key.EbonyBoard - m_Key.WithdrawIncrement;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else if (m_Key.EbonyBoard > 0)
				{
					m_From.AddToBackpack(new EbonyBoard(m_Key.EbonyBoard));
					m_Key.EbonyBoard = 0;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.SendMessage("You do not have any of that board!");
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
					m_Key.BeginCombine(m_From);
				}
			}
            else if (info.ButtonID == 9)
            {
                if (m_Key.BambooBoard >= m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new BambooBoard(m_Key.WithdrawIncrement));
                    m_Key.BambooBoard = m_Key.BambooBoard - m_Key.WithdrawIncrement;
                    m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
                }
                else if (m_Key.BambooBoard > 0)
                {
                    m_From.AddToBackpack(new BambooBoard(m_Key.BambooBoard));
                    m_Key.BambooBoard = 0;
                    m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
                }
                else
                {
                    m_From.SendMessage("You do not have any of that board!");
                    m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
                    m_Key.BeginCombine(m_From);
                }
            }
			else if (info.ButtonID == 10)
			{
				if (m_Key.PurpleHeartBoard >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new PurpleHeartBoard(m_Key.WithdrawIncrement));
					m_Key.PurpleHeartBoard = m_Key.PurpleHeartBoard - m_Key.WithdrawIncrement;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else if (m_Key.PurpleHeartBoard > 0)
				{
					m_From.AddToBackpack(new PurpleHeartBoard(m_Key.PurpleHeartBoard));
					m_Key.PurpleHeartBoard = 0;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.SendMessage("You do not have any of that board!");
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
					m_Key.BeginCombine(m_From);
				}
			}
			else if (info.ButtonID == 11)
			{
				if (m_Key.RedwoodBoard >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new RedwoodBoard(m_Key.WithdrawIncrement));
					m_Key.RedwoodBoard = m_Key.RedwoodBoard - m_Key.WithdrawIncrement;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else if (m_Key.RedwoodBoard > 0)
				{
					m_From.AddToBackpack(new RedwoodBoard(m_Key.RedwoodBoard));
					m_Key.RedwoodBoard = 0;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.SendMessage("You do not have any of that board!");
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
					m_Key.BeginCombine(m_From);
				}
			}
			else if (info.ButtonID == 12)
			{
				if (m_Key.PetrifiedBoard >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new PetrifiedBoard(m_Key.WithdrawIncrement));
					m_Key.PetrifiedBoard = m_Key.PetrifiedBoard - m_Key.WithdrawIncrement;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else if (m_Key.PetrifiedBoard > 0)
				{
					m_From.AddToBackpack(new PetrifiedBoard(m_Key.PetrifiedBoard));
					m_Key.PetrifiedBoard = 0;
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.SendMessage("You do not have any of that board!");
					m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
					m_Key.BeginCombine(m_From);
				}
			}			
			else if (info.ButtonID == 13)
            {
                if (m_Key.Arrow >= m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Arrow(m_Key.WithdrawIncrement));
                    m_Key.Arrow = m_Key.Arrow - m_Key.WithdrawIncrement;
                    m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
                }
                else if (m_Key.Arrow > 0)
                {
                    m_From.AddToBackpack(new Arrow(m_Key.Arrow));
                    m_Key.Arrow = 0;
                    m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
                }
                else
                {
                    m_From.SendMessage("You do not have any arrows stored!");
                    m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
                    m_Key.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 14)
            {
                if (m_Key.Bolt > m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Bolt(m_Key.WithdrawIncrement));
                    m_Key.Bolt = m_Key.Bolt - m_Key.WithdrawIncrement;
                    m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
                }
                else if (m_Key.Bolt > 0)
                {
                    m_From.AddToBackpack(new Bolt(m_Key.Bolt));
                    m_Key.Bolt = 0;
                    m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
                }
                else
                {
                    m_From.SendMessage("You do not have any bolts stored!");
                    m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
                    m_Key.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 15)
            {
                if (m_Key.Feather > m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Feather(m_Key.WithdrawIncrement));
                    m_Key.Feather = m_Key.Feather - m_Key.WithdrawIncrement;
                    m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
                }
                else if (m_Key.Feather > 0)
                {
                    m_From.AddToBackpack(new Feather(m_Key.Feather));
                    m_Key.Feather = 0;
                    m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
                }
                else
                {
                    m_From.SendMessage("You do not have any feathers stored!");
                    m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
                    m_Key.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 16)
            {
                if (m_Key.Shaft > m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Shaft(m_Key.WithdrawIncrement));
                    m_Key.Shaft = m_Key.Shaft - m_Key.WithdrawIncrement;
                    m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
                }
                else if (m_Key.Shaft > 0)
                {
                    m_From.AddToBackpack(new Shaft(m_Key.Shaft));
                    m_Key.Shaft = 0;
                    m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
                }
                else
                {
                    m_From.SendMessage("You do not have any shafts stored!");
                    m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
                    m_Key.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 17)
            {
                m_From.SendGump(new WoodWorkersKeyGump(m_From, m_Key));
                m_Key.BeginCombine(m_From);
            }
        }
    }
}

namespace Server.Items
{
    public class WoodWorkersKeyTarget : Target
    {
        private WoodWorkersKey m_Key;

        public WoodWorkersKeyTarget(WoodWorkersKey key) : base( 18, false, TargetFlags.None )
        {
            m_Key = key;
        }
        protected override void OnTarget(Mobile from, object targeted)
        {
            if (m_Key.Deleted)
                return;

            m_Key.EndCombine(from, targeted);
        }
    }
}
