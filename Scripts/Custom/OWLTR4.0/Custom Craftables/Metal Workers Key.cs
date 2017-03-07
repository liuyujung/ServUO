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

Sources:

Ingot Key script by GoldDrac13
Granite Box script by (unknown)
BankCrystal script by (unknown)
////////////////////////////////
////////////////////////////////////////
Modified by Ashlar, beloved of Morrigan.  
Modified by Tylius.
Modified gump and added custom ingots by daat99.
Modified a lof of the code by daat99.
//////////////////////////////////////////
This item is a resource storage key as well as a forge (backpack or ground), an anvil (ground only), and a banker (spoken). 
Add or remove references to fit your shard.  
Note however, that adding such items that have a number of uses, like shovels, will allow the player to put a almost caput 
shovel in and pop it back out with 50 uses left.
*/
using System;					//To be honest, I am not sure if all this is needed, but the script works!
using System.Collections;
using Server;
using Server.Prompts;
using Server.Mobiles;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Misc; 
using Server.Network;
using Server.Targeting;
using Server.Multis;
using Server.Regions;


namespace Server.Items
{
	[FlipableAttribute(0xFEF, 0xFF0, 0xFF1, 0xFF2, 0xFF3, 0xFF4, 0xFBD, 0xFBE)]

	public class MetalWorkersKey : Item
	{
		private int m_Iron;		//Declare all our resources as integer (number) variables.
		private int m_DullCopper;
		private int m_ShadowIron;
		private int m_Copper;
		private int m_Bronze;
		private int m_Gold;
		private int m_Agapite;
		private int m_Verite;
		private int m_Valorite;
		// custom ores daat99 1/7
		private int m_Blaze;
		private int m_Ice;
		private int m_Toxic;
		private int m_Electrum;
		private int m_Platinum;
		//
		private int m_StorageLimit;
		private int m_WithdrawIncrement;

		//This section allows GM's and above to change the amounts of the various properties of the key.
		[CommandProperty(AccessLevel.GameMaster)]
		public int StorageLimit { get { return m_StorageLimit; } set { m_StorageLimit = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int WithdrawIncrement { get { return m_WithdrawIncrement; } set { m_WithdrawIncrement = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Iron { get { return m_Iron; } set { m_Iron = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int DullCopper { get { return m_DullCopper; } set { m_DullCopper = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int ShadowIron { get { return m_ShadowIron; } set { m_ShadowIron = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Copper { get { return m_Copper; } set { m_Copper = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Bronze { get { return m_Bronze; } set { m_Bronze = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Gold { get { return m_Gold; } set { m_Gold = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Agapite { get { return m_Agapite; } set { m_Agapite = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Verite { get { return m_Verite; } set { m_Verite = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Valorite { get { return m_Valorite; } set { m_Valorite = value; InvalidateProperties(); } }

		// custom ores daat99 2/7
		[CommandProperty(AccessLevel.GameMaster)]
		public int Blaze { get { return m_Blaze; } set { m_Blaze = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Ice { get { return m_Ice; } set { m_Ice = value; InvalidateProperties(); } }
		
		[CommandProperty(AccessLevel.GameMaster)]
		public int Toxic { get { return m_Toxic; } set { m_Toxic = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Electrum { get { return m_Electrum; } set { m_Electrum = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Platinum { get { return m_Platinum; } set { m_Platinum = value; InvalidateProperties(); } }
		//

		//This is the default item you get when you [add MetalWorkersKey
		[Constructable]
		public MetalWorkersKey() : base( 0x176B )
		{
			Movable = true;
			Weight = 1.0;
			Hue = 20;
			Name = "Metal Worker's Keys";
			//LootType = LootType.Blessed;
			StorageLimit = 60000;
			WithdrawIncrement = 100;
		}

		[Constructable]
		public MetalWorkersKey(int storageLimit, int withdrawIncrement) : base( 0x176B )
		{
			Movable = true;
			Weight = 10.0;
			Hue = 20;
			Name = "Metal Worker's Keys";
			//LootType = LootType.Blessed;
			StorageLimit = storageLimit;
			WithdrawIncrement = withdrawIncrement;

		}
		public override void OnDoubleClick(Mobile from)
		{
			if (!(from is PlayerMobile))
				return;
			if (IsChildOf(from.Backpack))
				from.SendGump(new MetalWorkersKeyGump((PlayerMobile)from, this));
			else
				from.SendMessage("This must be in your backpack.");
		}
		public void BeginCombine(Mobile from)
		{
			from.Target = new MetalWorkersKeyTarget(this);
		}
		public void EndCombine(Mobile from, object o)
		{
			if (o is Item && (((Item)o).IsChildOf(from.Backpack) || ((Item)o).IsChildOf(from.BankBox)))
			{
				Item curItem = o as Item;
				if (curItem is BaseIngot)
				{
					if (curItem is IronIngot)
					{
						if (Iron + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Iron + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
						else
						{
							Iron += curItem.Amount;
							curItem.Delete();
							from.SendGump(new MetalWorkersKeyGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is DullCopperIngot)
					{
						if (DullCopper + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (DullCopper + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
						else
						{
							DullCopper += curItem.Amount;
							curItem.Delete();
							from.SendGump(new MetalWorkersKeyGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is ShadowIronIngot)
					{
						if (ShadowIron + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (ShadowIron + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
						else
						{
							ShadowIron += curItem.Amount;
							curItem.Delete();
							from.SendGump(new MetalWorkersKeyGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is CopperIngot)
					{
						if (Copper + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Copper + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
						else
						{
							Copper += curItem.Amount;
							curItem.Delete();
							from.SendGump(new MetalWorkersKeyGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is BronzeIngot)
					{

						if (Bronze + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Bronze + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
						else
						{
							Bronze += curItem.Amount;
							curItem.Delete();
							from.SendGump(new MetalWorkersKeyGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is GoldIngot)
					{
						if (Gold + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Gold + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
						else
						{
							Gold += curItem.Amount;
							curItem.Delete();
							from.SendGump(new MetalWorkersKeyGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is AgapiteIngot)
					{
						if (Agapite + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Agapite + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
						else
						{
							Agapite += curItem.Amount;
							curItem.Delete();
							from.SendGump(new MetalWorkersKeyGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is VeriteIngot)
					{

						if (Verite + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Verite + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
						else
						{
							Verite += curItem.Amount;
							curItem.Delete();
							from.SendGump(new MetalWorkersKeyGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is ValoriteIngot)
					{
						if (Valorite + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Valorite + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
						else
						{
							Valorite += curItem.Amount;
							curItem.Delete();
							from.SendGump(new MetalWorkersKeyGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
						// custom ores daat99 3/7
					else if (curItem is BlazeIngot)
					{
						if (Blaze + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Blaze + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
						else
						{
							Blaze += curItem.Amount;
							curItem.Delete();
							from.SendGump(new MetalWorkersKeyGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is IceIngot)
					{
						if (Ice + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Ice + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
						else
						{
							Ice += curItem.Amount;
							curItem.Delete();
							from.SendGump(new MetalWorkersKeyGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is ToxicIngot)
					{
						if (Toxic + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Toxic + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
						else
						{
							Toxic += curItem.Amount;
							curItem.Delete();
							from.SendGump(new MetalWorkersKeyGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is ElectrumIngot)
					{
						if (Electrum + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Electrum + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
						else
						{
							Electrum += curItem.Amount;
							curItem.Delete();
							from.SendGump(new MetalWorkersKeyGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is PlatinumIngot)
					{
						if (Platinum + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Platinum + curItem.Amount) - m_StorageLimit ) +" too much! The warehouse can store only "+ m_StorageLimit +" of this resource.");
						else
						{
							Platinum += curItem.Amount;
							curItem.Delete();
							from.SendGump(new MetalWorkersKeyGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					//
				}
			}
			else
			{
				from.SendLocalizedMessage(1045158); // You must have the item in your backpack to target it.
			}
		}
		public MetalWorkersKey(Serial serial) : base( serial )
		{
		}
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write( (int) 0 ); // version

			writer.Write((int)m_Iron);
			writer.Write((int)m_DullCopper);
			writer.Write((int)m_ShadowIron);
			writer.Write((int)m_Copper);
			writer.Write((int)m_Bronze);
			writer.Write((int)m_Gold);
			writer.Write((int)m_Agapite);
			writer.Write((int)m_Verite);
			writer.Write((int)m_Valorite);
			// custom ores daat99 4/7
			writer.Write((int)m_Blaze);
			writer.Write((int)m_Ice);
			writer.Write((int)m_Toxic);
			writer.Write((int)m_Electrum);
			writer.Write((int)m_Platinum);
			//
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
					m_Iron = reader.ReadInt();
					m_DullCopper = reader.ReadInt();
					m_ShadowIron = reader.ReadInt();
					m_Copper = reader.ReadInt();
					m_Bronze = reader.ReadInt();
					m_Gold = reader.ReadInt();
					m_Agapite = reader.ReadInt();
					m_Verite = reader.ReadInt();
					m_Valorite = reader.ReadInt();
					// custom ores daat99 5/7
					m_Blaze = reader.ReadInt();
					m_Ice = reader.ReadInt();
					m_Toxic = reader.ReadInt();
					m_Electrum = reader.ReadInt();
					m_Platinum = reader.ReadInt();
					//
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
    public class MetalWorkersKeyGump : Gump
    {
        private PlayerMobile m_From;
        private MetalWorkersKey m_Key;

        public MetalWorkersKeyGump(PlayerMobile from, MetalWorkersKey key) : base( 25, 25 )
        {
            m_From = from;
            m_Key = key;

            m_From.CloseGump(typeof(MetalWorkersKeyGump));

            AddPage(0);

            AddBackground(50, 10, 455, 260, 5054);
            AddImageTiled(58, 20, 438, 241, 2624);
            AddAlphaRegion(58, 20, 438, 241);
            
            AddLabel(200, 25, 88, "Metal Worker's Warehouse");

            AddLabel(125, 50, 0x486, "Iron");
            AddLabel(225, 50, 0x480, key.Iron.ToString());
            AddButton(75, 50, 4005, 4007, 1, GumpButtonType.Reply, 0);

            AddLabel(125, 75, 0x486, "Dull Copper");
            AddLabel(225, 75, 0x480, key.DullCopper.ToString());
            AddButton(75, 75, 4005, 4007, 2, GumpButtonType.Reply, 0);

            AddLabel(125, 100, 0x486, "Shadow Iron");
            AddLabel(225, 100, 0x480, key.ShadowIron.ToString());
            AddButton(75, 100, 4005, 4007, 3, GumpButtonType.Reply, 0);

            AddLabel(125, 125, 0x486, "Copper");
            AddLabel(225, 125, 0x480, key.Copper.ToString());
            AddButton(75, 125, 4005, 4007, 4, GumpButtonType.Reply, 0);

            AddLabel(125, 150, 0x486, "Bronze");
            AddLabel(225, 150, 0x480, key.Bronze.ToString());
            AddButton(75, 150, 4005, 4007, 5, GumpButtonType.Reply, 0);

            AddLabel(125, 175, 0x486, "Gold");
            AddLabel(225, 175, 0x480, key.Gold.ToString());
            AddButton(75, 175, 4005, 4007, 6, GumpButtonType.Reply, 0);

            AddLabel(125, 200, 0x486, "Agapite");
            AddLabel(225, 200, 0x480, key.Agapite.ToString());
            AddButton(75, 200, 4005, 4007, 7, GumpButtonType.Reply, 0);

            AddLabel(125, 225, 0x486, "Verite");
            AddLabel(225, 225, 0x480, key.Verite.ToString());
            AddButton(75, 225, 4005, 4007, 8, GumpButtonType.Reply, 0);

            AddLabel(325, 50, 0x486, "Valorite");
            AddLabel(425, 50, 0x480, key.Valorite.ToString());
            AddButton(275, 50, 4005, 4007, 9, GumpButtonType.Reply, 0);
// custom ores daat99 6/7
			AddLabel(325, 75, 0x486, "Blaze");
			AddLabel(425, 75, 0x480, key.Blaze.ToString());
			AddButton(275, 75, 4005, 4007, 10, GumpButtonType.Reply, 0);	

			AddLabel(325, 100, 0x486, "Ice");
			AddLabel(425, 100, 0x480, key.Ice.ToString());
			AddButton(275, 100, 4005, 4007, 11, GumpButtonType.Reply, 0);	

			AddLabel(325, 125, 0x486, "Toxic");
			AddLabel(425, 125, 0x480, key.Toxic.ToString());
			AddButton(275, 125, 4005, 4007, 12, GumpButtonType.Reply, 0);	

			AddLabel(325, 150, 0x486, "Electrum");
			AddLabel(425, 150, 0x480, key.Electrum.ToString());
			AddButton(275, 150, 4005, 4007, 13, GumpButtonType.Reply, 0);	

			AddLabel(325, 175, 0x486, "Platinum");
			AddLabel(425, 175, 0x480, key.Platinum.ToString());
			AddButton(275, 175, 4005, 4007, 14, GumpButtonType.Reply, 0);	
//			
			AddLabel(325, 200, 88, "Each Max:" );
			AddLabel(425, 200, 0x480, key.StorageLimit.ToString() );	

			AddLabel(325, 225, 88, "Add Metal");
			AddButton(275, 225, 4005, 4007, 15, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Key.Deleted)
                return;

            else if (info.ButtonID == 1)
            {
                if (m_Key.Iron > m_Key.WithdrawIncrement)								//if the key currently holds more ot this type than the increment amount
                {
                    m_From.AddToBackpack(new IronIngot(m_Key.WithdrawIncrement));  	//Send the increment amount of this type to players backpack
                    m_Key.Iron = m_Key.Iron - m_Key.WithdrawIncrement;				//removes that many from the keys count
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));					//Resets the gump with the new info
                }
                else if (m_Key.Iron > 0)
                {
                    m_From.AddToBackpack(new IronIngot(m_Key.Iron));  					//Sends all stored ingots of whichever type to players backpack
                    m_Key.Iron = 0;						     						//Sets the count in the key back to 0
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));					//Resets the gump with the new info
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Ingot!");			//Tell the player he is barking up the wrong tree
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));  				//Resets the gump 
                    m_Key.BeginCombine(m_From);										//Send the player a new in-game target in case more resources are to be added
                }
            }
            else if (info.ButtonID == 2)
            {
                if (m_Key.DullCopper > m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new DullCopperIngot(m_Key.WithdrawIncrement));
                    m_Key.DullCopper = m_Key.DullCopper - m_Key.WithdrawIncrement;
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
                }
                else if (m_Key.DullCopper > 0)
                {
                    m_From.AddToBackpack(new DullCopperIngot(m_Key.DullCopper));  	 
                    m_Key.DullCopper = 0;						     	 
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Ingot!");
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
                    m_Key.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 3)
            {
                if (m_Key.ShadowIron > m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new ShadowIronIngot(m_Key.WithdrawIncrement));
                    m_Key.ShadowIron = m_Key.ShadowIron - m_Key.WithdrawIncrement;
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
                }
                else if (m_Key.ShadowIron > 0)
                {
                    m_From.AddToBackpack(new ShadowIronIngot(m_Key.ShadowIron));  	 
                    m_Key.ShadowIron = 0;						     	  
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Ingot!");
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
                    m_Key.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 4)
            {
                if (m_Key.Copper > m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new CopperIngot(m_Key.WithdrawIncrement));
                    m_Key.Copper = m_Key.Copper - m_Key.WithdrawIncrement;
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
                }
                else if (m_Key.Copper > 0)
                {
                    m_From.AddToBackpack(new CopperIngot(m_Key.Copper));  	  
                    m_Key.Copper = 0;						     	  
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Ingot!");
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
                    m_Key.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 5)
            {
                if (m_Key.Bronze > m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new BronzeIngot(m_Key.WithdrawIncrement));
                    m_Key.Bronze = m_Key.Bronze - m_Key.WithdrawIncrement;
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
                }
                else if (m_Key.Bronze > 0)
                {
                    m_From.AddToBackpack(new BronzeIngot(m_Key.Bronze));  	  
                    m_Key.Bronze = 0;						     	  
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Ingot!");
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
                    m_Key.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 6)
            {
                if (m_Key.Gold > m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new GoldIngot(m_Key.WithdrawIncrement));
                    m_Key.Gold = m_Key.Gold - m_Key.WithdrawIncrement;
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
                }
                else if (m_Key.Gold > 0)
                {
                    m_From.AddToBackpack(new GoldIngot(m_Key.Gold));  	  
                    m_Key.Gold = 0;						     	 
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Ingot!");
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
                    m_Key.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 7)
            {
                if (m_Key.Agapite > m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new AgapiteIngot(m_Key.WithdrawIncrement));
                    m_Key.Agapite = m_Key.Agapite - m_Key.WithdrawIncrement;
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
                }
                else if (m_Key.Agapite > 0)
                {
                    m_From.AddToBackpack(new AgapiteIngot(m_Key.Agapite));  
                    m_Key.Agapite = 0;						     	  
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Ingot!");
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
                    m_Key.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 8)
            {
                if (m_Key.Verite > m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new VeriteIngot(m_Key.WithdrawIncrement));
                    m_Key.Verite = m_Key.Verite - m_Key.WithdrawIncrement;
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
                }
                else if (m_Key.Verite > 0)
                {
                    m_From.AddToBackpack(new VeriteIngot(m_Key.Verite));  	
                    m_Key.Verite = 0;						     	 
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Ingot!");
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
                    m_Key.BeginCombine(m_From);
                }
            }
            else if (info.ButtonID == 9)
            {
                if (m_Key.Valorite > m_Key.WithdrawIncrement)
                {
                    m_From.AddToBackpack(new ValoriteIngot(m_Key.WithdrawIncrement));
                    m_Key.Valorite = m_Key.Valorite - m_Key.WithdrawIncrement;
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
                }
                else if (m_Key.Valorite > 0)
                {
                    m_From.AddToBackpack(new ValoriteIngot(m_Key.Valorite));  	
                    m_Key.Valorite = 0;						     	  
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key)); 
                }
                else
                {
                    m_From.SendMessage("You do not have any of that Ingot!");
                    m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
                    m_Key.BeginCombine(m_From);
                }
            }
// custom ores daat99 7/7
			else if (info.ButtonID == 10)
			{
				if (m_Key.Blaze > m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new BlazeIngot(m_Key.WithdrawIncrement));
					m_Key.Blaze = m_Key.Blaze - m_Key.WithdrawIncrement;
					m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
				}
				else if (m_Key.Blaze > 0)
				{
					m_From.AddToBackpack(new BlazeIngot(m_Key.Blaze));  	
					m_Key.Blaze = 0;						     	  
					m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key)); 
				}
				else
				{
					m_From.SendMessage("You do not have any of that Ingot!");
					m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
					m_Key.BeginCombine(m_From);
				}
			}
			else if (info.ButtonID == 11)
			{
				if (m_Key.Ice > m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new IceIngot(m_Key.WithdrawIncrement));
					m_Key.Ice = m_Key.Ice - m_Key.WithdrawIncrement;
					m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
				}
				else if (m_Key.Ice > 0)
				{
					m_From.AddToBackpack(new IceIngot(m_Key.Ice));  	
					m_Key.Ice = 0;						     	  
					m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key)); 
				}
				else
				{
					m_From.SendMessage("You do not have any of that Ingot!");
					m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
					m_Key.BeginCombine(m_From);
				}
			}
			else if (info.ButtonID == 12)
			{
				if (m_Key.Toxic > m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new ToxicIngot(m_Key.WithdrawIncrement));
					m_Key.Toxic = m_Key.Toxic - m_Key.WithdrawIncrement;
					m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
				}
				else if (m_Key.Toxic > 0)
				{
					m_From.AddToBackpack(new ToxicIngot(m_Key.Toxic));  	
					m_Key.Toxic = 0;						     	  
					m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key)); 
				}
				else
				{
					m_From.SendMessage("You do not have any of that Ingot!");
					m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
					m_Key.BeginCombine(m_From);
				}
			}
			else if (info.ButtonID == 13)
			{
				if (m_Key.Electrum > m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new ElectrumIngot(m_Key.WithdrawIncrement));
					m_Key.Electrum = m_Key.Electrum - m_Key.WithdrawIncrement;
					m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
				}
				else if (m_Key.Electrum > 0)
				{
					m_From.AddToBackpack(new ElectrumIngot(m_Key.Electrum));  	
					m_Key.Electrum = 0;						     	  
					m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key)); 
				}
				else
				{
					m_From.SendMessage("You do not have any of that Ingot!");
					m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
					m_Key.BeginCombine(m_From);
				}
			}
			else if (info.ButtonID == 14)
			{
				if (m_Key.Platinum > m_Key.WithdrawIncrement)
				{
					m_From.AddToBackpack(new PlatinumIngot(m_Key.WithdrawIncrement));
					m_Key.Platinum = m_Key.Platinum - m_Key.WithdrawIncrement;
					m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
				}
				else if (m_Key.Platinum > 0)
				{
					m_From.AddToBackpack(new PlatinumIngot(m_Key.Platinum));  	
					m_Key.Platinum = 0;						     	  
					m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key)); 
				}
				else
				{
					m_From.SendMessage("You do not have any of that Ingot!");
					m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
					m_Key.BeginCombine(m_From);
				}
			}
//
			else if (info.ButtonID == 15)
            {
                m_From.SendGump(new MetalWorkersKeyGump(m_From, m_Key));
                m_Key.BeginCombine(m_From);
            }
        }
    }
}

namespace Server.Items
{
    public class MetalWorkersKeyTarget : Target
    {
        private MetalWorkersKey m_Key;

        public MetalWorkersKeyTarget(MetalWorkersKey key) : base( 18, false, TargetFlags.None )
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
