


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
	
    public class GemPouch : BaseContainer
    {
	
        
		public override bool DisplaysContent{ get{ return false; } }
		
        private int m_Diamond;
        private int m_Sapphire;
        private int m_Ruby;
        private int m_StarSapphire;
        private int m_Emerald;
        private int m_Citrine;
        private int m_Amethyst;
        private int m_Tourmaline;
        private int m_Amber;
		// Mondain's Legacy Gems 1/7
		private int m_BlueDiamond;
		private int m_BrilliantAmber;
		private int m_DarkSapphire;
		private int m_EcruCitrine;
		private int m_FireRuby;
		private int m_PerfectEmerald;
		private int m_Turquoise;
		private int m_WhitePearl;
		//
		
		private string msg_Full = "";

		private int m_StorageLimit;
        private int m_WithdrawIncrement;

        [CommandProperty(AccessLevel.GameMaster)]
        public int StorageLimit { get { return m_StorageLimit; } set { m_StorageLimit = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int WithdrawIncrement { get { return m_WithdrawIncrement; } set { m_WithdrawIncrement = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Diamond { get { return m_Diamond; } set { m_Diamond = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Sapphire { get { return m_Sapphire; } set { m_Sapphire = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Ruby { get { return m_Ruby; } set { m_Ruby = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int StarSapphire { get { return m_StarSapphire; } set { m_StarSapphire = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Emerald { get { return m_Emerald; } set { m_Emerald = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Citrine { get { return m_Citrine; } set { m_Citrine = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Amethyst { get { return m_Amethyst; } set { m_Amethyst = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Tourmaline { get { return m_Tourmaline; } set { m_Tourmaline = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Amber { get { return m_Amber; } set { m_Amber = value; InvalidateProperties(); } }
		
		// Mondain's Legacy Gems 2/7
		[CommandProperty(AccessLevel.GameMaster)]
		public int BlueDiamond { get { return m_BlueDiamond; } set { m_BlueDiamond = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int BrilliantAmber { get { return m_BrilliantAmber; } set { m_BrilliantAmber = value; InvalidateProperties(); } }
		
		[CommandProperty(AccessLevel.GameMaster)]
		public int DarkSapphire { get { return m_DarkSapphire; } set { m_DarkSapphire = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int EcruCitrine { get { return m_EcruCitrine; } set { m_EcruCitrine = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int FireRuby { get { return m_FireRuby; } set { m_FireRuby = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int PerfectEmerald { get { return m_PerfectEmerald; } set { m_PerfectEmerald = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int Turquoise { get { return m_Turquoise; } set { m_Turquoise = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public int WhitePearl { get { return m_WhitePearl; } set { m_WhitePearl = value; InvalidateProperties(); } }
		
		
		//



        [Constructable]
        public GemPouch() : base( 3705 )
        {
            Movable = true;
            Weight = 1.0;
			Hue = 1974;//2165;
            Name = "Gem Pouch";
            StorageLimit = 60000;
            WithdrawIncrement = 25;
        }

        [Constructable]
        public GemPouch(int storageLimit, int withdrawIncrement) : base( 3705 )
        {
            Movable = true;
            Weight = 10.0;
            Hue = 1974;//2165;
            Name = "Gem Pouch";
            StorageLimit = storageLimit;
            WithdrawIncrement = withdrawIncrement;
        }
		public override int DefaultDropSound{ get{ return 52; } }
		public override bool OnDragDrop( Mobile from, Item dropped )  
		{ 
			if ( dropped.Deleted ) return false;
			
			if ( dropped is Diamond )		
			{		
				int freespace = 60000 - this.Diamond;
				if ( freespace == 0 ) from.SendMessage("You're Diamond limit has been reached, and excess has fallen out!");
				else if (freespace >= dropped.Amount )
				{this.Diamond += dropped.Amount;dropped.Delete();}
				else{this.Diamond += freespace;dropped.Amount -= freespace;}
				from.SendSound( GetDroppedSound( dropped ), GetWorldLocation() );
								
			}
			else if ( dropped is Sapphire )		
			{		
				int freespace = 60000 - this.Sapphire;
				if ( freespace == 0 ) from.SendMessage("You're Sapphire limit has been reached, and excess has fallen out!");
				else if (freespace >= dropped.Amount )
				{this.Sapphire += dropped.Amount;dropped.Delete();}
				else{this.Sapphire += freespace;dropped.Amount -= freespace;}
				from.SendSound( GetDroppedSound( dropped ), GetWorldLocation() );
				
			}
			else if ( dropped is Ruby )		
			{
				int freespace = 60000 - this.Ruby;
				if ( freespace == 0 ) from.SendMessage("You're Ruby limit has been reached, and excess has fallen out!");
				else if (freespace >= dropped.Amount )
				{this.Ruby += dropped.Amount;dropped.Delete();}
				else{this.Ruby += freespace;dropped.Amount -= freespace;}
				from.SendSound( GetDroppedSound( dropped ), GetWorldLocation() );
			}
			else if ( dropped is StarSapphire )		
			{		
				int freespace = 60000 - this.StarSapphire;
				if ( freespace == 0 ) from.SendMessage("You're Star Sapphire limit has been reached, and excess has fallen out!");
				else if (freespace >= dropped.Amount )
				{this.StarSapphire += dropped.Amount;dropped.Delete();}
				else{this.StarSapphire += freespace;dropped.Amount -= freespace;}
				from.SendSound( GetDroppedSound( dropped ), GetWorldLocation() );
			}
			else if ( dropped is Emerald )		
			{		
				int freespace = 60000 - this.Emerald;
				if ( freespace == 0 ) from.SendMessage("You're Emerald limit has been reached, and excess has fallen out!");
				else if (freespace >= dropped.Amount )
				{this.Emerald += dropped.Amount;dropped.Delete();}
				else{this.Emerald += freespace;dropped.Amount -= freespace;}
				from.SendSound( GetDroppedSound( dropped ), GetWorldLocation() );
			}
			else if ( dropped is Citrine )		
			{		
				int freespace = 60000 - this.Citrine;
				if ( freespace == 0 ) from.SendMessage("You're Citrine limit has been reached, and excess has fallen out!");
				else if (freespace >= dropped.Amount )
				{this.Citrine += dropped.Amount;dropped.Delete();}
				else{this.Citrine += freespace;dropped.Amount -= freespace;}
				from.SendSound( GetDroppedSound( dropped ), GetWorldLocation() );
			}
			else if ( dropped is Amethyst )		
			{		
				int freespace = 60000 - this.Amethyst;
				if ( freespace == 0 ) from.SendMessage("You're Amethyst limit has been reached, and excess has fallen out!");
				else if (freespace >= dropped.Amount )
				{this.Amethyst += dropped.Amount;dropped.Delete();}
				else{this.Amethyst += freespace;dropped.Amount -= freespace;}
				from.SendSound( GetDroppedSound( dropped ), GetWorldLocation() );
			}
			else if ( dropped is Tourmaline )		
			{		
				int freespace = 60000 - this.Tourmaline;
				if ( freespace == 0 ) from.SendMessage("You're Tourmaline limit has been reached, and excess has fallen out!");
				else if (freespace >= dropped.Amount )
				{this.Tourmaline += dropped.Amount;dropped.Delete();}
				else{this.Tourmaline += freespace;dropped.Amount -= freespace;}
				from.SendSound( GetDroppedSound( dropped ), GetWorldLocation() );
			}
			else if ( dropped is Amber )		
			{		
				int freespace = 60000 - this.Amber;
				if ( freespace == 0 ) from.SendMessage("You're Amber limit has been reached, and excess has fallen out!");
				else if (freespace >= dropped.Amount )
				{this.Amber += dropped.Amount;dropped.Delete();}
				else{this.Amber += freespace;dropped.Amount -= freespace;}
				from.SendSound( GetDroppedSound( dropped ), GetWorldLocation() );
			}
			//ML Gems
			else if ( dropped is BlueDiamond )		
			{		
				int freespace = 60000 - this.BlueDiamond;
				if ( freespace == 0 ) from.SendMessage("You're Blue Diamond limit has been reached, and excess has fallen out!");
				else if (freespace >= dropped.Amount )
				{this.BlueDiamond += dropped.Amount;dropped.Delete();}
				else{this.BlueDiamond += freespace;dropped.Amount -= freespace;}
				from.SendSound( GetDroppedSound( dropped ), GetWorldLocation() );
			}
			else if ( dropped is BrilliantAmber )		
			{		
				int freespace = 60000 - this.BrilliantAmber;
				if ( freespace == 0 ) from.SendMessage("You're BrilliantAmber limit has been reached, and excess has fallen out!");
				else if (freespace >= dropped.Amount )
				{this.BrilliantAmber += dropped.Amount;dropped.Delete();}
				else{this.BrilliantAmber += freespace;dropped.Amount -= freespace;}
				from.SendSound( GetDroppedSound( dropped ), GetWorldLocation() );
			}
			else if ( dropped is DarkSapphire )		
			{		
				int freespace = 60000 - this.DarkSapphire;
				if ( freespace == 0 ) from.SendMessage("You're Dark Sapphire limit has been reached, and excess has fallen out!");
				else if (freespace >= dropped.Amount )
				{this.DarkSapphire += dropped.Amount;dropped.Delete();}
				else{this.DarkSapphire += freespace;dropped.Amount -= freespace;}
				from.SendSound( GetDroppedSound( dropped ), GetWorldLocation() );
			}
			else if ( dropped is EcruCitrine )		
			{		
				int freespace = 60000 - this.EcruCitrine;
				if ( freespace == 0 ) from.SendMessage("You're Ecru Citrine limit has been reached, and excess has fallen out!");
				else if (freespace >= dropped.Amount )
				{this.EcruCitrine += dropped.Amount;dropped.Delete();}
				else{this.EcruCitrine += freespace;dropped.Amount -= freespace;}
				from.SendSound( GetDroppedSound( dropped ), GetWorldLocation() );
			}
			else if ( dropped is FireRuby )		
			{		
				int freespace = 60000 - this.FireRuby;
				if ( freespace == 0 ) from.SendMessage("You're Fire Ruby limit has been reached, and excess has fallen out!");
				else if (freespace >= dropped.Amount )
				{this.FireRuby += dropped.Amount;dropped.Delete();}
				else{this.FireRuby += freespace;dropped.Amount -= freespace;}
				from.SendSound( GetDroppedSound( dropped ), GetWorldLocation() );
			}
			else if ( dropped is PerfectEmerald )		
			{		
				int freespace = 60000 - this.PerfectEmerald;
				if ( freespace == 0 ) from.SendMessage("You're Perfect Emerald limit has been reached, and excess has fallen out!");
				else if (freespace >= dropped.Amount )
				{this.PerfectEmerald += dropped.Amount;dropped.Delete();}
				else{this.PerfectEmerald += freespace;dropped.Amount -= freespace;}
				from.SendSound( GetDroppedSound( dropped ), GetWorldLocation() );
			}
			else if ( dropped is Turquoise )		
			{		
				int freespace = 60000 - this.Turquoise;
				if ( freespace == 0 ) from.SendMessage("You're Turquoise limit has been reached, and excess has fallen out!");
				else if (freespace >= dropped.Amount )
				{this.Turquoise += dropped.Amount;dropped.Delete();}
				else{this.Turquoise += freespace;dropped.Amount -= freespace;}
				from.SendSound( GetDroppedSound( dropped ), GetWorldLocation() );
			}
			else if ( dropped is WhitePearl )		
			{		
				int freespace = 60000 - this.WhitePearl;
				if ( freespace == 0 ) from.SendMessage("You're White Pearl limit has been reached, and excess has fallen out!");
				else if (freespace >= dropped.Amount )
				{this.WhitePearl += dropped.Amount;dropped.Delete();}
				else{this.WhitePearl += freespace;dropped.Amount -= freespace;}
				from.SendSound( GetDroppedSound( dropped ), GetWorldLocation() );				
			}//
			return false;
		}	
		public override void OnDoubleClick(Mobile from)
        {

			if (!(from is PlayerMobile))
				return;
			if (IsChildOf(from.Backpack))
				from.SendGump(new GemPouchGump((PlayerMobile)from, this));
			else
				from.SendMessage("This must be in your backpack.");
        }
        public void BeginCombine(Mobile from)
        {
            from.Target = new GemPouchTarget(this);
        }
        public void EndCombine(Mobile from, object o)
        {
			if (o is Item && (((Item)o).IsChildOf(from.Backpack) || ((Item)o).IsChildOf(from.BankBox)))
            {
				Item curItem = o as Item;
				if (curItem is Item)
				{
					if (curItem is Amber)
					{
						if (Amber + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Amber + curItem.Amount) - m_StorageLimit ) +" too much! The pouch can store only "+ m_StorageLimit +" of this gem.");
						else
						{
							Amber += curItem.Amount;
							curItem.Delete();
							from.SendGump(new GemPouchGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is Amethyst)
					{
						if (Amethyst + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Amethyst + curItem.Amount) - m_StorageLimit ) +" too much! The pouch can store only "+ m_StorageLimit +" of this gem.");
						else
						{
							Amethyst += curItem.Amount;
							curItem.Delete();
							from.SendGump(new GemPouchGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is Citrine)
					{
						if (Citrine + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Citrine + curItem.Amount) - m_StorageLimit ) +" too much! The pouch can store only "+ m_StorageLimit +" of this gem.");
						else
						{
							Citrine += curItem.Amount;
							curItem.Delete();
							from.SendGump(new GemPouchGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is Diamond)
					{
						if (Diamond + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Diamond + curItem.Amount) - m_StorageLimit ) +" too much! The pouch can store only "+ m_StorageLimit +" of this gem.");
						else
						{
							Diamond += curItem.Amount;
							curItem.Delete();
							from.SendGump(new GemPouchGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is Emerald)
					{

						if (Emerald + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Emerald + curItem.Amount) - m_StorageLimit ) +" too much! The pouch can store only "+ m_StorageLimit +" of this gem.");
						else
						{
							Emerald += curItem.Amount;
							curItem.Delete();
							from.SendGump(new GemPouchGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is Ruby)
					{
						if (Ruby + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Ruby + curItem.Amount) - m_StorageLimit ) +" too much! The pouch can store only "+ m_StorageLimit +" of this gem.");
						else
						{
							Ruby += curItem.Amount;
							curItem.Delete();
							from.SendGump(new GemPouchGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is Sapphire)
					{
						if (Sapphire + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Sapphire + curItem.Amount) - m_StorageLimit ) +" too much! The pouch can store only "+ m_StorageLimit +" of this gem.");
						else
						{
							Sapphire += curItem.Amount;
							curItem.Delete();
							from.SendGump(new GemPouchGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is StarSapphire)
					{
						if (StarSapphire + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (StarSapphire + curItem.Amount) - m_StorageLimit ) +" too much! The pouch can store only "+ m_StorageLimit +" of this gem.");
						else
						{
							StarSapphire += curItem.Amount;
							curItem.Delete();
							from.SendGump(new GemPouchGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is Tourmaline)
					{

						if (Tourmaline + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Tourmaline + curItem.Amount) - m_StorageLimit ) +" too much! The pouch can store only "+ m_StorageLimit +" of this gem.");
						else
						{
							Tourmaline += curItem.Amount;
							curItem.Delete();
							from.SendGump(new GemPouchGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
						// ML Gems
					else if (curItem is BlueDiamond)
					{
						if (BlueDiamond + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (BlueDiamond + curItem.Amount) - m_StorageLimit ) +" too much! The pouch can store only "+ m_StorageLimit +" of this gem.");
						else
						{
							BlueDiamond += curItem.Amount;
							curItem.Delete();
							from.SendGump(new GemPouchGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is BrilliantAmber)
					{
						if (BrilliantAmber + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (BrilliantAmber + curItem.Amount) - m_StorageLimit ) +" too much! The pouch can store only "+ m_StorageLimit +" of this gem.");
						else
						{
							BrilliantAmber += curItem.Amount;
							curItem.Delete();
							from.SendGump(new GemPouchGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is DarkSapphire)
					{
						if (DarkSapphire + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (DarkSapphire + curItem.Amount) - m_StorageLimit ) +" too much! The pouch can store only "+ m_StorageLimit +" of this gem.");
						else
						{
							DarkSapphire += curItem.Amount;
							curItem.Delete();
							from.SendGump(new GemPouchGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is EcruCitrine)
					{
						if (EcruCitrine + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (EcruCitrine + curItem.Amount) - m_StorageLimit ) +" too much! The pouch can store only "+ m_StorageLimit +" of this gem.");
						else
						{
							EcruCitrine += curItem.Amount;
							curItem.Delete();
							from.SendGump(new GemPouchGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is FireRuby)
					{
						if (FireRuby + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (FireRuby + curItem.Amount) - m_StorageLimit ) +" too much! The pouch can store only "+ m_StorageLimit +" of this gem.");
						else
						{
							FireRuby += curItem.Amount;
							curItem.Delete();
							from.SendGump(new GemPouchGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is PerfectEmerald)
					{
						if (PerfectEmerald + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (PerfectEmerald + curItem.Amount) - m_StorageLimit ) +" too much! The pouch can store only "+ m_StorageLimit +" of this gem.");
						else
						{
							PerfectEmerald += curItem.Amount;
							curItem.Delete();
							from.SendGump(new GemPouchGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is Turquoise)
					{
						if (Turquoise + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (Turquoise + curItem.Amount) - m_StorageLimit ) +" too much! The pouch can store only "+ m_StorageLimit +" of this gem.");
						else
						{
							Turquoise += curItem.Amount;
							curItem.Delete();
							from.SendGump(new GemPouchGump((PlayerMobile)from, this));
							BeginCombine(from);
						}
					}
					else if (curItem is WhitePearl)
					{
						if (WhitePearl + curItem.Amount > StorageLimit)
							from.SendMessage("You are trying to add "+ ( (WhitePearl + curItem.Amount) - m_StorageLimit ) +" too much! The pouch can store only "+ m_StorageLimit +" of this gem.");
						else
						{
							WhitePearl += curItem.Amount;
							curItem.Delete();
							from.SendGump(new GemPouchGump((PlayerMobile)from, this));
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
		
        public GemPouch(Serial serial) : base( serial )
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
			writer.Write( (int) 0 ); // version

            writer.Write((int)m_Diamond);
            writer.Write((int)m_Sapphire);
            writer.Write((int)m_Ruby);
            writer.Write((int)m_StarSapphire);
            writer.Write((int)m_Emerald);
            writer.Write((int)m_Citrine);
            writer.Write((int)m_Amethyst);
            writer.Write((int)m_Tourmaline);
            writer.Write((int)m_Amber);
			// ML Gems
			writer.Write((int)m_BlueDiamond);
			writer.Write((int)m_BrilliantAmber);
			writer.Write((int)m_DarkSapphire);
			writer.Write((int)m_EcruCitrine);
			writer.Write((int)m_FireRuby);
			writer.Write((int)m_PerfectEmerald);
			writer.Write((int)m_Turquoise);
			writer.Write((int)m_WhitePearl);
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
					m_Diamond = reader.ReadInt();
					m_Sapphire = reader.ReadInt();
					m_Ruby = reader.ReadInt();
					m_StarSapphire = reader.ReadInt();
					m_Emerald = reader.ReadInt();
					m_Citrine = reader.ReadInt();
					m_Amethyst = reader.ReadInt();
					m_Tourmaline = reader.ReadInt();
					m_Amber = reader.ReadInt();
					// ML Gems
					m_BlueDiamond = reader.ReadInt();
					m_BrilliantAmber = reader.ReadInt();
					m_DarkSapphire = reader.ReadInt();
					m_EcruCitrine = reader.ReadInt();
					m_FireRuby = reader.ReadInt();
					m_PerfectEmerald = reader.ReadInt();
					m_Turquoise = reader.ReadInt();
					m_WhitePearl = reader.ReadInt();
					//					
					m_StorageLimit = reader.ReadInt();
					m_WithdrawIncrement = reader.ReadInt();
					break;
				}
			}
        }
    }

	public class GemPouchGump : Gump
    {
        private PlayerMobile m_From;
        private GemPouch m_Key;

        public GemPouchGump(PlayerMobile from, GemPouch key) : base( 25, 25 )
        {
            m_From = from;
            m_Key = key;

            m_From.CloseGump(typeof(GemPouchGump));					
			
            AddPage(0);
			
            AddBackground(50, 10, 455, 325, 5054);
            AddImageTiled(58, 20, 438, 306, 2624);
            //AddAlphaRegion(58, 20, 438, 241);

            AddLabel(70, 25, 100, "((�*イ�*イ�*イ�*イ Gem Pouch ㅋ*イ�*イ�*イ�*�))");

            AddLabel(125, 50, 16, "Diamond");
            AddLabel(225, 50, 0x480, key.Diamond.ToString());
            AddButton(75, 50, 4005, 4007, 1, GumpButtonType.Reply, 0);

            AddLabel(125, 75, 16, "Sapphire");
            AddLabel(225, 75, 0x480, key.Sapphire.ToString());
            AddButton(75, 75, 4005, 4007, 2, GumpButtonType.Reply, 0);

            AddLabel(125, 100, 16, "Ruby");
            AddLabel(225, 100, 0x480, key.Ruby.ToString());
            AddButton(75, 100, 4005, 4007, 3, GumpButtonType.Reply, 0);

            AddLabel(125, 125, 16, "StarSapphire");
            AddLabel(225, 125, 0x480, key.StarSapphire.ToString());
            AddButton(75, 125, 4005, 4007, 4, GumpButtonType.Reply, 0);

            AddLabel(125, 150, 16, "Emerald");
            AddLabel(225, 150, 0x480, key.Emerald.ToString());
            AddButton(75, 150, 4005, 4007, 5, GumpButtonType.Reply, 0);

            AddLabel(125, 175, 16, "Citrine");
            AddLabel(225, 175, 0x480, key.Citrine.ToString());
            AddButton(75, 175, 4005, 4007, 6, GumpButtonType.Reply, 0);

            AddLabel(125, 200, 16, "Amethyst");
            AddLabel(225, 200, 0x480, key.Amethyst.ToString());
            AddButton(75, 200, 4005, 4007, 7, GumpButtonType.Reply, 0);

            AddLabel(125, 225, 16, "Tourmaline");
            AddLabel(225, 225, 0x480, key.Tourmaline.ToString());
            AddButton(75, 225, 4005, 4007, 8, GumpButtonType.Reply, 0);

            AddLabel(125, 250, 16, "Amber");
            AddLabel(225, 250, 0x480, key.Amber.ToString());
            AddButton(75, 250, 4005, 4007, 9, GumpButtonType.Reply, 0);
			// ML Gems
			AddLabel(125, 275, 16, "Blue Diamond");
			AddLabel(225, 275, 0x480, key.BlueDiamond.ToString());
			AddButton(75, 275, 4005, 4007, 10, GumpButtonType.Reply, 0);
	
			AddLabel(325, 50, 16, "Brilliant Amber");
			AddLabel(440, 50, 0x480, key.BrilliantAmber.ToString());
			AddButton(275, 50, 4005, 4007, 11, GumpButtonType.Reply, 0);

			AddLabel(325, 75, 16, "Dark Sapphire");
			AddLabel(440, 75, 0x480, key.DarkSapphire.ToString());
			AddButton(275, 75, 4005, 4007, 12, GumpButtonType.Reply, 0);	

			AddLabel(325, 100, 16, "Ecru Citrine");
			AddLabel(440, 100, 0x480, key.EcruCitrine.ToString());
			AddButton(275, 100, 4005, 4007, 13, GumpButtonType.Reply, 0);	

			AddLabel(325, 125, 16, "Fire Ruby");
			AddLabel(440, 125, 0x480, key.FireRuby.ToString());
			AddButton(275, 125, 4005, 4007, 14, GumpButtonType.Reply, 0);	

			AddLabel(325, 150, 16, "Perfect Emerald");
			AddLabel(440, 150, 0x480, key.PerfectEmerald.ToString());
			AddButton(275, 150, 4005, 4007, 15, GumpButtonType.Reply, 0);
	
			AddLabel(325, 175, 16, "Turquoise");
			AddLabel(440, 175, 0x480, key.Turquoise.ToString());
			AddButton(275, 175, 4005, 4007, 16, GumpButtonType.Reply, 0);

			AddLabel(325, 200, 16, "White Pearl");
			AddLabel(440, 200, 0x480, key.WhitePearl.ToString());
			AddButton(275, 200, 4005, 4007, 17, GumpButtonType.Reply, 0);
			//

			AddLabel(325, 250, 100, "Each Max:" );
			AddLabel(440, 250, 0x480, key.StorageLimit.ToString() );	

            AddLabel(325, 275, 100, "Add Gems");
            AddButton(275, 275, 4005, 4007, 18, GumpButtonType.Reply, 0);

			

        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
			if (m_Key.Deleted)
				return;
			

			else if (info.ButtonID == 1)
			{
                if (m_Key.Diamond > m_Key.WithdrawIncrement)								//if the pouch holds more then the withdrawal amount
                {
                    m_From.AddToBackpack(new Diamond(m_Key.WithdrawIncrement));  				//gives the withdrawal amount to players via their backpack
           			m_Key.Diamond = m_Key.Diamond - m_Key.WithdrawIncrement;				//removes that many from the pouch
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));						//rereshes the gump showing the new balance
					m_From.SendMessage("You remove 25 Diamonds");
                }
                else if (m_Key.Diamond > 0)
                {
                    m_From.AddToBackpack(new Diamond(m_Key.Diamond));  					//sends all the ramaining gems to the players backpack
                    m_From.SendMessage("You remove the last of your Diamonds");
					m_Key.Diamond = 0;						     						//sets the count in the the pouch for that gem back to 0
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					//rereshes the gump showing the new balance
                }
                else
                {
                    m_From.SendMessage("You do not have any of these gems!");			//tells the player that they dont have any of this gem 
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));  				//refreshes the gump 
                    									
                }
            }
			else if (info.ButtonID == 2)
			{
                if (m_Key.Sapphire > m_Key.WithdrawIncrement)								//if the pouch holds more then the withdrawal amount
                {
                    m_From.AddToBackpack(new Sapphire(m_Key.WithdrawIncrement));  	//gives the withdrawal amount to players via their backpack
 					m_Key.Sapphire = m_Key.Sapphire - m_Key.WithdrawIncrement;				//removes that many from the pouch
					m_From.SendGump(new GemPouchGump(m_From, m_Key));					//rereshes the gump showing the new balance
					m_From.SendMessage("You remove 25 Sapphires");
                }
                else if (m_Key.Sapphire > 0)
                {
                    m_From.AddToBackpack(new Sapphire(m_Key.Sapphire));  					//sends all the ramaining gems to the players backpack
                    m_From.SendMessage("You remove the last of your Sapphires");
					m_Key.Sapphire = 0;						     						//sets the count in the the pouch for that gem back to 0
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					//rereshes the gump showing the new balance
                }
                else
                {
                    m_From.SendMessage("You do not have any of these gems!");			//tells the player that they dont have any of this gem 
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));  				//Resets the gump 
																						
                }
            }
			else if (info.ButtonID == 3)
			{
                if (m_Key.Ruby > m_Key.WithdrawIncrement)							
                {
                    m_From.AddToBackpack(new Ruby(m_Key.WithdrawIncrement));  	
					m_Key.Ruby = m_Key.Ruby - m_Key.WithdrawIncrement;				
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
					m_From.SendMessage("You remove 25 Rubies");
                }
                else if (m_Key.Ruby > 0)
                {
                    m_From.AddToBackpack(new Ruby(m_Key.Ruby));  					
                    m_From.SendMessage("You remove the last of your Rubies");
					m_Key.Ruby = 0;						     						
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
                }
                else
                {
                    m_From.SendMessage("You do not have any of these gems!");			
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));  				
                }
            }
			else if (info.ButtonID == 4)
			{
                if (m_Key.StarSapphire > m_Key.WithdrawIncrement)								
                {
                    m_From.AddToBackpack(new StarSapphire(m_Key.WithdrawIncrement));  	
					m_Key.StarSapphire = m_Key.StarSapphire - m_Key.WithdrawIncrement;				
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
					m_From.SendMessage("You remove 25 Star Sapphires");
                }
                else if (m_Key.StarSapphire > 0)
                {
                    m_From.AddToBackpack(new StarSapphire(m_Key.StarSapphire));  					
                    m_From.SendMessage("You remove the last of your Star Sapphires");
					m_Key.StarSapphire = 0;						     						
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
                }
                else
                {
                    m_From.SendMessage("You do not have any of these gems!");			
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));  				
                 
                }
            }
			else if (info.ButtonID == 5)
			{
                if (m_Key.Emerald > m_Key.WithdrawIncrement)								
                {
                    m_From.AddToBackpack(new Emerald(m_Key.WithdrawIncrement));  	
					m_Key.Emerald = m_Key.Emerald - m_Key.WithdrawIncrement;				
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
					m_From.SendMessage("You remove 25 Emeralds");
                }
                else if (m_Key.Emerald > 0)
                {
                    m_From.AddToBackpack(new Emerald(m_Key.Emerald));  					
                    m_From.SendMessage("You remove the last of your Emeralds");
					m_Key.Emerald = 0;						     						
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
                }
                else
                {
                    m_From.SendMessage("You do not have any of these gems!");			
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));  				
       
                }
            }
			else if (info.ButtonID == 6)
            {    
				if (m_Key.Citrine > m_Key.WithdrawIncrement)								
                {
                    m_From.AddToBackpack(new Citrine(m_Key.WithdrawIncrement));  	
					m_Key.Citrine = m_Key.Citrine - m_Key.WithdrawIncrement;				
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
					m_From.SendMessage("You remove 25 Citrines");
                }
                else if (m_Key.Citrine > 0)
                {
                    m_From.AddToBackpack(new Citrine(m_Key.Citrine));  					
                    m_From.SendMessage("You remove the last of your Citrines");
					m_Key.Citrine = 0;						     						
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
                }
                else
                {
                    m_From.SendMessage("You do not have any of these gems!");			
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));  				
              
                }
            }
			else if (info.ButtonID == 7)
			{
                if (m_Key.Amethyst > m_Key.WithdrawIncrement)								
                {
                    m_From.AddToBackpack(new Amethyst(m_Key.WithdrawIncrement));  	
					m_Key.Amethyst = m_Key.Amethyst - m_Key.WithdrawIncrement;				
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
					m_From.SendMessage("You remove 25 Amethysts");
                }
                else if (m_Key.Amethyst > 0)
                {
                    m_From.AddToBackpack(new Amethyst(m_Key.Amethyst));  					
                    m_From.SendMessage("You remove the last of your Amethysts");
					m_Key.Amethyst = 0;						     						
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
                }
                else
                {
                    m_From.SendMessage("You do not have any of these gems!");			
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));  				
           
                }
            }
			else if (info.ButtonID == 8)
			{
                if (m_Key.Tourmaline > m_Key.WithdrawIncrement)								
                {
                    m_From.AddToBackpack(new Tourmaline(m_Key.WithdrawIncrement));  	
					m_Key.Tourmaline = m_Key.Tourmaline - m_Key.WithdrawIncrement;				
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
					m_From.SendMessage("You remove 25 Tourmalines");
                }
                else if (m_Key.Tourmaline > 0)
                {
                    m_From.AddToBackpack(new Tourmaline(m_Key.Tourmaline));  					
                    m_From.SendMessage("You remove the last of your Tourmalines");
					m_Key.Tourmaline = 0;						     						
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
                }
                else
                {
                    m_From.SendMessage("You do not have any of these gems!");			
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));  				
                }
            }
			else if (info.ButtonID == 9)
			{
                if (m_Key.Amber > m_Key.WithdrawIncrement)								
                {
                    m_From.AddToBackpack(new Amber(m_Key.WithdrawIncrement));  	
					m_Key.Amber = m_Key.Amber - m_Key.WithdrawIncrement;				
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
					m_From.SendMessage("You remove 25 Ambers");
                }
                else if (m_Key.Amber > 0)
                {
                    m_From.AddToBackpack(new Amber(m_Key.Amber));  				
                    m_From.SendMessage("You remove the last of your Ambers");
					m_Key.Amber = 0;						     						
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
                }
                else
                {
                    m_From.SendMessage("You do not have any of these gems!");		
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));  			
                }
            }
				// ML Gems
            else if (info.ButtonID == 10)
            {
                if (m_Key.BlueDiamond > m_Key.WithdrawIncrement)								
                {
                    m_From.AddToBackpack(new BlueDiamond(m_Key.WithdrawIncrement));  	
					m_Key.BlueDiamond = m_Key.BlueDiamond - m_Key.WithdrawIncrement;				
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
					m_From.SendMessage("You remove 25 Blue Diamonds");
                }
                else if (m_Key.BlueDiamond > 0)
                {
                    m_From.AddToBackpack(new BlueDiamond(m_Key.BlueDiamond));  					
                    m_From.SendMessage("You remove the last of your Blue Diamonds");
					m_Key.BlueDiamond = 0;						     						
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
                }
                else
                {
                    m_From.SendMessage("You do not have any of these gems!");			
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));  				
              
                }
            }
			else if (info.ButtonID == 11)
			{
                if (m_Key.BrilliantAmber > m_Key.WithdrawIncrement)								
                {
                    m_From.AddToBackpack(new BrilliantAmber(m_Key.WithdrawIncrement));  	
					m_Key.BrilliantAmber = m_Key.BrilliantAmber - m_Key.WithdrawIncrement;				
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
					m_From.SendMessage("You remove 25 Brilliant Ambers");
                }
                else if (m_Key.BrilliantAmber > 0)
                {
                    m_From.AddToBackpack(new BrilliantAmber(m_Key.BrilliantAmber));  					
                    m_From.SendMessage("You remove the last of your Brilliant Ambers");
					m_Key.BrilliantAmber = 0;						     						
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
                }
                else
                {
                    m_From.SendMessage("You do not have any of these gems!");			
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));  				
                }
            }
			else if (info.ButtonID == 12)
			{
                if (m_Key.DarkSapphire > m_Key.WithdrawIncrement)								
                {
                    m_From.AddToBackpack(new DarkSapphire(m_Key.WithdrawIncrement));  	
					m_Key.DarkSapphire = m_Key.DarkSapphire - m_Key.WithdrawIncrement;				
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
					m_From.SendMessage("You remove 25 Dark Sapphires");
                }
                else if (m_Key.DarkSapphire > 0)
                {
                    m_From.AddToBackpack(new DarkSapphire(m_Key.DarkSapphire));  				
                    m_From.SendMessage("You remove the last of your Dark Sapphires");
					m_Key.DarkSapphire = 0;						     						
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));				
                }
                else
                {
                    m_From.SendMessage("You do not have any of these gems!");			
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));  				
                }
            }
			else if (info.ButtonID == 13)
			{
                if (m_Key.EcruCitrine > m_Key.WithdrawIncrement)								
                {
                    m_From.AddToBackpack(new EcruCitrine(m_Key.WithdrawIncrement));  	
					m_Key.EcruCitrine = m_Key.EcruCitrine - m_Key.WithdrawIncrement;				
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
					m_From.SendMessage("You remove 25 Ecru Citrines");
                }
                else if (m_Key.EcruCitrine > 0)
                {
                    m_From.AddToBackpack(new EcruCitrine(m_Key.EcruCitrine));  					
                    m_From.SendMessage("You remove the last of your Ecru Citrines");
					m_Key.EcruCitrine = 0;						     						
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));				
                }
                else
                {
                    m_From.SendMessage("You do not have any of these gems!");			
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));  				
                }
            }
			else if (info.ButtonID == 14)
			{
                if (m_Key.FireRuby > m_Key.WithdrawIncrement)								
                {
                    m_From.AddToBackpack(new FireRuby(m_Key.WithdrawIncrement));  	
					m_Key.FireRuby = m_Key.FireRuby - m_Key.WithdrawIncrement;				
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
					m_From.SendMessage("You remove 25 Fire Rubies");
                }
                else if (m_Key.FireRuby > 0)
                {
                    m_From.AddToBackpack(new FireRuby(m_Key.FireRuby));  				
                    m_From.SendMessage("You remove the last of your Fire Rubies");
					m_Key.FireRuby = 0;						     						
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
                }
                else
                {
                    m_From.SendMessage("You do not have any of these gems!");			
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));  				
                }
            }
			else if (info.ButtonID == 15)
			{
                if (m_Key.PerfectEmerald > m_Key.WithdrawIncrement)								
                {
                    m_From.AddToBackpack(new PerfectEmerald(m_Key.WithdrawIncrement));  	
					m_Key.PerfectEmerald = m_Key.PerfectEmerald - m_Key.WithdrawIncrement;				
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));				
					m_From.SendMessage("You remove 25 Perfect Emeralds");
                }
                else if (m_Key.PerfectEmerald > 0)
                {
                    m_From.AddToBackpack(new PerfectEmerald(m_Key.PerfectEmerald));  					
                    m_From.SendMessage("You remove the last of your Perfect Emeralds");
					m_Key.PerfectEmerald = 0;						     					
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));		
				}
                else
                {
                    m_From.SendMessage("You do not have any of these gems!");			
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));  				
                }
            }
			else if (info.ButtonID == 16)
			{
                if (m_Key.Turquoise > m_Key.WithdrawIncrement)							
                {
                    m_From.AddToBackpack(new Turquoise(m_Key.WithdrawIncrement));  	
					m_Key.Turquoise = m_Key.Turquoise - m_Key.WithdrawIncrement;				
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
					m_From.SendMessage("You remove 25 Turquoise");
                }
                else if (m_Key.Turquoise > 0)
                {
                    m_From.AddToBackpack(new Turquoise(m_Key.Turquoise));  					
                    m_From.SendMessage("You remove the last of your Turquoise");
					m_Key.Turquoise = 0;						     						
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
                }
                else
                {
                    m_From.SendMessage("You do not have any of these gems!");			
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));  				
                }
            }
			else if (info.ButtonID == 17)
			{
                if (m_Key.WhitePearl > m_Key.WithdrawIncrement)								
                {
                    m_From.AddToBackpack(new WhitePearl(m_Key.WithdrawIncrement));  	
					m_Key.WhitePearl = m_Key.WhitePearl - m_Key.WithdrawIncrement;				
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
					m_From.SendMessage("You remove 25 White Pearls");
                }
                else if (m_Key.WhitePearl > 0)
                {
                    m_From.AddToBackpack(new WhitePearl(m_Key.WhitePearl));  					
                    m_From.SendMessage("You remove the last of your White Pearls");
					m_Key.WhitePearl = 0;						     						
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));					
                }
                else
                {
                    m_From.SendMessage("You do not have any of these gems!");			
                    m_From.SendGump(new GemPouchGump(m_From, m_Key));  				
                }
            }
			else if (info.ButtonID == 18)
			{
				m_From.SendGump(new GemPouchGump(m_From, m_Key));
				m_Key.BeginCombine(m_From);
			
			}
		}
    }
}

namespace Server.Items
{
    public class GemPouchTarget : Target
    {
        private GemPouch m_Key;

        public GemPouchTarget(GemPouch key) : base( 18, false, TargetFlags.None )
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