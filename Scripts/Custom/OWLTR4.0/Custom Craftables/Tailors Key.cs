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
Modified by Tylius.
added scales + custom by daat99.
Modified a lof of the code by daat99.
Rewrote a lot of the code by daat99 and added custom leather on 13/01/2005.
////////////////////////////////////////
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
    public class TailorsKey : Item
    {
        private int m_Leather;
        private int m_Spined;
        private int m_Horned;
        private int m_Barbed;
		private int m_Polar;
		private int m_Synthetic;
		private int m_BlazeL;
		private int m_Daemonic;
		private int m_Shadow;
		private int m_Frost;
		private int m_Ethereal;
		private int m_Cloth;
        private int m_UncutCloth;
        private int m_BoltOfCloth;
		private int m_Yarn;
		private int m_SpoolOfThread;
        private int m_Wool;
		private int m_Cotton;
		private int m_RedScales;
		private int m_YellowScales;
		private int m_BlackScales;
		private int m_GreenScales;
		private int m_WhiteScales;
		private int m_BlueScales;
		private int m_CopperScales;
		private int m_SilverScales;
		private int m_GoldScales;
		private int m_StorageLimit;
        private int m_WithdrawIncrement;

        [CommandProperty(AccessLevel.GameMaster)]
        public int StorageLimit { get { return m_StorageLimit; } set { m_StorageLimit = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int WithdrawIncrement { get { return m_WithdrawIncrement; } set { m_WithdrawIncrement = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Leather { get { return m_Leather; } set { m_Leather = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Spined { get { return m_Spined; } set { m_Spined = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Horned { get { return m_Horned; } set { m_Horned = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Barbed { get { return m_Barbed; } set { m_Barbed = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Polar { get { return m_Polar; } set { m_Polar = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Synthetic { get { return m_Synthetic; } set { m_Synthetic = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int BlazeL { get { return m_BlazeL; } set { m_BlazeL = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Daemonic { get { return m_Daemonic; } set { m_Daemonic = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Shadow { get { return m_Shadow; } set { m_Shadow = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Frost { get { return m_Frost; } set { m_Frost = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Ethereal { get { return m_Ethereal; } set { m_Ethereal = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Cloth { get { return m_Cloth; } set { m_Cloth = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int UncutCloth { get { return m_UncutCloth; } set { m_UncutCloth = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BoltOfCloth { get { return m_BoltOfCloth; } set { m_BoltOfCloth = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Yarn { get { return m_Yarn; } set { m_Yarn = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int SpoolOfThread { get { return m_SpoolOfThread; } set { m_SpoolOfThread = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Wool { get { return m_Wool; } set { m_Wool = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Cotton { get { return m_Cotton; } set {m_Cotton = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int RedScales { get { return m_RedScales; } set { m_RedScales = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int YellowScales { get { return m_YellowScales; } set { m_YellowScales = value; InvalidateProperties(); } }
		
		[CommandProperty(AccessLevel.GameMaster)]
		public int BlackScales { get { return m_BlackScales; } set { m_BlackScales = value; InvalidateProperties(); } }
		
		[CommandProperty(AccessLevel.GameMaster)]
		public int GreenScales { get { return m_GreenScales; } set { m_GreenScales = value; InvalidateProperties(); } }
		
		[CommandProperty(AccessLevel.GameMaster)]
		public int WhiteScales { get { return m_WhiteScales; } set { m_WhiteScales = value; InvalidateProperties(); } }
		
		[CommandProperty(AccessLevel.GameMaster)]
		public int BlueScales { get { return m_BlueScales; } set { m_BlueScales = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int CopperScales { get { return m_CopperScales; } set { m_CopperScales = value; InvalidateProperties(); } }
		
		[CommandProperty(AccessLevel.GameMaster)]
		public int SilverScales { get { return m_SilverScales; } set { m_SilverScales = value; InvalidateProperties(); } }
		
		[CommandProperty(AccessLevel.GameMaster)]
		public int GoldScales { get { return m_GoldScales; } set { m_GoldScales = value; InvalidateProperties(); } }

        [Constructable]
        public TailorsKey() : base( 0x176B )
        {
            Movable = true;
            Weight = 1.0;
            Hue = 69;
            Name = "Tailor's Keys";
            //LootType = LootType.Blessed;
            StorageLimit = 60000;
            WithdrawIncrement = 100;
        }

        [Constructable]
        public TailorsKey(int storageLimit, int withdrawIncrement) : base( 0x176B )
        {
            Movable = true;
            Weight = 1.0;
            Hue = 69;
            Name = "Tailor's Keys";
            //LootType = LootType.Blessed;
            StorageLimit = storageLimit;
            WithdrawIncrement = withdrawIncrement;
        }

        public override void OnDoubleClick(Mobile from)
        {
			if (!(from is PlayerMobile))
				return;
			if (IsChildOf(from.Backpack))
				from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
			else
				from.SendMessage("This must be in your backpack.");
        }

        public void BeginCombine(Mobile from)
        {
            from.Target = new TailorsKeyTarget(this);
        }

        public void EndCombine(Mobile from, object o)
        {
			if (o is Item && (((Item)o).IsChildOf(from.Backpack) || ((Item)o).IsChildOf(from.BankBox)))
            {
                Item curItem = o as Item;
                if (curItem is Leather)
                {
                    if (Leather + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Leather + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Leather += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is SpinedLeather)
                {
                    if (Spined + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Spined + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Spined += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is HornedLeather)
                {
                    if (Horned + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Horned + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Horned += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is BarbedLeather)
                {
                    if (Barbed + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Barbed + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Barbed += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
				else if (curItem is PolarLeather)
				{
					if (Polar + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (Polar + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						Polar += curItem.Amount;
						curItem.Delete();
						from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is SyntheticLeather)
				{
					if (Synthetic + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (Synthetic + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						Synthetic += curItem.Amount;
						curItem.Delete();
						from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is BlazeLeather)
				{
					if (BlazeL + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (BlazeL + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						BlazeL += curItem.Amount;
						curItem.Delete();
						from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is DaemonicLeather)
				{
					if (Daemonic + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (Daemonic + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						Daemonic += curItem.Amount;
						curItem.Delete();
						from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is ShadowLeather)
				{
					if (Shadow + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (Shadow + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						Shadow += curItem.Amount;
						curItem.Delete();
						from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is FrostLeather)
				{
					if (Frost + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (Frost + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						Frost += curItem.Amount;
						curItem.Delete();
						from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is EtherealLeather)
				{
					if (Ethereal + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (Ethereal + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						Ethereal += curItem.Amount;
						curItem.Delete();
						from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is Cloth)
                {
                    if (Cloth + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Cloth + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Cloth += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is UncutCloth)
                {
                    if (UncutCloth + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (UncutCloth + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        UncutCloth += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is BoltOfCloth)
                {
                    if (BoltOfCloth + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (BoltOfCloth + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        BoltOfCloth += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is SpoolOfThread)
                {
                    if (SpoolOfThread + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (SpoolOfThread + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        SpoolOfThread += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is DarkYarn || curItem is LightYarn || curItem is LightYarnUnraveled)
                {
                    if (Yarn + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Yarn + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Yarn += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is Cotton)
                {
                    if (Cotton + (curItem.Amount * 2) > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Cotton + (curItem.Amount * 2)) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Cotton += (curItem.Amount * 2);
                        curItem.Delete();
                        from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
                else if (curItem is Wool || curItem is Flax)
                {
                    if (Wool + curItem.Amount > StorageLimit)
                        from.SendMessage("You are trying to add "+ ( (Wool + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
                    else
                    {
                        Wool += curItem.Amount;
                        curItem.Delete();
                        from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
                        BeginCombine(from);
                    }
                }
				else if (curItem is RedScales)
				{
					if (RedScales + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (RedScales + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						RedScales += curItem.Amount;
						curItem.Delete();
						from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is YellowScales)
				{
					if (YellowScales + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (YellowScales + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						YellowScales += curItem.Amount;
						curItem.Delete();
						from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is BlackScales)
				{
					if (BlackScales + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (BlackScales + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						BlackScales += curItem.Amount;
						curItem.Delete();
						from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is GreenScales)
				{
					if (GreenScales + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (GreenScales + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						GreenScales += curItem.Amount;
						curItem.Delete();
						from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is WhiteScales)
				{
					if (WhiteScales + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (WhiteScales + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						WhiteScales += curItem.Amount;
						curItem.Delete();
						from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is BlueScales)
				{
					if (BlueScales + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (BlueScales + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						BlueScales += curItem.Amount;
						curItem.Delete();
						from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is CopperScales)
				{
					if (CopperScales + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (CopperScales + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						CopperScales += curItem.Amount;
						curItem.Delete();
						from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is SilverScales)
				{
					if (SilverScales + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (SilverScales + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						SilverScales += curItem.Amount;
						curItem.Delete();
						from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
				else if (curItem is GoldScales)
				{
					if (GoldScales + curItem.Amount > StorageLimit)
						from.SendMessage("You are trying to add "+ ( (GoldScales + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
					else
					{
						GoldScales += curItem.Amount;
						curItem.Delete();
						from.SendGump(new TailorsKeyGump((PlayerMobile)from, this));
						BeginCombine(from);
					}
				}
            }
			else
				from.SendLocalizedMessage(1045158); // You must have the item in your backpack to target it.
		}

        public TailorsKey(Serial serial) : base( serial )
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
			writer.Write( (int) 2 ); // version
			//version 2
			writer.Write((int)m_SpoolOfThread);
			writer.Write((int)m_Cotton);
			//version 1
			writer.Write((int)m_BlazeL);
            //version 0
			writer.Write((int)m_Leather);
            writer.Write((int)m_Spined);
            writer.Write((int)m_Horned);
            writer.Write((int)m_Barbed);
			writer.Write((int)m_Polar);
			writer.Write((int)m_Synthetic);
			writer.Write((int)m_Daemonic);
			writer.Write((int)m_Shadow);
			writer.Write((int)m_Frost);
			writer.Write((int)m_Ethereal);
			writer.Write((int)m_Cloth);
            writer.Write((int)m_UncutCloth);
            writer.Write((int)m_BoltOfCloth);
            writer.Write((int)m_Yarn);
            writer.Write((int)m_Wool);
			writer.Write((int)m_RedScales);
			writer.Write((int)m_YellowScales);
			writer.Write((int)m_BlackScales);
			writer.Write((int)m_GreenScales);
			writer.Write((int)m_WhiteScales);
			writer.Write((int)m_BlueScales);
			writer.Write((int)m_CopperScales);
			writer.Write((int)m_SilverScales);
			writer.Write((int)m_GoldScales);
			writer.Write((int)m_StorageLimit);
            writer.Write((int)m_WithdrawIncrement);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
			int version = reader.ReadInt();
			
			switch ( version )
			{
				case 2:
				{
					m_SpoolOfThread = reader.ReadInt();
					m_Cotton = reader.ReadInt();
					goto case 1;
				}
				case 1:
				{
					m_BlazeL = reader.ReadInt();
					goto case 0;
				}
				case 0:
				{
					m_Leather = reader.ReadInt();
					m_Spined = reader.ReadInt();
					m_Horned = reader.ReadInt();
					m_Barbed = reader.ReadInt();
					m_Polar = reader.ReadInt();
					m_Synthetic = reader.ReadInt();
					m_Daemonic = reader.ReadInt();
					m_Shadow = reader.ReadInt();
					m_Frost = reader.ReadInt();
					m_Ethereal = reader.ReadInt();
					m_Cloth = reader.ReadInt();
					m_UncutCloth = reader.ReadInt();
					m_BoltOfCloth = reader.ReadInt();
					m_Yarn = reader.ReadInt();
					m_Wool = reader.ReadInt();
					m_RedScales = reader.ReadInt();
					m_YellowScales = reader.ReadInt();
					m_BlackScales = reader.ReadInt();
					m_GreenScales = reader.ReadInt();
					m_WhiteScales = reader.ReadInt();
					m_BlueScales = reader.ReadInt();
					m_CopperScales = reader.ReadInt();
					m_SilverScales = reader.ReadInt();
					m_GoldScales = reader.ReadInt();
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
    public class TailorsKeyGump : Gump
    {
        private PlayerMobile m_From;
        private TailorsKey m_Key;

        public TailorsKeyGump(PlayerMobile from, TailorsKey key ) : base( 25, 25 )
        {
            m_From = from;
            m_Key = key;

            m_From.CloseGump(typeof(TailorsKeyGump));

            AddPage(0);

            AddBackground(50, 10, 455, 430, 5054);
            AddImageTiled(60, 20, 435, 410, 2624);
            AddAlphaRegion(60, 20, 435, 410);

            AddLabel(210, 25, 88, "Tailor's Warehouse");

            AddLabel(125, 50, 0x486, "Leather");
            AddLabel(225, 50, 0x480, key.Leather.ToString());
            AddButton(75, 50, 4005, 4007, (m_Key.Leather <= 0) ? 999 : 1, GumpButtonType.Reply, 0);

            AddLabel(125, 75, 0x486, "Spined Leather");
            AddLabel(225, 75, 0x480, key.Spined.ToString());
            AddButton(75, 75, 4005, 4007, (m_Key.Spined <= 0) ? 999 : 2, GumpButtonType.Reply, 0);

            AddLabel(125, 100, 0x486, "Horned Leather");
            AddLabel(225, 100, 0x480, key.Horned.ToString());
            AddButton(75, 100, 4005, 4007, (m_Key.Horned <= 0) ? 999 : 3, GumpButtonType.Reply, 0);

            AddLabel(125, 125, 0x486, "Barbed Leather");
            AddLabel(225, 125, 0x480, key.Barbed.ToString());
            AddButton(75, 125, 4005, 4007, (m_Key.Barbed <= 0) ? 999 : 4, GumpButtonType.Reply, 0);

			AddLabel(125, 150, 0x486, "Polar Leather");
			AddLabel(225, 150, 0x480, key.Polar.ToString());
			AddButton(75, 150, 4005, 4007, (m_Key.Polar <= 0) ? 999 : 5, GumpButtonType.Reply, 0);

			AddLabel(125, 175, 0x486, "Synthetic Leather");
			AddLabel(225, 175, 0x480, key.Synthetic.ToString());
			AddButton(75, 175, 4005, 4007, (m_Key.Synthetic <= 0) ? 999 : 6, GumpButtonType.Reply, 0);

			AddLabel(125, 200, 0x486, "Blaze Leather");
			AddLabel(225, 200, 0x480, key.BlazeL.ToString());
			AddButton(75, 200, 4005, 4007, (m_Key.BlazeL <= 0) ? 999 : 7, GumpButtonType.Reply, 0);

			AddLabel(125, 225, 0x486, "Daemonic Leather");
			AddLabel(225, 225, 0x480, key.Daemonic.ToString());
			AddButton(75, 225, 4005, 4007, (m_Key.Daemonic <= 0) ? 999 : 8, GumpButtonType.Reply, 0);

			AddLabel(125, 250, 0x486, "Shadow Leather");
			AddLabel(225, 250, 0x480, key.Shadow.ToString());
			AddButton(75, 250, 4005, 4007, (m_Key.Shadow <= 0) ? 999 : 9, GumpButtonType.Reply, 0);
			
			AddLabel(125, 275, 0x486, "Frost Leather");
			AddLabel(225, 275, 0x480, key.Frost.ToString());
			AddButton(75, 275, 4005, 4007, (m_Key.Frost <= 0) ? 999 : 10, GumpButtonType.Reply, 0);

			AddLabel(125, 300, 0x486, "Ethereal Leather");
			AddLabel(225, 300, 0x480, key.Ethereal.ToString());
			AddButton(75, 300, 4005, 4007, (m_Key.Ethereal <= 0) ? 999 : 11, GumpButtonType.Reply, 0);
						
			AddLabel(325, 50, 0x486, "Red Scales");
			AddLabel(425, 50, 0x480, key.RedScales.ToString());
			AddButton(275, 50, 4005, 4007, (m_Key.RedScales <= 0) ? 999 : 12, GumpButtonType.Reply, 0);

			AddLabel(325, 75, 0x486, "Yellow Scales");
			AddLabel(425, 75, 0x480, key.YellowScales.ToString());
			AddButton(275, 75, 4005, 4007, (m_Key.YellowScales <= 0) ? 999 : 13, GumpButtonType.Reply, 0);

			AddLabel(325, 100, 0x486, "Black Scales");
			AddLabel(425, 100, 0x480, key.BlackScales.ToString());
			AddButton(275, 100, 4005, 4007, (m_Key.BlackScales <= 0) ? 999 : 14, GumpButtonType.Reply, 0);

			AddLabel(325, 125, 0x486, "Green Scales");
			AddLabel(425, 125, 0x480, key.GreenScales.ToString());
			AddButton(275, 125, 4005, 4007, (m_Key.GreenScales <= 0) ? 999 : 15, GumpButtonType.Reply, 0);

			AddLabel(325, 150, 0x486, "White Scales");
			AddLabel(425, 150, 0x480, key.WhiteScales.ToString());
			AddButton(275, 150, 4005, 4007, (m_Key.WhiteScales <= 0) ? 999 : 16, GumpButtonType.Reply, 0);

			AddLabel(325, 175, 0x486, "Blue Scales");
			AddLabel(425, 175, 0x480, key.BlueScales.ToString());
			AddButton(275, 175, 4005, 4007, (m_Key.BlueScales <= 0) ? 999 : 17, GumpButtonType.Reply, 0);
			
			AddLabel(325, 200, 0x486, "Copper Scales");
			AddLabel(425, 200, 0x480, key.CopperScales.ToString());
			AddButton(275, 200, 4005, 4007, (m_Key.CopperScales <= 0) ? 999 : 18, GumpButtonType.Reply, 0);

			AddLabel(325, 225, 0x486, "Silver Scales");
			AddLabel(425, 225, 0x480, key.SilverScales.ToString());
			AddButton(275, 225, 4005, 4007, (m_Key.SilverScales <= 0) ? 999 : 19, GumpButtonType.Reply, 0);

			AddLabel(325, 250, 0x486, "Gold Scales");
			AddLabel(425, 250, 0x480, key.GoldScales.ToString());
			AddButton(275, 250, 4005, 4007, (m_Key.GoldScales <= 0) ? 999 : 20, GumpButtonType.Reply, 0);

			AddLabel(125, 325, 0x486, "Cloth");
            AddLabel(225, 325, 0x480, key.Cloth.ToString());
            AddButton(75, 325, 4005, 4007, (m_Key.Cloth <= 0) ? 999 : 21, GumpButtonType.Reply, 0);

            AddLabel(125, 350, 0x486, "Uncut Cloth");
            AddLabel(225, 350, 0x480, key.UncutCloth.ToString());
            AddButton(75, 350, 4005, 4007, (m_Key.UncutCloth <= 0) ? 999 : 22, GumpButtonType.Reply, 0);

            AddLabel(125, 375, 0x486, "Bolt of Cloth");
            AddLabel(225, 375, 0x480, key.BoltOfCloth.ToString());
            AddButton(75, 375, 4005, 4007, (m_Key.BoltOfCloth <= 0) ? 999 : 23, GumpButtonType.Reply, 0);

			AddLabel(325, 275, 0x486, "Yarn");
            AddLabel(425, 275, 0x480, key.Yarn.ToString());
            AddButton(275, 275, 4005, 4007, (m_Key.Yarn<= 0) ? 999 : 24, GumpButtonType.Reply, 0);

            AddLabel(325, 300, 0x486, "Wool");
            AddLabel(425, 300, 0x480, key.Wool.ToString());
            AddButton(275, 300, 4005, 4007, (m_Key.Wool <= 0) ? 999 : 25, GumpButtonType.Reply, 0);

			AddLabel(325, 325, 0x486, "Cotton");
			AddLabel(425, 325, 0x480, key.Cotton.ToString());
			AddButton(275, 325, 4005, 4007, (m_Key.Cotton <= 0) ? 999 : 26, GumpButtonType.Reply, 0);

			AddLabel(125, 400, 0x486, "Spool of Thread");
			AddLabel(225, 400, 0x480, key.SpoolOfThread.ToString());
			AddButton(75, 400, 4005, 4007, (m_Key.SpoolOfThread <= 0) ? 999 : 27, GumpButtonType.Reply, 0);

			AddLabel(325, 375, 88, "Each Max:" );
			AddLabel(425, 375, 0x480, key.StorageLimit.ToString() );	

            AddLabel(325, 400, 88, "Add resource");
            AddButton(275, 400, 4005, 4007, 999, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Key.Deleted)
                return;

            else if (info.ButtonID == 1)
            {
                if (m_Key.Leather >= m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Leather(m_Key.WithdrawIncrement));
                    m_Key.Leather = m_Key.Leather - m_Key.WithdrawIncrement;
                    m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
                }
                else 
                {
                    m_From.AddToBackpack(new Leather(m_Key.Leather));
                    m_Key.Leather = 0;
                    m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
                }
            }
            else if (info.ButtonID == 2)
            {
                if (m_Key.Spined >= m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new SpinedLeather(m_Key.WithdrawIncrement));
                    m_Key.Spined = m_Key.Spined - m_Key.WithdrawIncrement;
                    m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
                }
                else
                {
                    m_From.AddToBackpack(new SpinedLeather(m_Key.Spined));
                    m_Key.Spined = 0;
                    m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
                }
            }
            else if (info.ButtonID == 3)
            {
                if (m_Key.Horned >= m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new HornedLeather(m_Key.WithdrawIncrement));
                    m_Key.Horned = m_Key.Horned - m_Key.WithdrawIncrement;
                    m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
                }
                else 
                {
                    m_From.AddToBackpack(new HornedLeather(m_Key.Horned));
                    m_Key.Horned = 0;
                    m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
                }
            }
            else if (info.ButtonID == 4)
            {
                if (m_Key.Barbed >= m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new BarbedLeather(m_Key.WithdrawIncrement));
                    m_Key.Barbed = m_Key.Barbed - m_Key.WithdrawIncrement;
                    m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
                }
                else 
                {
                    m_From.AddToBackpack(new BarbedLeather(m_Key.Barbed));
                    m_Key.Barbed = 0;
                    m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
                }
            }
			else if (info.ButtonID == 5)
			{
				if (m_Key.Polar >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new PolarLeather(m_Key.WithdrawIncrement));
					m_Key.Polar = m_Key.Polar - m_Key.WithdrawIncrement;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
				else 
				{
					m_From.AddToBackpack(new PolarLeather(m_Key.Polar));
					m_Key.Polar = 0;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
			}
			else if (info.ButtonID == 6)
			{
				if (m_Key.Synthetic >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new SyntheticLeather(m_Key.WithdrawIncrement));
					m_Key.Synthetic = m_Key.Synthetic - m_Key.WithdrawIncrement;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.AddToBackpack(new SyntheticLeather(m_Key.Synthetic));
					m_Key.Synthetic = 0;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
			}
			else if (info.ButtonID == 7)
			{
				if (m_Key.BlazeL >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new BlazeLeather(m_Key.WithdrawIncrement));
					m_Key.BlazeL = m_Key.BlazeL - m_Key.WithdrawIncrement;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.AddToBackpack(new BlazeLeather(m_Key.BlazeL));
					m_Key.BlazeL = 0;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
			}
			else if (info.ButtonID == 8)
			{
				if (m_Key.Daemonic >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new DaemonicLeather(m_Key.WithdrawIncrement));
					m_Key.Daemonic = m_Key.Daemonic - m_Key.WithdrawIncrement;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.AddToBackpack(new DaemonicLeather(m_Key.Daemonic));
					m_Key.Daemonic = 0;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
			}
			else if (info.ButtonID == 9)
			{
				if (m_Key.Shadow >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new ShadowLeather(m_Key.WithdrawIncrement));
					m_Key.Shadow = m_Key.Shadow - m_Key.WithdrawIncrement;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.AddToBackpack(new ShadowLeather(m_Key.Shadow));
					m_Key.Shadow = 0;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
			}
			else if (info.ButtonID == 10)
			{
				if (m_Key.Frost >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new FrostLeather(m_Key.WithdrawIncrement));
					m_Key.Frost = m_Key.Frost - m_Key.WithdrawIncrement;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.AddToBackpack(new FrostLeather(m_Key.Frost));
					m_Key.Frost = 0;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
			}
			else if (info.ButtonID == 11)
			{
				if (m_Key.Ethereal >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new EtherealLeather(m_Key.WithdrawIncrement));
					m_Key.Ethereal = m_Key.Ethereal - m_Key.WithdrawIncrement;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.AddToBackpack(new EtherealLeather(m_Key.Ethereal));
					m_Key.Ethereal = 0;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
			}
			else if (info.ButtonID == 12)
			{
				if (m_Key.RedScales >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new RedScales(m_Key.WithdrawIncrement));
					m_Key.RedScales = m_Key.RedScales - m_Key.WithdrawIncrement;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.AddToBackpack(new RedScales(m_Key.RedScales));
					m_Key.RedScales = 0;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
			}
			else if (info.ButtonID == 13)
			{
				if (m_Key.YellowScales >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new YellowScales(m_Key.WithdrawIncrement));
					m_Key.YellowScales = m_Key.YellowScales - m_Key.WithdrawIncrement;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.AddToBackpack(new YellowScales(m_Key.YellowScales));
					m_Key.YellowScales = 0;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
			}
			else if (info.ButtonID == 14)
			{
				if (m_Key.BlackScales >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new BlackScales(m_Key.WithdrawIncrement));
					m_Key.BlackScales = m_Key.BlackScales - m_Key.WithdrawIncrement;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.AddToBackpack(new BlackScales(m_Key.BlackScales));
					m_Key.BlackScales = 0;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
			}
			else if (info.ButtonID == 15)
			{
				if (m_Key.GreenScales >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new GreenScales(m_Key.WithdrawIncrement));
					m_Key.GreenScales = m_Key.GreenScales - m_Key.WithdrawIncrement;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.AddToBackpack(new GreenScales(m_Key.GreenScales));
					m_Key.GreenScales = 0;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
			}
			else if (info.ButtonID == 16)
			{
				if (m_Key.WhiteScales >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new WhiteScales(m_Key.WithdrawIncrement));
					m_Key.WhiteScales = m_Key.WhiteScales - m_Key.WithdrawIncrement;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.AddToBackpack(new WhiteScales(m_Key.WhiteScales));
					m_Key.WhiteScales = 0;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
			}
			else if (info.ButtonID == 17)
			{
				if (m_Key.BlueScales >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new BlueScales(m_Key.WithdrawIncrement));
					m_Key.BlueScales = m_Key.BlueScales - m_Key.WithdrawIncrement;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.AddToBackpack(new BlueScales(m_Key.BlueScales));
					m_Key.BlueScales = 0;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
			}
			else if (info.ButtonID == 18)
			{
				if (m_Key.CopperScales >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new CopperScales(m_Key.WithdrawIncrement));
					m_Key.CopperScales = m_Key.CopperScales - m_Key.WithdrawIncrement;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.AddToBackpack(new CopperScales(m_Key.CopperScales));
					m_Key.CopperScales = 0;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
			}
			else if (info.ButtonID == 19)
			{
				if (m_Key.SilverScales >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new SilverScales(m_Key.WithdrawIncrement));
					m_Key.SilverScales = m_Key.SilverScales - m_Key.WithdrawIncrement;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.AddToBackpack(new SilverScales(m_Key.SilverScales));
					m_Key.SilverScales = 0;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
			}
			else if (info.ButtonID == 20)
			{
				if (m_Key.GoldScales >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new GoldScales(m_Key.WithdrawIncrement));
					m_Key.GoldScales = m_Key.GoldScales - m_Key.WithdrawIncrement;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.AddToBackpack(new GoldScales(m_Key.GoldScales));
					m_Key.GoldScales = 0;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
			}

            else if (info.ButtonID == 21)
            {
                if (m_Key.Cloth >= m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Cloth(m_Key.WithdrawIncrement));
                    m_Key.Cloth = m_Key.Cloth - m_Key.WithdrawIncrement;
                    m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
                }
                else
                {
                    m_From.AddToBackpack(new Cloth(m_Key.Cloth));
                    m_Key.Cloth = 0;
                    m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
                }
			}
            else if (info.ButtonID == 22)
            {
                if (m_Key.UncutCloth >= m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new UncutCloth(m_Key.WithdrawIncrement));
                    m_Key.UncutCloth = m_Key.UncutCloth - m_Key.WithdrawIncrement;
                    m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
                }
                else
                {
                    m_From.AddToBackpack(new UncutCloth(m_Key.UncutCloth));
                    m_Key.UncutCloth = 0;
                    m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
                }
            }
            else if (info.ButtonID == 23)
            {
                if (m_Key.BoltOfCloth >= m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new BoltOfCloth(m_Key.WithdrawIncrement));
                    m_Key.BoltOfCloth = m_Key.BoltOfCloth - m_Key.WithdrawIncrement;
                    m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
                }
                else
                {
                    m_From.AddToBackpack(new BoltOfCloth(m_Key.BoltOfCloth));
                    m_Key.BoltOfCloth = 0;
                    m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
                }
            }
            else if (info.ButtonID == 24)
            {
                if (m_Key.Yarn >= m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new DarkYarn(m_Key.WithdrawIncrement));
                    m_Key.Yarn = m_Key.Yarn - m_Key.WithdrawIncrement;
                    m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
                }
                else
                {
                    m_From.AddToBackpack(new DarkYarn(m_Key.Yarn));
                    m_Key.Yarn = 0;
                    m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
                }
			}
			else if (info.ButtonID == 25)
            {
                if (m_Key.Wool >= m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new Wool(m_Key.WithdrawIncrement));
                    m_Key.Wool = m_Key.Wool - m_Key.WithdrawIncrement;
                    m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
                }
                else
                {
                    m_From.AddToBackpack(new Wool(m_Key.Wool));
                    m_Key.Wool = 0;
                    m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
                }
			}
			if (info.ButtonID == 26)
			{
				if (m_Key.Cotton >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new Cotton(m_Key.WithdrawIncrement));
					m_Key.Cotton = m_Key.Cotton - m_Key.WithdrawIncrement;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.AddToBackpack(new Cotton(m_Key.Cotton));
					m_Key.Cotton = 0;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
			}
			if (info.ButtonID == 27)
			{
				if (m_Key.SpoolOfThread >= m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new SpoolOfThread(m_Key.WithdrawIncrement));
					m_Key.SpoolOfThread = m_Key.SpoolOfThread - m_Key.WithdrawIncrement;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
				else
				{
					m_From.AddToBackpack(new SpoolOfThread(m_Key.SpoolOfThread));
					m_Key.SpoolOfThread = 0;
					m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				}
			}
			else if (info.ButtonID == 999)
			{
				m_From.SendMessage("Please select items to add to the tailors key.");
				m_From.SendGump(new TailorsKeyGump(m_From, m_Key));
				m_Key.BeginCombine(m_From);
			}
        }
    }
}

namespace Server.Items
{
    public class TailorsKeyTarget : Target
    {
        private TailorsKey m_Key;

        public TailorsKeyTarget(TailorsKey key) : base( 18, false, TargetFlags.None )
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